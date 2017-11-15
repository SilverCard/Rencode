namespace SilverCard.Rencode
{
    internal class TypeCode
    {
        // The bencode 'typecodes' such as i, d, etc have been
        // extended and relocated on the base-256 character set.
        public const int List = 59;
        public const int Dictionary = 60;
        public const int Number = 61;
        public const int Byte = 62;
        public const int Short = 63;
        public const int Int = 64;
        public const int Long = 65;
        public const int Float = 66;
        public const int Double = 44;
        public const int True = 67;
        public const int False = 68;
        public const int Null = 69;
        public const int End = 127;
        public const int LengthDelimiter = ':';

        // Positive integers
        public const int PositiveIntegerStart = 0;
        public const int PositiveIntegerCount = 44;

        // Negative integers
        public const int NegativeIntStart = 70;
        public const int NegativeIntCount = 32;

        // Dictionaries
        public const int DictionaryStart = 102;
        public const int DictionaryCount = 25;

        // Strings
        public const int StringStart = 128;
        public const int StringCount = 64;

        // Lists
        public const int ListStart = StringStart + StringCount;
        public const int ListCount = 64;

    }
}
