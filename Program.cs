using System.Threading.Tasks;
using VirtualIoT;
using VirtualIoT.Models.Virtual;

class Program
{
    static async Task Main()
    {
        var mqtt = new MqttService();

        var device = new VirtualDevice("device1", mqtt);
        device.AddSensor(new VirtualTemperatureSensor());
        device.AddSensor(new VirtualHumiditySensor());

        await device.InitializeAsync();
        await device.RunAsync();
    }
}
