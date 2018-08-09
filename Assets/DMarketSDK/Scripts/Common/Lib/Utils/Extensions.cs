using SHLibrary.Logging;
using System;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace SHLibrary.Utils
{
    public static class Extensions
    {
        public static void SafeRaise(this Action action)
        {
            var a = action;
            if (a != null)
            {
                a.Invoke();
            }
        }

        public static void SafeRaise<T>(this Action<T> action, T arg)
        {
            var a = action;
            if (a != null)
            {
                a.Invoke(arg);
            }
        }

        public static void SafeRaise<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            var a = action;
            if (a != null)
            {
                a.Invoke(arg1, arg2);
            }
        }

        public static void SafeRaise<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            var a = action;
            if (a != null)
            {
                a.Invoke(arg1, arg2, arg3);
            }
        }

        public static void SafeRaise<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2,
            T3 arg3, T4 arg4)
        {
            var a = action;
            if (a != null)
            {
                a.Invoke(arg1, arg2, arg3, arg4);
            }
        }

        /// <summary>
        ///     <para>Creates a log-string from the Exception.</para>
        ///     <para>
        ///         The result includes the stack trace, inner exception and data, separated by
        ///         <seealso cref="Environment.NewLine" />.
        ///     </para>
        /// </summary>
        /// <param name="exception">The exception to create the string from.</param>
        /// <param name="additionalMessage">Additional message to place at the top of the string, maybe be empty or null.</param>
        /// <returns></returns>
        public static string ToLogString(this Exception exception, string additionalMessage = null)
        {
            var msg = new StringBuilder();

            if (!string.IsNullOrEmpty(additionalMessage))
            {
                msg.AppendFormat("{0}{1}", additionalMessage, Environment.NewLine);
            }

            if (exception != null)
            {
                Exception orgEx = exception;

                msg.AppendFormat("Exception:{0}", Environment.NewLine);
                while (orgEx != null)
                {
                    msg.AppendFormat("{0}{1}", orgEx.Message, Environment.NewLine);
                    orgEx = orgEx.InnerException;
                }

                foreach (object value in exception.Data)
                {
                    msg.AppendFormat("Data:{1}{0}", Environment.NewLine, value);
                }

                if (exception.StackTrace != null)
                {
                    msg.AppendFormat("StackTrace:{0}{1}{0}", Environment.NewLine, exception.StackTrace);
                }

                if (exception.Source != null)
                {
                    msg.AppendFormat("Source:{0}{1}{0}", Environment.NewLine, exception.Source);
                }

                if (exception.TargetSite != null)
                {
                    msg.AppendFormat("TargetSite:{0}{1}{0}", Environment.NewLine, exception.TargetSite);
                }

                msg.AppendFormat("BaseException:{0}{1}", Environment.NewLine, exception.GetBaseException());
            }
            return msg.ToString();
        }

        /// <summary>
        ///     Tries to cast <paramref name="obj" /> to type <typeparamref name="T" />.
        ///     Returns default value of type <typeparamref name="T" /> if the <paramref name="obj" /> cannot be casted to type
        ///     <typeparam name="T" />
        ///     .
        /// </summary>
        /// <typeparam name="T">
        ///     The type to which the <paramref name="obj" /> has to be casted.
        /// </typeparam>
        /// <param name="obj">
        ///     The object to cast.
        /// </param>
        /// <returns>
        ///     New instance of the
        ///     <typeparam name="T" />
        ///     type if <paramref name="obj" /> is casted;
        ///     otherwise, default value of type <typeparamref name="T" />.
        /// </returns>
        public static T To<T>(this object obj)
        {
            // ReSharper disable once IntroduceOptionalParameters.Global
            return To(obj, default(T));
        }

        /// <summary>
        ///     Tries to cast <paramref name="obj" /> to type <typeparamref name="T" />.
        ///     Returns <paramref name="defaultValue" /> if the <paramref name="obj" /> cannot be casted to type
        ///     <typeparam name="T" />
        ///     .
        /// </summary>
        /// <typeparam name="T">
        ///     The type to which the <paramref name="obj" /> has to be casted.
        /// </typeparam>
        /// <param name="obj">
        ///     The object to cast.
        /// </param>
        /// <param name="defaultValue">
        ///     The value to return if <paramref name="obj" /> is not casted.
        /// </param>
        /// <returns>
        ///     New instance of the
        ///     <typeparam name="T" />
        ///     type if <paramref name="obj" /> is casted;
        ///     otherwise, <paramref name="defaultValue" />.
        /// </returns>
        public static T To<T>(this object obj, T defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            if (obj is T)
            {
                return (T)obj;
            }

            Type type = typeof(T);

            // Place convert to reference types here
            if (type == typeof(string))
            {
                return (T)(object)obj.ToString();
            }

            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                return To(obj, defaultValue, underlyingType);
            }

            return To(obj, defaultValue, type);
        }

        /// <summary>
        ///     Converts the 32-bit signed integer value to its
        ///     <typeparam name="TEnum" />
        ///     equivalent.
        /// </summary>
        /// <typeparam name="TEnum">
        ///     The target enum type.
        /// </typeparam>
        /// <param name="intValue">
        ///     The value to convert.
        /// </param>
        /// <param name="result">
        ///     When this method returns, contains the
        ///     <typeparam name="TEnum" />
        ///     equivalent of the converted 32-bit signed value,
        ///     if the conversion succeeded, or default enum value if the conversion is failed.
        ///     The conversion fails if the 32-bit signed integer value cannot be parsed to
        ///     <typeparam name="TEnum" />
        ///     ,
        ///     <typeparam name="TEnum" />
        ///     is not enum type.
        /// </param>
        /// <returns>
        ///     A value indicating whether the conversion is succeeded.
        /// </returns>
        public static bool TryParseToEnum<TEnum>(this int intValue, out TEnum result)
        {
            var isParsed = false;
            result = default(TEnum);

            Type type = typeof(TEnum);
            if (type.IsEnum)
            {
                try
                {
                    result = (TEnum)Enum.Parse(typeof(TEnum), intValue.ToString(CultureInfo.InvariantCulture), false);
                    isParsed = true;
                }
                catch (Exception)
                {
                    result = default(TEnum);
                }
            }

            return isParsed;
        }

        /// <summary>
        ///     Converts the 64-bit signed integer value to its
        ///     <typeparam name="TEnum" />
        ///     equivalent.
        /// </summary>
        /// <typeparam name="TEnum">
        ///     The target enum type.
        /// </typeparam>
        /// <param name="intValue">
        ///     The value to convert.
        /// </param>
        /// <param name="result">
        ///     When this method returns, contains the
        ///     <typeparam name="TEnum" />
        ///     equivalent of the converted 64-bit signed value,
        ///     if the conversion succeeded, or default enum value if the conversion is failed.
        ///     The conversion fails if the 64-bit signed integer value cannot be parsed to
        ///     <typeparam name="TEnum" />
        ///     ,
        ///     <typeparam name="TEnum" />
        ///     is not enum type.
        /// </param>
        /// <returns>
        ///     A value indicating whether the conversion is succeeded.
        /// </returns>
        public static bool TryParseToEnum<TEnum>(this long intValue, out TEnum result)
        {
            var isParsed = false;
            result = default(TEnum);

            Type type = typeof(TEnum);
            if (type.IsEnum)
            {
                try
                {
                    result = (TEnum)Enum.Parse(typeof(TEnum), intValue.ToString(CultureInfo.InvariantCulture), false);
                    isParsed = true;
                }
                catch (Exception)
                {
                    result = default(TEnum);
                }
            }

            return isParsed;
        }

        private static T To<T>(object obj, T defaultValue, Type type)
        {
            if (type.IsEnum)
            {
                object o = Convert.ChangeType(obj, Enum.GetUnderlyingType(typeof(T)));

                if (o != null)
                {
                    try
                    {
                        return (T)Enum.Parse(type, obj.ToString());
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }

                return defaultValue;
            }

            try
            {
                return (T)Convert.ChangeType(obj, type);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }

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

        /// <summary>
        ///     Call <see cref="AssignToParent{TComponent}" /> for <see cref="parent" />'s transform.
        /// </summary>
        /// <typeparam name="TComponent">Type of assigned component.</typeparam>
        /// <param name="component">Component to assign.</param>
        /// <param name="parent">Parent component.</param>
        /// <param name="resetLocalData">
        ///     If true reset <see cref="Transform.localPosition" /> ,
        ///     <see cref="Transform.localScale" /> ,
        ///     <see cref="Transform.localRotation" /> to it's default values.
        /// </param>
        /// <returns>The same <see cref="component" /> but already assigned to <see cref="parent" /></returns>
        /// <seealso cref="AssignToParent{TComponent}" />
        public static TComponent AssignTo<TComponent>(
            this TComponent component,
            MonoBehaviour parent,
            bool resetLocalData = true) where TComponent : MonoBehaviour
        {
            return AssignToParent(component, parent.transform, resetLocalData);
        }

        /// <summary>
        ///     Assing <see cref="component" /> to <see cref="parentTransform" /> transform.
        /// </summary>
        /// <typeparam name="TComponent">Type of assigned component.</typeparam>
        /// <param name="component">Component to assign.</param>
        /// <param name="parentTransform">Parent component.</param>
        /// <param name="resetLocalData">
        ///     If true reset <see cref="Transform.localPosition" /> ,
        ///     <see cref="Transform.localScale" /> ,
        ///     <see cref="Transform.localRotation" /> to it's default values.
        /// </param>
        /// <returns>The same <see cref="component" /> but already assigned to <see cref="parentTransform" /></returns>
        public static TComponent AssignToParent<TComponent>(
            this TComponent component,
            Transform parentTransform,
            bool resetLocalData) where TComponent : MonoBehaviour
        {
            Transform t = component.transform;

            t.parent = parentTransform;

            if (resetLocalData)
            {
                t.localPosition = Vector3.zero;
                t.localScale = Vector3.one;
                t.localRotation = Quaternion.identity;
            }
            else
            {
                Vector3 position = t.localPosition;
                Vector3 scale = t.localScale;
                Quaternion rotation = t.localRotation;

                t.localPosition = position;
                t.localScale = scale;
                t.localRotation = rotation;
            }

            return component;
        }

        public static TComponent GetSafeComponent<TComponent>(this Transform transform) where TComponent : MonoBehaviour
        {
            return GetSafeComponent<TComponent>(transform.gameObject);
        }

        public static TComponent GetSafeComponent<TComponent>(this MonoBehaviour behaviour)
            where TComponent : MonoBehaviour
        {
            return GetSafeComponent<TComponent>(behaviour.gameObject);
        }

        public static TComponent GetSafeComponent<TComponent>(this GameObject obj) where TComponent : MonoBehaviour
        {
            var component = obj.GetComponent<TComponent>();

            if (component == null)
            {
                Debug.LogError(
                    string.Format("Expected to find component of type {0} but not found", typeof(TComponent)),
                    obj);

                component = obj.AddComponent<TComponent>();
            }

            return component;
        }

        /*    public static TComponent InstantiateAndGetSafeComponent<TComponent>(
            this RelativeLinkToGameObject relativeLinkToGameObject,
            Vector3? position = null,
            Quaternion? rotation = null) where TComponent : MonoBehaviour
        {
            GameObject go = relativeLinkToGameObject;
            return go.InstantiateAndGetSafeComponent<TComponent>(position, rotation);
        }*/

        public static TComponent InstantiateAndGetSafeComponent<TComponent>(
            this GameObject objectToInstantiate,
            Vector3? position = null,
            Quaternion? rotation = null) where TComponent : MonoBehaviour
        {
            if (objectToInstantiate != null)
            {
                GameObject o = UnityEngine.Object.Instantiate(
                                   objectToInstantiate,
                                   position != null ? position.Value : Vector3.zero,
                                   rotation != null ? rotation.Value : Quaternion.identity);

                if (o != null)
                {
                    return o.GetSafeComponent<TComponent>();
                }
            }
            else
            {
                DevLogger.Error(
                    string.Format("Instantiated object with component {0} is null", typeof(TComponent)),
                    LogChannel.Common);
            }

            DevLogger.Error("Couldn't instantiate object", LogChannel.Common);

            return default(TComponent);
        }

        public static void SetRotationX(this Transform transform, float x)
        {
            Quaternion r = transform.localRotation;
            transform.localRotation = new Quaternion(x, r.y, r.z, r.w);
        }

        public static void SetRotationY(this Transform transform, float y)
        {
            Quaternion r = transform.localRotation;
            transform.localRotation = new Quaternion(r.x, y, r.z, r.w);
        }

        public static void SetX(this Transform transform, float x)
        {
            Vector3 p = transform.position;
            transform.position = new Vector3(x, p.y, p.z);
        }

        public static void SetY(this Transform transform, float y)
        {
            Vector3 p = transform.position;
            transform.position = new Vector3(p.x, y, p.z);
        }

        public static void SetZ(this Transform transform, float z)
        {
            Vector3 p = transform.position;
            transform.position = new Vector3(p.x, p.y, z);
        }

        public static string ToLogString(this Vector3 vector3)
        {
            return string.Format("[{0},{1},{2}]", (int)vector3.x, (int)vector3.y, (int)vector3.z);
        }

        public static float[] ToFloats(this Vector3 vector3)
        {
            var result = new float[3];

            result[0] = vector3.x;
            result[1] = vector3.z;
            result[2] = vector3.y;

            return result;
        }

        public static float[] ToFloats(this Quaternion quaternion)
        {
            var rotationValue = new float[4];

            rotationValue[0] = quaternion.x;
            rotationValue[1] = quaternion.y;
            rotationValue[2] = quaternion.z;
            rotationValue[3] = quaternion.w;

            return rotationValue;
        }

        public static Quaternion ToQuanternion(this float[] floats)
        {
            if (floats != null && floats.Length >= 4)
            {
                return new Quaternion(floats[0], floats[1], floats[2], floats[3]);
            }

            return Quaternion.identity;
        }

        public static Quaternion FromEulerToQuanternion(this float[] floats)
        {
            if (floats != null && floats.Length >= 3)
            {
                return Quaternion.Euler(floats[0], floats[1], floats[2]);
            }

            return Quaternion.identity;
        }

        public static Vector3 ToVector3(this float[] floats)
        {
            if (floats != null && floats.Length >= 3)
            {
                return new Vector3 { x = floats[0], z = floats[1], y = floats[2] };
            }

            return Vector3.zero;
        }

        public static Vector3 Merge(this Vector3 a, Vector3 b, Vector3 bm)
        {
            Vector3 am = Vector3.one - bm;
            a = Vector3.Scale(a, am);
            b = Vector3.Scale(b, bm);
            return a + b;
        }
    }
}