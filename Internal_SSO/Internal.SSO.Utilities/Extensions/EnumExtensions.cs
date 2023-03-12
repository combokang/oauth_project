using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Internal.SSO.Utilities.Extensions
{
    public class Enum<T> where T : struct, IConvertible
    {
        public static IEnumerable<T> List
        {
            get
            {
                if (!typeof(T).IsEnum)
                    throw new ArgumentException("T must be an enumerated type");

                return Enum.GetValues(typeof(T)).Cast<T>();
            }
        }
    }

    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T value) where T : struct
        {
            FieldInfo field;
            DescriptionAttribute attribute;
            string result;

            field = value.GetType().GetField(value.ToString());
            attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            result = attribute != null ? attribute.Description : string.Empty;

            return result;
        }
    }
}
