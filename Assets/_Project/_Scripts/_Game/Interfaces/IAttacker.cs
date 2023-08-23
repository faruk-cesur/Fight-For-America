public interface IAttacker<T>
{
    T AttackerData { get; set; }
    void Attack();
}