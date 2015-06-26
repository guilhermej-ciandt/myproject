using System;
using Spring.Data.Generic;

namespace AG.Framework.Data.Ado
{
    /// <summary>
    /// Classe pai de todos os Daos da aplicação
    /// </summary>
    public abstract class AbstractBaseDao : AdoDaoSupport
    {
        public static String NAO = "N";
        public static String SIM = "S";

        public static string BooleanToSimNao(bool vbEntrada)
        {
            return vbEntrada ? SIM : NAO;
        }

        public static bool ToBoolean(string p)
        {
            return ToBoolean(p);
        }

    }
}
