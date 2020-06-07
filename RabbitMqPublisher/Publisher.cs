using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp5
{
    public static class Publisher
    {
        static void Main(string[] args)
        {
            var hostName = "localhost";

            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri($"amqp://transcode_user:password@{hostName}/video.transcode.vhost"),                
            };

            var exchange = "videoreceived.exchange";
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var properties = channel.CreateBasicProperties();
                properties.Persistent = false;
                var propertiesDictionary = new Dictionary<string, object>();
                properties.Headers = propertiesDictionary;
                propertiesDictionary.Add("app_id", "Matlus Video site");
                propertiesDictionary.Add("content_type", "text/plain");

                do
                {
                    propertiesDictionary["message_id"] = Guid.NewGuid().ToString("N");
                    var messageBody = GetMessageBody();
                    var body = Encoding.UTF8.GetBytes(messageBody);
                    channel.BasicPublish(exchange: exchange, routingKey: string.Empty, properties, body: body);
                    Console.WriteLine($"Sent Message: {messageBody}");
                    Console.ReadLine();
                    
                }
                while (true);
            }
        }

        private static string GetMessageBody()
        {
            return $"date_time: {DateTime.UtcNow.ToString()}" +
                $"\r\ndata: This is the body of the message";
        }
    }
}
