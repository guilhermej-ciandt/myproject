namespace AG.Framework.Utils
{
    /// <summary>
    /// Utilitário para montagem de "dicionários serializáveis"
    /// </summary>
    /// <typeparam name="T">Chave</typeparam>
    /// <typeparam name="TK">Valor</typeparam>
    public class KeyValue<T, TK>
    {
        /// <summary>
        /// Key
        /// </summary>
        public T Key { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public TK Value { get; set; }
    }
}
