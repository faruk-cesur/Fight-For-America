public interface IMovable<T>
{
    T MovementData { get; set; }
    void Move();
    void Stop();
    void Rotate();
}