public interface IVirtualSensor
{
    string Name { get; }
    object ReadValue();
}
