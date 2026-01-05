public interface IVirtualActuator
{
    string Name { get; }
    object State { get; }
    void SetState(object value);
}
