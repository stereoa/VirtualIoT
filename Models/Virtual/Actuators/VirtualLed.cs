public class VirtualLed : IVirtualActuator
{
    public string Name => "led";
    public bool IsOn { get; private set; } = false;
    public object State => IsOn;

    public void SetState(object value)
    {
        if (value is bool b)
        {
            IsOn = b;
            Console.WriteLine($"LED is now {(IsOn ? "ON" : "OFF")}");
        }
    }
}
