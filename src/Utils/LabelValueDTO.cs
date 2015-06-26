namespace AG.Framework.Utils
{
    /// <summary>
    /// DTO utilitário para montagem de combos
    /// </summary>
    public class LabelValueDTO<T>
    {
        /// <summary>
        /// Label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        public T Value { get; set; }
    }
}
