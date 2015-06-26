namespace AG.Framework.Log
{
    /// <summary>
    /// Toda classe que loga um evento deve implementar essa interface
    /// </summary>
    public interface ILoggable
    {
        LogEvent GetLog();
    }
}