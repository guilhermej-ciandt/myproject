using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AG.Framework.Utils
{
    public class DataReaderUtils
    {
        public static string GetString(IDataReader dataReader, string nomeDaColuna)
        {

            return Convert.IsDBNull(dataReader[nomeDaColuna])
                       ? null
                       : Convert.ToString(dataReader[nomeDaColuna]);
        }

        public static int? GetInt(IDataReader dataReader, string nomeDaColuna)
        {
            return Convert.IsDBNull(dataReader[nomeDaColuna])
                       ? (int?)null
                       : Convert.ToInt32(dataReader[nomeDaColuna]);
        }

        public static decimal? GetDecimal(IDataReader dataReader, string nomeDaColuna)
        {
            return Convert.IsDBNull(dataReader[nomeDaColuna])
                       ? (decimal?)null
                       : Convert.ToDecimal(dataReader[nomeDaColuna]);
        }

        public static bool GetBool(IDataReader dataReader, string nomeDaColuna)
        {
            var sn = GetString(dataReader, nomeDaColuna);
            return sn != null && sn.Equals("S");
        }

        public static DateTime? GetDateTime(IDataReader dataReader, string nomeDaColuna)
        {
            return Convert.IsDBNull(dataReader[nomeDaColuna])
                       ? (DateTime?)null
                       : Convert.ToDateTime(dataReader[nomeDaColuna]);
        }
    }
}
