using System;

namespace DMarketSDK.Common.Lib.Utils
{
    public static class EnumExtensions
    {
        public static bool IsSet<TEnum>(this TEnum value, TEnum flags)
            where TEnum : struct
        {
            Type type = typeof(TEnum);

            // Only works with enums.
            if (!type.IsEnum)
            {
                throw new Exception("The type parameter TEnum must be an enum type.");
            }

            // Handle each underlying type.
            Type numberType = Enum.GetUnderlyingType(type);

            if (numberType == typeof(int))
            {
                int numberValue = Convert.ToInt32(value);
                int numberFlags = Convert.ToInt32(flags);
                return (numberValue & numberFlags) == numberFlags;
            }

            if (numberType == typeof(sbyte))
            {
                sbyte numberValue = Convert.ToSByte(value);
                sbyte numberFlags = Convert.ToSByte(flags);
                return (numberValue & numberFlags) == numberFlags;
            }

            if (numberType == typeof(byte))
            {
                byte numberValue = Convert.ToByte(value);
                byte numberFlags = Convert.ToByte(flags);
                return (numberValue & numberFlags) == numberFlags;
            }

            if (numberType == typeof(short))
            {
                short numberValue = Convert.ToInt16(value);
                short numberFlags = Convert.ToInt16(flags);
                return (numberValue & numberFlags) == numberFlags;
            }

            if (numberType == typeof(ushort))
            {
                ushort numberValue = Convert.ToUInt16(value);
                ushort numberFlags = Convert.ToUInt16(flags);
                return (numberValue & numberFlags) == numberFlags;
            }

            if (numberType == typeof(uint))
            {
                uint numberValue = Convert.ToUInt32(value);
                uint numberFlags = Convert.ToUInt32(flags);
                return (numberValue & numberFlags) == numberFlags;
            }

            if (numberType == typeof(long))
            {
                long numberValue = Convert.ToInt64(value);
                long numberFlags = Convert.ToInt64(flags);
                return (numberValue & numberFlags) == numberFlags;
            }

            if (numberType == typeof(ulong))
            {
                ulong numberValue = Convert.ToUInt64(value);
                ulong numberFlags = Convert.ToUInt64(flags);
                return (numberValue & numberFlags) == numberFlags;
            }

            if (numberType == typeof(char))
            {
                char numberValue = Convert.ToChar(value);
                char numberFlags = Convert.ToChar(flags);
                return (numberValue & numberFlags) == numberFlags;
            }

            throw new Exception(string.Format("Unknown enum underlying type {0}.", numberType.Name));
        }

        public static bool AnyFlagsSet<TEnum>(this TEnum value, TEnum flags)
            where TEnum : struct
        {
            Type type = typeof(TEnum);

            // Only works with enums.
            if (!type.IsEnum)
            {
                throw new Exception("The type parameter TEnum must be an enum type.");
            }

            // Handle each underlying type.
            Type numberType = Enum.GetUnderlyingType(type);

            if (numberType == typeof(int))
            {
                return (Convert.ToInt32(value) & Convert.ToInt32(flags)) != 0;
            }

            if (numberType == typeof(sbyte))
            {
                return (Convert.ToSByte(value) & Convert.ToSByte(flags)) != 0;
            }

            if (numberType == typeof(byte))
            {
                return (Convert.ToByte(value) & Convert.ToByte(flags)) != 0;
            }

            if (numberType == typeof(short))
            {
                return (Convert.ToInt16(value) & Convert.ToInt16(flags)) != 0;
            }

            if (numberType == typeof(ushort))
            {
                return (Convert.ToUInt16(value) & Convert.ToUInt16(flags)) != 0;
            }

            if (numberType == typeof(uint))
            {
                return (Convert.ToUInt32(value) & Convert.ToUInt32(flags)) != 0;
            }

            if (numberType == typeof(long))
            {
                return (Convert.ToInt64(value) & Convert.ToInt64(flags)) != 0;
            }

            if (numberType == typeof(ulong))
            {
                return (Convert.ToUInt64(value) & Convert.ToUInt64(flags)) != 0;
            }

            if (numberType == typeof(char))
            {
                return (Convert.ToChar(value) & Convert.ToChar(flags)) != 0;
            }

            throw new Exception(string.Format("Unknown enum underlying type {0}.", numberType.Name));
        }
    }
}