using VirtualIoT;
using VirtualIoT.Models.Virtual.Sensors;

public class VirtualParkingSpotDevice : VirtualDevice
{
    public VirtualParkingSpotDevice(string deviceId, MqttService mqtt)
        : base(deviceId, mqtt, "parking") 
    {
        AddSensor(new VirtualOccupiedSensor());
        AddSensor(new VirtualTemperatureSensor());
    }
}
