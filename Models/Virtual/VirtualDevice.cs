using MQTTnet.Client;
using System.Text;
using System.Text.Json;
using VirtualIoT;

public class VirtualDevice
{
    private readonly string _deviceId;
    private readonly MqttService _mqtt;
    private readonly List<IVirtualSensor> _sensors = new();
    private readonly List<IVirtualActuator> _actuators = new();
    private readonly string _topicPrefix;

    public VirtualDevice(string deviceId, MqttService mqtt, string topicPrefix = "virtualiot")
    {
        _deviceId = deviceId;
        _mqtt = mqtt;
        _topicPrefix = topicPrefix;
    }

    public void AddSensor(IVirtualSensor sensor) => _sensors.Add(sensor);
    public void AddActuator(IVirtualActuator actuator) => _actuators.Add(actuator);

    public async Task InitializeAsync()
    {
        await _mqtt.ConnectAsync();
        await _mqtt.SubscribeAsync($"{_topicPrefix}/{_deviceId}/command");

        _mqtt.Client.UseApplicationMessageReceivedHandler(e =>
        {
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload).ToLower();

            foreach (var actuator in _actuators)
            {
                if (actuator.Name.ToLower() == payload)
                {
                    actuator.SetState(true);
                }
            }
        });
    }

    public async Task RunAsync()
    {
        Console.WriteLine($"[{_deviceId}] Device started");

        while (true)
        {
            var telemetry = new Dictionary<string, object>
            {
                ["deviceId"] = _deviceId,
                ["timestamp"] = DateTime.UtcNow
            };

            foreach (var sensor in _sensors)
            {
                telemetry[sensor.Name] = sensor.ReadValue();
            }

            foreach (var actuator in _actuators)
            {
                telemetry[actuator.Name] = actuator.State;
            }

            var payload = JsonSerializer.Serialize(telemetry);

            await _mqtt.PublishAsync($"{_topicPrefix}/{_deviceId}/telemetry", payload);

            Console.WriteLine($"[{_deviceId}] Telemetry: {payload}");

            Thread.Sleep(3000);
        }
    }
}
