using VirtualIoT;

class Program
{
    static async Task Main()
    {
        var mqtt = new MqttService("test.mosquitto.org", 1883);

        var spots = new List<VirtualParkingSpotDevice>();

        for (int i = 1; i <= 5; i++)
        {
            var spot = new VirtualParkingSpotDevice($"spot{i}", mqtt);

            await spot.InitializeAsync();
            spots.Add(spot);
        }

        var tasks = new List<Task>();
        foreach (var spot in spots)
        {
            tasks.Add(Task.Run(() => spot.RunAsync()));
        }

        await Task.WhenAll(tasks);
    }
}
