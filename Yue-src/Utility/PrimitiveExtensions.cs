using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yue.Extensions
{
    public static class PrimitiveExtensions
    {
        #region STRING
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
        public static bool CompareTo(this string source, string toCheck, StringComparison comp)
        {
            switch (comp)
            {
                case StringComparison.OrdinalIgnoreCase:
                    return source.ToLower().CompareTo(toCheck.ToLower()) == 0;
                case StringComparison.Ordinal:
                    return source.CompareTo(toCheck) == 0;
            }

            return false;
        }
        #endregion

        #region CHAR
        public static bool CompareTo(this char source, char toCheck, StringComparison comp)
        {
            switch (comp)
            {
                case StringComparison.OrdinalIgnoreCase:
                    return char.ToUpperInvariant(source) == char.ToUpperInvariant(toCheck);
                case StringComparison.Ordinal:
                    return source.CompareTo(toCheck) == 0;
            }

            return false;
        }
        #endregion
    }
}