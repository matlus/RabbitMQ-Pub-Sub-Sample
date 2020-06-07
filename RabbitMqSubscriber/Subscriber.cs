using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMqSubscriber
{
    public static class Subscriber
    {
        static void Main(string[] args)
        {
            var hostName = "localhost";

            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri($"amqp://transcode_user:password@{hostName}/video.transcode.vhost"),
            };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                Console.WriteLine("Waiting for Messages...");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var appId = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["app_id"]);
                    var messageId = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["message_id"]);
                    var contentType = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["content_type"]);
                    Console.WriteLine($"App Id: {appId}\tMessage Id: {messageId}\tContent Type: {contentType}");                                         

                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine($"[x] {message}");
                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                };

                var queueName = "videoreceived.queue";
                var consumerTag = channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                Console.WriteLine(" Press [enter] to exit.");                
                Console.ReadLine();
                channel.BasicCancel(consumerTag);
            }
        }
    }
}
