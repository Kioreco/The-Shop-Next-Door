public interface IObjectPool
{
    public IContext GetPoolableObject();
    public void Release(IContext obj);
}
