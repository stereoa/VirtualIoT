using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace VirtualIoT
{
    public class MqttService
    {
        public IMqttClient Client { get; private set; }

        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly bool _useTls;

        public MqttService(string host, int port = 1883, string username = null, string password = null, bool useTls = false)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;
            _useTls = useTls;
        }

        public async Task ConnectAsync()
        {
            var factory = new MqttFactory();
            Client = factory.CreateMqttClient();

            var optionsBuilder = new MqttClientOptionsBuilder()
                .WithTcpServer(_host, _port);

            if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                optionsBuilder = optionsBuilder.WithCredentials(_username, _password);

            if (_useTls)
                optionsBuilder = optionsBuilder.WithTls();

            var options = optionsBuilder.Build();

            Client.UseConnectedHandler(_ =>
            {
                Console.WriteLine($"MQTT Connected to {_host}:{_port}");
            });

            Client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine($"MQTT Disconnected from {_host}:{_port}. Reason: {e.Reason}");
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
