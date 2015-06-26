using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;

namespace AG.Framework.Web.Script.Serialization
{
    public class IDictionaryJavascriptConverter : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(IDictionary<string, string>) }));
            }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, 
                                           JavaScriptSerializer serializer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override IDictionary<string, object> Serialize(object obj, 
                                                              JavaScriptSerializer serializer)
        {
            IDictionary<string, string> dic = obj as Dictionary<string, string>;
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (dic != null)
            {
                if (dic.Count > 0)
                {
                    foreach (KeyValuePair<string, string> pair in dic)
                    {
                        result.Add(pair.Key, pair.Value);
                    }
                }
            }

            return result;
        }
    }
}