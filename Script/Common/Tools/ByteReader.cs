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
        /// 通用字节读取工具类
        /// 支持从 byte[] 或 Stream 读取各种类型的数据
        /// </summary>
        public class ByteReader : IReference, IDisposable, IAsyncDisposable
        {
            private Stream _stream;
            private bool _ownsStream;
            private bool _disposed;

            public ByteReader(byte[] data)
            {
                if (data == null)
                    throw new ArgumentNullException(nameof(data));
                
                _stream = new MemoryStream(data);
                _ownsStream = true;
            }

            public ByteReader(Stream stream)
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
            public bool IsEnd => _stream.Position >= _stream.Length;

            public void Seek(long position)
            {
                _stream.Seek(position, SeekOrigin.Begin);
            }

            public void Skip(int count)
            {
                _stream.Seek(count, SeekOrigin.Current);
            }

            public byte ReadByte()
            {
                int b = _stream.ReadByte();
                if (b < 0)
                    throw new EndOfStreamException();
                
                return (byte)b;
            }

            public byte[] ReadBytes(int count)
            {
                byte[] buffer = new byte[count];
                int read = _stream.Read(buffer, 0, count);
                if (read < count)
                    throw new EndOfStreamException();
                
                return buffer;
            }

            public bool ReadBoolean()
            {
                return ReadByte() != 0;
            }

            public short ReadInt16()
            {
                byte[] buffer = ReadBytes(2);
                return (short)(buffer[0] | (buffer[1] << 8));
            }

            public ushort ReadUInt16()
            {
                byte[] buffer = ReadBytes(2);
                return (ushort)(buffer[0] | (buffer[1] << 8));
            }

            public int ReadInt32()
            {
                byte[] buffer = ReadBytes(4);
                return buffer[0] | (buffer[1] << 8) | (buffer[2] << 16) | (buffer[3] << 24);
            }

            public uint ReadUInt32()
            {
                byte[] buffer = ReadBytes(4);
                return (uint)(buffer[0] | (buffer[1] << 8) | (buffer[2] << 16) | (buffer[3] << 24));
            }

            public long ReadInt64()
            {
                byte[] buffer = ReadBytes(8);
                uint lo = (uint)(buffer[0] | (buffer[1] << 8) | (buffer[2] << 16) | (buffer[3] << 24));
                uint hi = (uint)(buffer[4] | (buffer[5] << 8) | (buffer[6] << 16) | (buffer[7] << 24));
                return (long)((ulong)hi << 32 | lo);
            }

            public ulong ReadUInt64()
            {
                byte[] buffer = ReadBytes(8);
                uint lo = (uint)(buffer[0] | (buffer[1] << 8) | (buffer[2] << 16) | (buffer[3] << 24));
                uint hi = (uint)(buffer[4] | (buffer[5] << 8) | (buffer[6] << 16) | (buffer[7] << 24));
                return (ulong)hi << 32 | lo;
            }

            public float ReadSingle()
            {
                byte[] buffer = ReadBytes(4);
                return BitConverter.ToSingle(buffer, 0);
            }

            public double ReadDouble()
            {
                byte[] buffer = ReadBytes(8);
                return BitConverter.ToDouble(buffer, 0);
            }

            public string ReadString()
            {
                int length = ReadInt32();
                if (length <= 0)
                    return string.Empty;
                
                byte[] buffer = ReadBytes(length);
                return Encoding.UTF8.GetString(buffer);
            }

            public string ReadFixedString(int length)
            {
                byte[] buffer = ReadBytes(length);
                int end = Array.IndexOf(buffer, (byte)0);
                if (end < 0)
                    end = length;
                
                return Encoding.UTF8.GetString(buffer, 0, end);
            }

            public Vector2 ReadVector2()
            {
                float x = ReadSingle();
                float y = ReadSingle();
                return new Vector2(x, y);
            }

            public Vector3 ReadVector3()
            {
                float x = ReadSingle();
                float y = ReadSingle();
                float z = ReadSingle();
                return new Vector3(x, y, z);
            }

            public Vector4 ReadVector4()
            {
                float x = ReadSingle();
                float y = ReadSingle();
                float z = ReadSingle();
                float w = ReadSingle();
                return new Vector4(x, y, z, w);
            }

            public Color ReadColor()
            {
                float r = ReadSingle();
                float g = ReadSingle();
                float b = ReadSingle();
                float a = ReadSingle();
                return new Color(r, g, b, a);
            }

            public Color32 ReadColor32()
            {
                byte r = ReadByte();
                byte g = ReadByte();
                byte b = ReadByte();
                byte a = ReadByte();
                return new Color32(r, g, b, a);
            }

            public Quaternion ReadQuaternion()
            {
                float x = ReadSingle();
                float y = ReadSingle();
                float z = ReadSingle();
                float w = ReadSingle();
                return new Quaternion(x, y, z, w);
            }

            public void Clear()
            {
                if (_disposed)
                    return;
                
                _disposed = true;
                _stream?.Dispose();
                
                _stream = null;
            }

            public void Dispose()
            {
                if (_disposed)
                    return;
                
                _disposed = true;
                if (_ownsStream && _stream != null)
                    _stream.Dispose();
                
                _stream = null;
            }

            public async ValueTask DisposeAsync()
            {
                if (_disposed)
                    return;
                
                _disposed = true;
                if (_ownsStream && _stream != null)
                    await _stream.DisposeAsync();
                
                _stream = null;
            }
        }
    }
}
