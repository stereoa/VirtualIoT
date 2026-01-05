namespace VirtualIoT.Models.Virtual
{
    public class VirtualLed
    {
        public bool IsOn { get; private set; } = false;

        public void Toggle() => IsOn = !IsOn;

        public void TurnOn() => IsOn = true;

        public void TurnOff() => IsOn = false;
    }
}
