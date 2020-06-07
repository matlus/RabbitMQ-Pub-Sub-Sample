using System;
using System.Threading.Tasks;
using System.Windows.Forms;

//// Rabbitmq Logo Icon by https://iconscout.com/contributors/icon-mafia

namespace RabbitMqPubSubControlUI
{
    public partial class Form1 : Form
    {
        private RabbitMqGateway _rabbitMqGateway = new RabbitMqGateway();
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            var messageCount = await _rabbitMqGateway.GetQueueMessageCount(txtUsername.Text, txtPassword.Text, txtHost.Text, 15672, txtVhost.Text, txtQueue.Text);
            lblMessageCount.Text = string.Format("{0:###,###,##0}", messageCount);
            timer1.Start();
        }
    }
}
