using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Threading;

namespace th105Edit
{
    public class TenshiEntry : Stream
    {
        private Stream m_mainstream;
        private Stream m_changedstream;
        private Stream m_stream
        {
            get
            {
                if (m_changedstream == null) return m_mainstream;
                else return m_changedstream;
            }
        }
        private string m_entry;
        private long m_offset, m_length, m_keyoffset;
        private long m_position;
        private cvnType m_type;
        private byte KeyBase
        {
            get
            {
                if(m_keyoffset == 0) return (byte)(((m_offset >> 1) | 0x23) & 0xff);
                else return (byte)(((m_keyoffset >> 1) | 0x23) & 0xff);
            }
        }
        private byte OriginalKeyBase
        {
            get
            {
                return (byte)(((m_offset >> 1) | 0x23) & 0xff);
            }
        }

        public string Entry
        {
            get { return m_entry; }
        }
        public string[] EntryPath
        {
            get { return m_entry.Split('/', '\\'); }
        }
        public cvnType Type
        {
            get { return m_type; }
        }
        public Stream ChangedStream
        {
            get { return m_changedstream; }
            set
            {
                if (m_changedstream != null)
                {
                    m_changedstream.Close(); m_changedstream.Dispose();
                } m_changedstream = value; m_position = 0;
            }
        }
        public long NewOffset
        {
            set { m_keyoffset = value; }
        }

        public int EntryLength
        {
            get { return 9 + Encoding.GetEncoding("Shift-JIS").GetByteCount(m_entry); }
        }

        public TenshiEntry(Stream MainStream, string EntryName, long Offset, long Length)
        {
            m_mainstream = MainStream;
            m_entry = EntryName;
            m_offset = Offset;
            m_length = Length;
            m_changedstream = null;
            m_position = 0;

            m_type = cvnType.Unknown;
            string extension = m_entry.Substring(m_entry.IndexOf('.') + 1);
            switch (extension)
            {
                case "cv0": m_type = cvnType.Text; break;
                case "cv1": m_type = cvnType.CSV; break;
                case "cv2": m_type = cvnType.Graphic; break;
                case "cv3": m_type = cvnType.Audio; break;
            }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            // do nothing
        }

        public override long Length
        {
            get
            {
                if (m_changedstream == null) return m_length;
                else return m_changedstream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return m_position;
            }
            set
            {
                m_position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int actual_read;
            if (m_changedstream == null)
            {
                m_mainstream.Seek(m_offset + m_position, SeekOrigin.Begin);
                actual_read = (int)(m_length - m_position);
                if (actual_read > count) actual_read = count;

                actual_read = m_mainstream.Read(buffer, offset, actual_read);
                for (int i = 0; i < actual_read; i++)
                    buffer[offset + i] ^= KeyBase;
            }
            else
            {
                m_changedstream.Seek(m_position, SeekOrigin.Begin);
                actual_read = (int)(m_length - m_position);
                if (actual_read > count) actual_read = count;

                actual_read = m_changedstream.Read(buffer, offset, actual_read);
            }
            m_position += actual_read;
            return actual_read;
        }
        public int ReadEncrpyted(byte[] buffer, int offset, int count)
        {
            int actual_read;
            byte key_base;
            if (m_changedstream == null)
            {
                m_mainstream.Seek(m_offset + m_position, SeekOrigin.Begin);
                actual_read = (int)(m_length - m_position);
                if (actual_read > count) actual_read = count;

                actual_read = m_mainstream.Read(buffer, offset, actual_read);
                key_base = (byte)(OriginalKeyBase ^ KeyBase);
            }
            else
            {
                m_changedstream.Seek(m_position, SeekOrigin.Begin);
                actual_read = (int)(m_length - m_position);
                if (actual_read > count) actual_read = count;

                actual_read = m_changedstream.Read(buffer, offset, actual_read);
                key_base = KeyBase;
            }
            if (key_base != 0)
            {
                for (int i = 0; i < actual_read; i++)
                    buffer[offset + i] ^= key_base;
            }
            m_position += actual_read;
            return actual_read;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    m_position = offset;
                    break;
                case SeekOrigin.Current:
                    m_position += offset;
                    break;
                case SeekOrigin.End:
                    m_position = m_length + offset;
                    break;
            }
            if (m_position < 0) m_position = 0;
            if (m_position > m_length) m_position = m_length;
            return m_position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override string ToString()
        {
            return m_entry;
        }
    }
    public class TenshiEntryCollection : Collection<TenshiEntry>
    {

    }

    public class HinanawiTenshi : IDisposable
    {
        public delegate void SaveFileCallback(ushort count, ushort total);

        private short m_list_count;
        private uint m_list_length;
        private FileStream m_stream;
        private TenshiEntryCollection m_entries;
        public TenshiEntryCollection Entries
        {
            get { return m_entries; }
        }
        private Encoding ShiftJIS;

        private const byte key_base = 0xc5;
        private const byte key_delta = 0x83;
        private const byte key_ddelta = 0x53;

        public HinanawiTenshi()
        {
            m_list_count = 0;
            m_list_length = 0;
            m_stream = null;
            m_entries = new TenshiEntryCollection();
            ShiftJIS = Encoding.GetEncoding("Shift-JIS");
        }

        public HinanawiTenshi(string Path)
            : this()
        {
            Open(Path);
        }

        public void Open(string Path)
        {
            if (m_stream != null) m_stream.Close();
            m_stream = new FileStream(Path, FileMode.Open);
            m_entries.Clear();
            byte[] buf = new byte[4];
            m_stream.Read(buf, 0, 2);
            m_list_count = (short)(buf[0] + (buf[1] << 8));
            m_stream.Read(buf, 0, 4);
            m_list_length = (uint)(buf[0] + (buf[1] << 8) + (buf[2] << 16) + (buf[3] << 24));

            TenshiRandomGenerator mt = new TenshiRandomGenerator(m_list_length + 6);
            byte[] list_buf = new byte[m_list_length];
            m_stream.Read(list_buf, 0, (int)m_list_length);
            byte key = key_base, delta = key_delta;
            for (uint i = 0; i < m_list_length; i++)
            {
                list_buf[i] ^= (byte)(mt.NextInt() & 0xff);
                list_buf[i] ^= key;
                key += delta;
                delta += key_ddelta;
            }

            uint c = 0;
            for (short i = 0; i < m_list_count; i++)
            {
                uint off, len;
                off = (uint)(list_buf[c] + (list_buf[c + 1] << 8) + (list_buf[c + 2] << 16) + (list_buf[c + 3] << 24));
                c += 4;
                len = (uint)(list_buf[c] + (list_buf[c + 1] << 8) + (list_buf[c + 2] << 16) + (list_buf[c + 3] << 24));
                c += 4;
                byte entry_len = list_buf[c];
                c++;
                byte[] entry_buf = new byte[entry_len];
                Array.Copy(list_buf, c, entry_buf, 0, entry_len);
                c += entry_len;
                string entry_name = ShiftJIS.GetString(entry_buf);
                m_entries.Add(new TenshiEntry(m_stream, entry_name, (long)off, (long)len));
            }
        }
        public void Save(string Path, SaveFileCallback callback = null)
        {
            FileStream save_stream = new FileStream(Path, FileMode.Create);
            try
            {
                ushort entry_count = (ushort)m_entries.Count;
                uint entry_len = 0;

                // 엔트리 리스트 길이 계산
                foreach (TenshiEntry i in m_entries)
                {
                    entry_len += (uint)i.EntryLength;
                }
                save_stream.Write(LittleEndian.ToEndian(entry_count), 0, 2);
                save_stream.Write(LittleEndian.ToEndian(entry_len), 0, 4);
                // 엔트리 리스트 구성
                uint offset = 6 + entry_len;
                MemoryStream entry_list_stream = new MemoryStream((int)entry_len);
                TenshiRandomGenerator rand = new TenshiRandomGenerator(6 + entry_len);
                foreach (TenshiEntry i in m_entries)
                {
                    entry_list_stream.Write(LittleEndian.ToEndian(offset), 0, 4);
                    entry_list_stream.Write(LittleEndian.ToEndian((uint)i.Length), 0, 4);
                    entry_list_stream.WriteByte((byte)ShiftJIS.GetByteCount(i.Entry));
                    entry_list_stream.Write(ShiftJIS.GetBytes(i.Entry), 0, ShiftJIS.GetByteCount(i.Entry));
                    i.NewOffset = offset;
                    offset += (uint)i.Length;
                }
                // 리스트 암호화
                {
                    byte[] buf = entry_list_stream.GetBuffer();
                    byte key = key_base, delta = key_delta;
                    for (uint i = 0; i < m_list_length; i++)
                    {
                        buf[i] ^= key;
                        key += delta;
                        delta += key_ddelta;
                        buf[i] ^= (byte)(rand.NextInt() & 0xff);
                    }
                    save_stream.Write(buf, 0, (int)entry_len);
                    entry_list_stream.Dispose();
                }
                // 나 쓴다 데이터
                ushort count = 0;
                if (m_buf_queue == null) m_buf_queue = new Queue<byte[]>();
                else m_buf_queue.Clear();
                Thread th = new Thread(new ThreadStart(CryptThread));
                th.Start();
                for (ushort i = 0; i < entry_count; i++)
                {
                    while (m_buf_queue.Count <= 0) ;
                    byte[] buf = m_buf_queue.Dequeue();
                    save_stream.Write(buf, 0, buf.Length);
                    if (callback != null) callback(++count, entry_count);
                }
                save_stream.Close();
                // 다시 열기
                m_stream.Close();
                Open(Path);
            }
            catch
            {
            }
            finally
            {
                save_stream.Close();
            }
        }

        public void Dispose()
        {
            m_entries.Clear();
            if (m_stream != null)
            {
                m_stream.Close();
                m_stream.Dispose();
            }
        }

        private Queue<byte[]> m_buf_queue;
        private void CryptThread()
        {
            foreach (TenshiEntry i in m_entries)
            {
                byte[] buf = new byte[i.Length];
                i.Seek(0, SeekOrigin.Begin);
                i.ReadEncrpyted(buf, 0, (int)i.Length);
                m_buf_queue.Enqueue(buf);
            }
        }
    }

    class TenshiRandomGenerator
    {
        private const int N = 624;
        private const int M = 397;
        private const uint MATRIX_A = 0x9908b0df;
        private const uint UPPER_MASK = 0x80000000;
        private const uint LOWER_MASK = 0x7FFFFFFF;

        private uint[] mt;
        private int mti;

        private static uint[] mag01 = new uint[] { 0, MATRIX_A };

        public TenshiRandomGenerator(uint seed)
        {
            mt = new uint[N];
            Initialize(seed);
        }

        private void Initialize(uint seed)
        {
            mt[0] = seed;
            for (mti = 1; mti < N; ++mti)
            {
                mt[mti] =
                    (uint)((((1812433253 * (mt[mti - 1] ^ ((mt[mti - 1] >> 30) & 0xFFFFFFFF))) & 0xFFFFFFFF) + mti) & 0xFFFFFFFF);
            }
        }

        public uint NextInt()
        {
            uint y;
            if (mti >= N)
            {
                int kk;
                if (mti == N + 1)
                    Initialize(5489);

                for (kk = 0; kk < N - M; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }
                for (; kk < N - 1; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }
                y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
                mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1UL];

                mti = 0;
            }

            y = mt[mti++];

            /* Tempering */
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680;
            y ^= (y << 15) & 0xefc60000;
            y ^= (y >> 18);

            return y;
        }
    }
}
