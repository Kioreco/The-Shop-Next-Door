public interface IObjectPool<T>
{
    public T GetPoolableObject();
    public void Release(T obj);
}
