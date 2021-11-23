using System;

namespace LandMassCreator
{
    /// <summary>
    /// Holds ExtensionsMethods
    /// </summary>
    static class ExtensionMethods
    {
        /// <summary>
        /// Clamp function example use: clampedVal = val.Clamp(min, max);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val">The value to be clamped</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns></returns>
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }
}