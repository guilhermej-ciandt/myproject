using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace AG.Framework.Dao.NHibernate.UserType
{
    [Serializable]
    public sealed class XmlSerializedType<T> : IEnhancedUserType
    {

        private static readonly XmlSerializer serializer = new XmlSerializer(
            typeof(T), "" );

        private static readonly SqlType[] sqlTypes = new SqlType[] { new SqlXmlType() };

        #region IEnhancedUserType Members

        bool IUserType.Equals(object x, object y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            var result = x.Equals(y);
            return result;
        }

        public object NullSafeGet(IDataReader data, string[] names, object owner)
        {
            return Unmarshal(NHibernateUtil.String.NullSafeGet(data, names[0]) as string);
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);
                return;
            }
            NHibernateUtil.String.NullSafeSet(cmd, Marshal(value), index);
        }

        public object DeepCopy(object value)
        {
            return value == null ? (object) null : BinarySerializeAndDeserialize((T) value);
        }

        public SqlType[] SqlTypes
        {
            get { return sqlTypes; }
        }

        public Type ReturnedType
        {
            get { return typeof(T); }
        }

        public bool IsMutable
        {
            get { return false; }
        }

        public object Assemble(object cached, object owner)
        {
            return DeepCopy(cached);
        }

        public object Disassemble(object value)
        {
            return DeepCopy(value);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object FromXMLString(string xml)
        {
            return Unmarshal(xml);
        }

        public string ObjectToSQLString(object value)
        {
            if (value == null)
            {
                return null;
            }
            return "'" + Marshal(value) + "'";
        }

        public string ToXMLString(object value)
        {
            return Marshal(value);
        }

        #endregion

        private static string Marshal(object value)
        {
            if (value == null)
            {
                return null;
            }
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, value);
                writer.Flush();
                return writer.ToString();
            }
        }

        private static object Unmarshal(string value)
        {
            if (value == null)
            {
                return null;
            }
            using (var reader = new StringReader(value))
            {
                return serializer.Deserialize(reader);
            }
        }

        private static T BinarySerializeAndDeserialize(T source)
        {
            var type = typeof (T);
            if (!type.IsSerializable) {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (ReferenceEquals(source, null)) {
                return default(T);
            }

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream()) {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T) formatter.Deserialize(stream);
            }
        }
    }

    public class SqlXmlType : SqlType
    {
        public SqlXmlType()
            : base(DbType.Xml)
        {
        }
    }

}