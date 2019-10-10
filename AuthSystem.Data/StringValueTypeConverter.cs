using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using ValueOf;

namespace AuthSystem.Data
{
    public class StringValueTypeConverter<TWrapperType> : TypeConverter where TWrapperType : ValueOf<string, TWrapperType>, new()
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(TWrapperType))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return typeof(TWrapperType)
                    .GetMethod("From", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) // need FlattenHierarchy because From comes from ValueOf parent class
                    .Invoke(null, new object[] {value});
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(TWrapperType))
            {
                return typeof(TWrapperType)
                    .GetMethod("From", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) // need FlattenHierarchy because From comes from ValueOf parent class
                    .Invoke(null, new object[] {value});
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
