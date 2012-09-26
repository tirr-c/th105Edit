using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;

namespace cvn_helper
{
    class TenshiEntry : Stream
    {
        private Stream m_mainstream;
        private string m_entry;
        private long m_offset, m_length;
        private long m_position;

        public TenshiEntry(Stream MainStream, string EntryName, long Offset, long Length)
        {
            m_mainstream = MainStream;
            m_entry = EntryName;
            m_offset = Offset;
            m_length = Length;
            m_position = 0;
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
            throw new NotImplementedException();
        }

        public override long Length
        {
            get { return m_length; }
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
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
    class TenshiEntryCollection : Collection<TenshiEntry>
    {

    }

    class HinanawiTenshi : IDisposable
    {
        private short m_list_count;
        private uint m_list_length;
        private FileStream m_stream;
        private TenshiEntryCollection m_entries;
        public TenshiEntryCollection Entries
        {
            get { return m_entries; }
        }

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
            byte[] buf = new byte[4];
            m_stream.Read(buf, 0, 2);
            m_list_count = (short)(buf[0] + (buf[1] << 8));
            m_stream.Read(buf, 0, 4);
            m_list_length = (uint)(buf[0] + (buf[1] << 8) + (buf[2] << 16) + (buf[3] << 24));

            TenshiRandomGenerator mt = new TenshiRandomGenerator(m_list_length + 6);
            byte[] list_buf = new byte[m_list_length];
            m_stream.Read(list_buf, 0, (int)m_list_length);
            for (uint i = 0; i < m_list_length; i++)
            {
                list_buf[i] ^= (byte)(mt.NextInt() ^ 0xff);
            }

            uint c = 0;
            for (short i = 0; i < m_list_count; i++)
            {
                uint off, len;
                off = (uint)(buf[c] + (buf[c + 1] << 8) + (buf[c + 2] << 16) + (buf[c + 3] << 24));
                c += 4;
                len = (uint)(buf[c] + (buf[c + 1] << 8) + (buf[c + 2] << 16) + (buf[c + 3] << 24));
                c += 4;
                byte entry_len = buf[c];
                c++;
                byte[] entry_buf = new byte[entry_len];
                Array.Copy(buf, c, entry_buf, 0, entry_len);
                m_entries.Add(new TenshiEntry(m_stream, Encoding.ASCII.GetString(entry_buf), (long)off, (long)len));
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
                    (uint)((1812433253 * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti) & 0xFFFFFFFF);
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
