using System.Threading.Tasks;
using VirtualIoT;
using VirtualIoT.Models.Virtual;

class Program
{
    static async Task Main()
    {
        var mqtt = new MqttService("test.mosquitto.org", 1883);

        var device = new VirtualDevice("device1", mqtt);
        device.AddSensor(new VirtualTemperatureSensor());
        device.AddSensor(new VirtualHumiditySensor());

        await device.InitializeAsync();
        await device.RunAsync();
    }
}
