using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;

namespace SilverCard.Rencode
{
    internal class Encoder : IDisposable
    {
        private BinaryWriter _Stream;
        public Encoding Encoding { get; private set; }

        public Encoder(Stream _stream, Encoding encoding)
        {
            _Stream = new BinaryWriter(_stream, encoding);
            Encoding = encoding;
        }

        public Encoder(Stream _stream) : this(_stream, Encoding.UTF8)
        {
        }

        public void WriteNull()
        {
            _Stream.Write((byte)TypeCode.Null);
        }

        public void WriteBytes(byte[] value)
        {
            WriteBytes(value, 0, value.Length);
        }

        public void WriteBytes(byte[] value, int offset, int length)
        {
            _Stream.Write(value, offset, length);
        }

        public void WriteBoolean(Boolean value)
        {
            _Stream.Write(value ? (byte)TypeCode.True : (byte)TypeCode.False);
        }

        public void WriteChar(Char value)
        {
            WriteByteValue(value);
        }

        public void WriteByteValue(int value)
        {
            _Stream.Write((byte)TypeCode.Byte);
            _Stream.Write(value);
        }

        public void WriteString(String value)
        {
            int len = value.Length;
            if (len < TypeCode.StringCount)
            {
                _Stream.Write((byte)(TypeCode.StringStart + len));
            }
            else
            {
                _Stream.Write(Encoding.GetBytes(len.ToString()));
                _Stream.Write((byte)TypeCode.LengthDelimiter);
            }

            _Stream.Write(Encoding.GetBytes(value));
        }

        public void WriteList(IList value)
        {
            Boolean useEndToken = value.Count >= TypeCode.ListCount;
            if (useEndToken)
            {
                _Stream.Write((byte)TypeCode.List);
            }
            else
            {
                _Stream.Write((byte)(TypeCode.ListStart + value.Count));
            }

            foreach (var item in value)
            {
                WriteObject(item);
            }

            if (useEndToken)
            {
                _Stream.Write((byte)TypeCode.End);
            }
        }

        public void WriteDicionary(IDictionary map)
        {
            Boolean untilEnd = map.Count >= TypeCode.DictionaryCount;

            if (untilEnd)
            {
                _Stream.Write((byte)TypeCode.Dictionary);
            }
            else
            {
                _Stream.Write((byte)(TypeCode.DictionaryStart + map.Count));
            }

            foreach (DictionaryEntry item in map)
            {
                WriteObject(item.Key);
                WriteObject(item.Value);
            }

            if (untilEnd)
            {
                _Stream.Write((byte)(TypeCode.End));
            }
        }

        public void WriteObject(Object value)
        {
            if (value == null)
            {
                WriteNull();
            }
            else if (value is byte[])
            {
                WriteBytes((byte[])value);
            }
            else if (value is Boolean)
            {
                WriteBoolean((Boolean)value);
            }
            else if (value is Char)
            {
                WriteChar((Char)value);
            }
            else if (value is Int16)
            {
                _Stream.Write((byte)TypeCode.Short);
                _Stream.Write(IPAddress.HostToNetworkOrder((Int16)value));
            }
            else if (value is Int32)
            {
                _Stream.Write((byte)TypeCode.Int);
                _Stream.Write(IPAddress.HostToNetworkOrder((int)value));
            }
            else if (value is Int64)
            {
                _Stream.Write((byte)TypeCode.Long);
                _Stream.Write(IPAddress.HostToNetworkOrder((Int64)value));
            }
            else if (value is float)
            {
                _Stream.Write((byte)TypeCode.Float);
                _Stream.Write((float)value);
            }
            else if (value is double)
            {
                _Stream.Write((byte)TypeCode.Double);
                _Stream.Write((double)value);
            }
            else if (value is String)
            {
                WriteString((String)value);
            }
            else if (value is IList)
            {
                WriteList((IList)value);
            }
            else if (value is IDictionary)
            {
                WriteDicionary((IDictionary)value);
            }
            else
            {
                throw new NotImplementedException("I can't encode this type.");
            }
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
        // ~Encoder() {
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
