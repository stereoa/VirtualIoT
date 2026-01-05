using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VirtualIoT;
using VirtualIoT.Models.Virtual;

public class VirtualDevice
{
    private readonly string _deviceId;
    private readonly MqttService _mqtt;
    private readonly List<IVirtualSensor> _sensors = new();
    private readonly VirtualLed _led = new();

    public VirtualDevice(string deviceId, MqttService mqtt)
    {
        _deviceId = deviceId;
        _mqtt = mqtt;
    }

    public void AddSensor(IVirtualSensor sensor)
    {
        _sensors.Add(sensor);
    }

    public async Task InitializeAsync()
    {
        await _mqtt.ConnectAsync();
        await _mqtt.SubscribeAsync($"virtualiot/{_deviceId}/led");

        _mqtt.Client.UseApplicationMessageReceivedHandler(e =>
        {
            var payload = Encoding.UTF8
                .GetString(e.ApplicationMessage.Payload)
                .ToLower();

            if (payload == "on") _led.TurnOn();
            if (payload == "off") _led.TurnOff();

            Console.WriteLine($"[{_deviceId}] LED {(_led.IsOn ? "ON" : "OFF")}");
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
                ["timestamp"] = DateTime.UtcNow,
                ["led"] = _led.IsOn
            };

            foreach (var sensor in _sensors)
                telemetry[sensor.Name] = sensor.ReadValue();

            var payload = JsonSerializer.Serialize(telemetry);

            await _mqtt.PublishAsync(
                $"virtualiot/{_deviceId}/telemetry",
                payload
            );

            Console.WriteLine($"[{_deviceId}] Telemetry sent");

            Thread.Sleep(3000);
        }
    }
}
