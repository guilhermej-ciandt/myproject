using System;
using System.Collections.Generic;
using System.Linq;

namespace AG.Framework.EnumUtils
{
    public static class EnumExtensionMethods
    {

        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            var type = value.GetType();
            
            // Get fieldinfo for this type
            var fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            var attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }

        public static IEnumerable<T> GetValues<T>(this Enum value)
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T StringToEnumCoverter<T>(string tipo)
        {
            return (T)StringEnum.Parse(typeof(T), tipo, true);
        }

        public static T ToEnum<T>(this string tipo)
        {
            return (T)StringEnum.Parse(typeof(T), tipo, true);
        }
    }
}
