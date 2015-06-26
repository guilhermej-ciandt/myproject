using System.Collections.Generic;
using System.Data;
using Common.Logging;

namespace AG.Framework.Utils
{
    public static class DataTableExtensioMethod
    {
        #region Logging

        private static readonly ILog Logger = LogManager.GetLogger(typeof(DataTableExtensioMethod));

        #endregion

        public static DataTable Delete(this DataTable table, string filter)
        {
            table.Select(filter).Delete();

            return table;
        }

        public static void Delete(this IEnumerable<DataRow> rows)
        {
            foreach (var row in rows)
                row.Delete();
        }

        /// <summary>
        /// Atualiza um único campos do DataTable apartir de um campo e valor
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="dictionary"></param>
        public static DataTable Update(this DataTable table, string filter, string field, object value)
        {
            table.Select(filter).Update(field, value);

            return table;
        }

        public static void Update(this IEnumerable<DataRow> rows, string field, object value)
        {
            foreach (var row in rows)
                row[field] = value;
        }

        /// <summary>
        /// Atualiza um conjunto de campos do DataTable apartir de um dicionário
        /// O(N*M) - take careful!!
        ///   N = Size of rows
        ///   M = Size of dictionary
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="dictionary">Nome do campo a ser atulizado é chave do dicionário (campo, valor)</param>
        public static DataTable Update(this DataTable table, string filter, IDictionary<string, object> dictionary)
        {
            table.Select(filter).Update(dictionary);

            return table;
        }

        public static void Update(this IEnumerable<DataRow> rows, IDictionary<string, object> dictionary)
        {
            foreach (var row in rows)
                foreach (var data in dictionary)
                    row[data.Key] = data.Value;
        }

        public static void AddRange(this DataTable table, IEnumerable<DataRow> rows)
        {
            foreach (var row in rows)
                table.ImportRow(row);
        }

    }
}