using System;

namespace SHLibrary.Utils
{
    public static class TypeExtensions
    {
        public static int GetBaseTypesCount(this Type type)
        {
            var count = 0;

            while (type.BaseType != null)
            {
                type = type.BaseType;
                count++;
            }

            return count;
        }
    }
}
