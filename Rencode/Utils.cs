using System;

namespace SilverCard.Rencode
{
    internal class Utils
    {
        // Character Encodings
        internal static String UTF_8 = "UTF-8";
        internal static String ISO_8859 = "ISO-8859-1";

        // Byte-lengths for types
        internal static int SHORT_BYTES = sizeof(short) / sizeof(byte);
        internal static int INTEGER_BYTES = sizeof(int) / sizeof(byte);
        internal static int LONG_BYTES = sizeof(long) / sizeof(byte);
        internal static int FLOAT_BYTES = sizeof(float) / sizeof(byte);
        internal static int DOUBLE_BYTES = sizeof(double) / sizeof(byte);

        // Maximum length of integer when written as base 10 string.
        internal static int MAX_INT_LENGTH = 64;

        private static Boolean IsInRage(int token, int start, int count)
        {
            return start <= token && token < (start + count);
        }

        internal static Boolean isFixedNumber(int token)
        {
            return isPositiveFixedNumber(token) || isNegativeFixedNumber(token);
        }

        internal static Boolean isPositiveFixedNumber(int token)
        {
            return IsInRage(token, TypeCode.PositiveIntegerStart, TypeCode.PositiveIntegerCount);
        }

        internal static Boolean isNegativeFixedNumber(int token)
        {
            return IsInRage(token, TypeCode.NegativeIntStart, TypeCode.NegativeIntCount);
        }

        internal static Boolean IsFixedList(int token)
        {
            return IsInRage(token, TypeCode.ListStart, TypeCode.ListCount);
        }

        internal static Boolean IsFixedDictionary(int token)
        {
            return IsInRage(token, TypeCode.DictionaryStart, TypeCode.DictionaryCount);
        }

        internal static Boolean IsFixedString(int token)
        {
            return IsInRage(token, TypeCode.StringStart, TypeCode.StringCount);
        }

        internal static Boolean IsDigit(int token)
        {
            return '0' <= token && token <= '9';
        }
    }
}
