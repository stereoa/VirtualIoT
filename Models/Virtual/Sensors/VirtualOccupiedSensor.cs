public class VirtualOccupiedSensor : IVirtualSensor
{
    private readonly Random _rand = new();
    private bool _occupied = false;

    public string Name => "occupied";

    public object ReadValue()
    {
        if (_rand.NextDouble() < 0.3)
        {
            _occupied = !_occupied;
        }

        return _occupied;
    }
}
