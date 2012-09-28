using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;

namespace cvn_helper
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
        private long m_offset, m_length;
        private long m_position;
        private cvnType m_type;
        private byte m_key_base
        {
            get { return (byte)(((m_offset >> 1) | 0x23) & 0xff);}
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
            set { m_changedstream = value; m_position = 0; }
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
                    buffer[offset + i] ^= m_key_base;
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
        private short m_list_count;
        private uint m_list_length;
        private FileStream m_stream;
        private TenshiEntryCollection m_entries;
        public TenshiEntryCollection Entries
        {
            get { return m_entries; }
        }

        private const byte key_base = 0xc5;
        private const byte key_delta = 0x83;
        private const byte key_ddelta = 0x53;

        public HinanawiTenshi()
        {
            m_list_count = 0;
            m_list_length = 0;
            m_stream = null;
            m_entries = new TenshiEntryCollection();
        }

        public HinanawiTenshi(string Path)
            : this()
        {
            Open(Path);
        }

        public void Open(string Path)
        {
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
            Encoding ShiftJIS = Encoding.GetEncoding("Shift-JIS");
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

        public void Dispose()
        {
            m_entries.Clear();
            if (m_stream != null)
            {
                m_stream.Close();
                m_stream.Dispose();
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
