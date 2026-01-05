namespace VirtualIoT.Models.Virtual.Sensors
{
    public class VirtualTemperatureSensor : IVirtualSensor
    {
        private readonly Random _rand = new();

        public string Name => "temperature";

        public object ReadValue()
            => Math.Round(20 + _rand.NextDouble() * 10, 2);
    }

}
