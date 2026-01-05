using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Text;
using System.Threading.Tasks;

namespace VirtualIoT
{
    public class MqttService
    {
        public IMqttClient Client { get; private set; }

        public async Task ConnectAsync()
        {
            var factory = new MqttFactory();
            Client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("test.mosquitto.org", 1883)
                .Build();

            Client.UseConnectedHandler(_ =>
            {
                Console.WriteLine("MQTT Connected");
            });

            Client.UseApplicationMessageReceivedHandler(e =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($"Received [{e.ApplicationMessage.Topic}]: {payload}");
            });

            await Client.ConnectAsync(options);
        }

        public async Task PublishAsync(string topic, string message)
        {
            var msg = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .Build();

            await Client.PublishAsync(msg);
        }

        public async Task SubscribeAsync(string topic)
        {
            await Client.SubscribeAsync(topic);
            Console.WriteLine($"Subscribed to {topic}");
        }
    }
}
