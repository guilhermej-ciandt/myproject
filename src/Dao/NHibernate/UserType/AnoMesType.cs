using System;
using System.Data;
using AG.Framework.Domain;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace AG.Framework.Dao.NHibernate.UserType
{
    /// <summary>
    /// Tipo AnoMes para atributos AnoMes que sao armazenados como
    /// inteiros no banco de dados
    /// </summary>
    [Serializable]
    public class AnoMesType : IUserType
    {
        #region IUserType Members

        public bool IsMutable
        {
            get { return false; }
        }

        public Type ReturnedType
        {
            get { return typeof(AnoMesType); }
        }

        public SqlType[] SqlTypes
        {
            get { return new[] { NHibernateUtil.Int32.SqlType }; }
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var obj = NHibernateUtil.Int32.NullSafeGet(rs, names[0]);

            if (obj == null) return null;

            AnoMes anoMes;

            try
            {
                anoMes = new AnoMes((int)obj);
            }
            catch (ArgumentException e)
            {
                throw new HibernateException(e.Message);
            }

            return anoMes;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                var anoMes = (AnoMes)value;
                ((IDataParameter)cmd.Parameters[index]).Value = anoMes.Value;
            }
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (x == null || y == null) return false;

            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x == null ? typeof(bool).GetHashCode() + 473 : x.GetHashCode();
        }

        #endregion
    }

}
