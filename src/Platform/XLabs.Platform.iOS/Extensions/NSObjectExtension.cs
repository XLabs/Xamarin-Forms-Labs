using Foundation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace XLabs.Platform.Extensions
{
    /// <summary>
    /// Extension method to transform an NS object into a plain .NET object
    /// </summary>
    public static class NSObjectExtension
    {
        /// <summary>
        /// Transform an NSObject into a .Net object accordint to parameter, 
        /// The transformation handle nullable struct
        /// </summary>
        /// <param name="nsO"></param>
        /// <param name="targetType">Target type</param>
        /// <returns>The .Net object</returns>
        public static Object ToObject(this NSObject nsO, Type targetType)
        {

            bool isNullable = Nullable.GetUnderlyingType(targetType) != null;
            if (isNullable)
                targetType = Nullable.GetUnderlyingType(targetType);


            if (nsO is NSString)
            {
                if (targetType == typeof(string))
                    return nsO.ToString();
                if (targetType == typeof(char))
                    return nsO.ToString()[0];

            }

           


            if (nsO is NSDate)
            {
                DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0);
                DateTime currentDate = reference.AddSeconds(((NSDate)nsO).SecondsSinceReferenceDate);
                DateTime localDate = currentDate.ToLocalTime();
                return localDate;
            }

            if (nsO is NSDecimalNumber)
            {
                return (isNullable) ? new Nullable<decimal>(decimal.Parse(nsO.ToString(), CultureInfo.InvariantCulture)) : decimal.Parse(nsO.ToString(), CultureInfo.InvariantCulture);
            }

            if (nsO is NSNumber)
            {
                var x = (NSNumber)nsO;

                switch (Type.GetTypeCode(targetType))
                {
                    case TypeCode.Boolean:
                        return (isNullable) ? (new Nullable<bool>(x.BoolValue)) : (x.BoolValue);
                    case TypeCode.Char:
                        return (isNullable) ? new Nullable<char>(Convert.ToChar(x.ByteValue)) : Convert.ToChar(x.ByteValue);
                    case TypeCode.SByte:
                        return (isNullable) ? new Nullable<sbyte>(x.SByteValue) : x.SByteValue;
                    case TypeCode.Byte:
                        return (isNullable) ? new Nullable<byte>(x.ByteValue) : x.ByteValue;
                    case TypeCode.Int16:
                        return (isNullable) ? new Nullable<Int16>(x.Int16Value) : x.Int16Value;
                    case TypeCode.UInt16:
                        return (isNullable) ? new Nullable<UInt16>(x.UInt16Value) : x.UInt16Value;
                    case TypeCode.Int32:
                        return (isNullable) ? new Nullable<Int32>(x.Int32Value) : x.Int32Value;
                    case TypeCode.UInt32:
                        return (isNullable) ? new Nullable<UInt32>(x.UInt32Value) : x.UInt32Value;
                    case TypeCode.Int64:
                        return (isNullable) ? new Nullable<Int64>(x.Int64Value) : x.Int64Value;
                    case TypeCode.UInt64:
                        return (isNullable) ? new Nullable<UInt64>(x.UInt64Value) : x.UInt64Value;
                    case TypeCode.Single:
                        return (isNullable) ? new Nullable<float>(x.FloatValue) : x.FloatValue;
                    case TypeCode.Double:
                        return (isNullable) ? new Nullable<double>(x.DoubleValue) : x.DoubleValue;
                }
            }

            if (nsO is NSValue)
            {
                var v = (NSValue)nsO;

                if (targetType == typeof(IntPtr))
                {
                    return v.PointerValue;
                }

                if (targetType == typeof(SizeF))
                {
                    return v.SizeFValue;
                }

                if (targetType == typeof(RectangleF))
                {
                    return v.RectangleFValue;
                }

                if (targetType == typeof(PointF))
                {
                    return v.PointFValue;
                }
            }

            return nsO;
        }
    }
}
