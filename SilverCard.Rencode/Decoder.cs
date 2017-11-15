using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SilverCard.Rencode
{
    /// <summary>
    /// Decode rencoded bytes.
    /// </summary>
    internal class Decoder : IDisposable
    {
        private BinaryReader _Stream;
        public Encoding Encoding { get; private set; }

        public Decoder(Stream _stream, Encoding encoding)
        {
            if (_stream == null) throw new ArgumentNullException(nameof(_stream));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            _Stream = new BinaryReader(_stream, encoding);
            Encoding = encoding;
        }

        public Decoder(Stream _stream) : this(_stream, Encoding.UTF8)
        {
        }

        public Object ReadObject()
        {
            return ReadObject(ReadToken());
        }

        private List<Object> ReadList()
        {
            List<Object> list = new List<Object>();
            int token = -1;
            while ((token = ReadToken()) != TypeCode.End)
            {
                list.Add(ReadObject(token));
            }
            return list;
        }

        protected Object ReadObject(int token)
        {
            if (token == TypeCode.Dictionary)
            {
                return ReadDicionary();
            }
            else if (Utils.IsFixedDictionary(token))
            {
                return ReadFixedDicionary(token);
            }
            else if (token == TypeCode.List)
            {
                return ReadList();
            }
            else if (Utils.IsFixedList(token))
                return ReadFixedList(token);
            else if (token == TypeCode.Byte)
                return _Stream.ReadByte();
            else if (token == TypeCode.Short)
                return BitConverter.ToInt16(ReadAsBigEndian(2), 0);
            else if (token == TypeCode.Int)
                return BitConverter.ToInt32(ReadAsBigEndian(4), 0);
            else if (token == TypeCode.Long)
                return BitConverter.ToInt64(ReadAsBigEndian(8), 0);
            else if (token == TypeCode.Float)
                return _Stream.ReadSingle();
            else if (token == TypeCode.Double)
                return _Stream.ReadDouble();
            else if (token == TypeCode.Number)
                return _Stream.ReadDecimal();
            else if (Utils.isNegativeFixedNumber(token))
            {
                return TypeCode.NegativeIntStart - 1 - token;
            }
            else if (Utils.isPositiveFixedNumber(token))
            {
                return TypeCode.PositiveIntegerStart + token;
            }
            else if (token == TypeCode.False)
            {
                return false;
            }
            else if (token == TypeCode.True)
            {
                return true;
            }
            else if (token == TypeCode.Null)
            {
                return null;
            }
            else if (Utils.IsDigit(token) || Utils.IsFixedString(token))
            {
                return ReadString(token);
            }

            throw new NotImplementedException($"I can't decode token {token}.");
        }


        private Dictionary<String, Object> ReadDicionary()
        {
            var dic = new Dictionary<String, Object>();
            int token = -1;

            while ((token = ReadToken()) != TypeCode.End)
            {
                String key = ReadString(token);
                Object value = ReadObject();
                dic.Add(key, value);
            }

            return dic;
        }
        private Dictionary<String, Object> ReadFixedDicionary(int token)
        {
            var dic = new Dictionary<String, Object>();
            int count = token - TypeCode.DictionaryStart;

            if (count < 0) throw new InvalidOperationException("Dicionary count is negative.");

            for (int i = 0; i < count; i++)
            {
                String key = ReadString(ReadToken());
                Object value = ReadObject();
                dic.Add(key, value);
            }

            return dic;
        }

        public int ReadToken()
        {
            return _Stream.ReadByte();
        }

        private List<Object> ReadFixedList(int token)
        {
            List<Object> list = new List<Object>();
            int length = token - TypeCode.ListStart;

            if (length < 0) throw new InvalidOperationException("Length of list is negative.");

            for (int i = 0; i < length; i++)
            {
                list.Add(ReadObject());
            }
            return list;
        }

        private byte[] ReadAsBigEndian(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            byte[] bytes = _Stream.ReadBytes(count);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }

        private String ReadString(int token)
        {
            if (Utils.IsFixedString(token))
            {
                int length = token - TypeCode.StringStart;
                return this.Encoding.GetString(_Stream.ReadBytes(length));
            }
            return this.Encoding.GetString(ReadBytesValue(token));
        }

        private byte[] ReadBytesValue(int token)
        {
            return _Stream.ReadBytes(ReadLength(token));
        }

        private int ReadLength(int token)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append((char)token);

            while ((token = ReadToken()) != TypeCode.LengthDelimiter)
            {
                buffer.Append((char)token);
            }

            int len = int.Parse(buffer.ToString());
            if (len < 0) throw new InvalidOperationException("Length is negative.");
            return len;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_Stream != null) _Stream.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Decoder() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}