using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using UnityEngine;

namespace Aquila.Toolkit
{
    public partial class Tools
    {
        /// <summary>
        /// 通用字节写入工具类
        /// 支持向 byte[] 或 Stream 写入各种类型的数据
        /// </summary>
        public class ByteWriter : IReference, IDisposable, IAsyncDisposable
        {
            private Stream _stream;
            private bool _ownsStream;
            private bool _disposed;

            public ByteWriter()
            {
                _stream = new MemoryStream();
                _ownsStream = true;
            }

            public ByteWriter(int capacity)
            {
                _stream = new MemoryStream(capacity);
                _ownsStream = true;
            }

            public ByteWriter(Stream stream)
            {
                _stream = stream ?? throw new ArgumentNullException(nameof(stream));
                _ownsStream = false;
            }

            public long Position
            {
                get => _stream.Position;
                set => _stream.Position = value;
            }

            public long Length => _stream.Length;

            public void Seek(long position)
            {
                _stream.Seek(position, SeekOrigin.Begin);
            }

            public byte[] ToArray()
            {
                if (_stream is MemoryStream ms)
                    return ms.ToArray();
                
                throw new InvalidOperationException("Stream is not a MemoryStream");
            }

            public void WriteByte(byte value)
            {
                _stream.WriteByte(value);
            }

            public void WriteBytes(byte[] buffer)
            {
                if (buffer == null)
                    throw new ArgumentNullException(nameof(buffer));
                
                _stream.Write(buffer, 0, buffer.Length);
            }

            public void WriteBytes(byte[] buffer, int offset, int count)
            {
                if (buffer == null)
                    throw new ArgumentNullException(nameof(buffer));
                
                _stream.Write(buffer, offset, count);
            }

            public void WriteBoolean(bool value)
            {
                WriteByte((byte)(value ? 1 : 0));
            }

            public void WriteInt16(short value)
            {
                byte[] buffer = new byte[2];
                buffer[0] = (byte)value;
                buffer[1] = (byte)(value >> 8);
                WriteBytes(buffer);
            }

            public void WriteUInt16(ushort value)
            {
                byte[] buffer = new byte[2];
                buffer[0] = (byte)value;
                buffer[1] = (byte)(value >> 8);
                WriteBytes(buffer);
            }

            public void WriteInt32(int value)
            {
                byte[] buffer = new byte[4];
                buffer[0] = (byte)value;
                buffer[1] = (byte)(value >> 8);
                buffer[2] = (byte)(value >> 16);
                buffer[3] = (byte)(value >> 24);
                WriteBytes(buffer);
            }

            public void WriteUInt32(uint value)
            {
                byte[] buffer = new byte[4];
                buffer[0] = (byte)value;
                buffer[1] = (byte)(value >> 8);
                buffer[2] = (byte)(value >> 16);
                buffer[3] = (byte)(value >> 24);
                WriteBytes(buffer);
            }

            public void WriteInt64(long value)
            {
                byte[] buffer = new byte[8];
                buffer[0] = (byte)value;
                buffer[1] = (byte)(value >> 8);
                buffer[2] = (byte)(value >> 16);
                buffer[3] = (byte)(value >> 24);
                buffer[4] = (byte)(value >> 32);
                buffer[5] = (byte)(value >> 40);
                buffer[6] = (byte)(value >> 48);
                buffer[7] = (byte)(value >> 56);
                WriteBytes(buffer);
            }

            public void WriteUInt64(ulong value)
            {
                byte[] buffer = new byte[8];
                buffer[0] = (byte)value;
                buffer[1] = (byte)(value >> 8);
                buffer[2] = (byte)(value >> 16);
                buffer[3] = (byte)(value >> 24);
                buffer[4] = (byte)(value >> 32);
                buffer[5] = (byte)(value >> 40);
                buffer[6] = (byte)(value >> 48);
                buffer[7] = (byte)(value >> 56);
                WriteBytes(buffer);
            }

            public void WriteSingle(float value)
            {
                byte[] buffer = BitConverter.GetBytes(value);
                WriteBytes(buffer);
            }

            public void WriteDouble(double value)
            {
                byte[] buffer = BitConverter.GetBytes(value);
                WriteBytes(buffer);
            }

            public void WriteString(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    WriteInt32(0);
                    return;
                }
                
                byte[] buffer = Encoding.UTF8.GetBytes(value);
                WriteInt32(buffer.Length);
                WriteBytes(buffer);
            }

            public void WriteFixedString(string value, int length)
            {
                byte[] buffer = new byte[length];
                if (!string.IsNullOrEmpty(value))
                {
                    byte[] strBytes = Encoding.UTF8.GetBytes(value);
                    int copyLength = Math.Min(strBytes.Length, length);
                    Array.Copy(strBytes, buffer, copyLength);
                }
                
                WriteBytes(buffer);
            }

            public void WriteVector2(Vector2 value)
            {
                WriteSingle(value.x);
                WriteSingle(value.y);
            }

            public void WriteVector3(Vector3 value)
            {
                WriteSingle(value.x);
                WriteSingle(value.y);
                WriteSingle(value.z);
            }

            public void WriteVector4(Vector4 value)
            {
                WriteSingle(value.x);
                WriteSingle(value.y);
                WriteSingle(value.z);
                WriteSingle(value.w);
            }

            public void WriteColor(Color value)
            {
                WriteSingle(value.r);
                WriteSingle(value.g);
                WriteSingle(value.b);
                WriteSingle(value.a);
            }

            public void WriteColor32(Color32 value)
            {
                WriteByte(value.r);
                WriteByte(value.g);
                WriteByte(value.b);
                WriteByte(value.a);
            }

            public void WriteQuaternion(Quaternion value)
            {
                WriteSingle(value.x);
                WriteSingle(value.y);
                WriteSingle(value.z);
                WriteSingle(value.w);
            }

            public void Clear()
            {
                Dispose();
            }

            public void Dispose()
            {
                if (_disposed)
                    return;
                
                _disposed = true;
                
                if (_ownsStream && _stream != null)
                {
                    _stream.Dispose();
                }
                
                _stream = null;
            }

            public async ValueTask DisposeAsync()
            {
                if (_disposed)
                    return;
                
                _disposed = true;
                
                if (_ownsStream && _stream != null)
                {
                    await _stream.DisposeAsync();
                }
                
                _stream = null;
            }
        }
    }
}
