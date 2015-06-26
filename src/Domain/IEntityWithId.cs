namespace AG.Framework.Domain
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityWithId<T>
    {
        T Id { get; set; }
    }
}
