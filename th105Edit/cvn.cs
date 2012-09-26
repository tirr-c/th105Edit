using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Media;
using System.Drawing;
using System.IO;

namespace cvn_helper
{
    public enum cvnType
    {
        Text,
        CSV,
        Graphic,
        Audio,
        Auto,
    }

    public class cv1DataLine
    {
        private string[] fields;
        public string[] Fields
        {
            get { return fields; }
        }

        public cv1DataLine()
        {
            fields = null;
        }
        public cv1DataLine(string Data)
            : this()
        {
            fields = Data.Split(new char[] { ',' });
        }

        public override string ToString()
        {
            if (fields == null) return string.Empty;
            return string.Join(",", fields);
        }
    }
    public class cv1DataCollection : Collection<cv1DataLine>
    {
        public override string ToString()
        {
            string result = string.Empty;
            foreach (cv1DataLine i in this)
            {
                result += i.ToString() + "\r\n";
            }
            return result;
        }
    }

    public abstract class cvnBase : IDisposable
    {
        protected cvnType m_type;
        public cvnType Type
        {
            get { return m_type; }
        }

        protected string m_path;
        public string Path
        {
            get { return m_path; }
        }

        public abstract object Data
        {
            get;
        }

        protected cvnBase()
        {
        }

        public abstract void Open(string Path);
        public abstract void SaveToFile(string Path);

        public abstract void Dispose();

        public override string ToString()
        {
            return "cvn_helper.cvnBase";
        }
    }
    public class cv0 : cvnBase
    {
        public cv0()
            : base()
        {
            m_type = cvnType.Text;
            m_path = string.Empty;
            m_encoding = null;
            m_buf = null;
        }
        public cv0(string Path)
            : base()
        {
            Open(Path);
        }

        private byte[] m_buf;
        private Encoding m_encoding;
        public virtual Encoding StringEncoding
        {
            get { return m_encoding; }
            set { m_encoding = value; }
        }

        public override void Open(string Path)
        {
            Open(Path, Encoding.GetEncoding("Shift-JIS"));
        }
        public virtual void Open(string Path, Encoding StringEncoding)
        {
            FileStream fp = new FileStream(Path, FileMode.Open);
            long len = fp.Length;
            m_buf = new byte[len];
            fp.Read(m_buf, 0, (int)len);
            fp.Close();

            byte key, delta;
            const byte d_delta = 0x6b;
            key = 0x8b;
            delta = 0x71;
            for (long i = 0; i < len; i++)
            {
                m_buf[i] ^= key;
                key += delta;
                delta -= d_delta;
            }
            m_encoding = StringEncoding;
        }
        public override void SaveToFile(string Path)
        {
            int len = m_buf.Length;
            byte[] m_newbuf = new byte[m_buf.Length];
            m_buf.CopyTo(m_newbuf, 0);
            byte key, delta;
            const byte d_delta = 0x6b;
            key = 0x8b;
            delta = 0x71;
            for (int i = 0; i < len; i++)
            {
                m_newbuf[i] ^= key;
                key += delta;
                delta -= d_delta;
            }
            File.WriteAllBytes(Path, m_newbuf);
        }

        public override object Data
        {
            get
            {
                if (m_encoding == null || m_buf == null) return string.Empty;
                return m_encoding.GetString(m_buf);
            }
        }
        public void SetData(string Data)
        {
            m_buf = m_encoding.GetBytes(Data);
        }

        public override void Dispose()
        {
            m_buf = null;
            m_encoding = null;
        }

        public override string ToString()
        {
            return Data as string;
        }
    }
    public class cv1 : cv0
    {
        public cv1()
            : base()
        {
            m_type = cvnType.CSV;
            m_records = new cv1DataCollection();
        }
        public cv1(string Path)
            : this()
        {
            Open(Path);
        }

        public string RawData
        {
            get { return base.Data as string; }
        }
        public override Encoding StringEncoding
        {
            get { return base.StringEncoding; }
            set
            {
                base.StringEncoding = value;
                ReloadRecords();
            }
        }
        private cv1DataCollection m_records;

        public override void Open(string Path)
        {
            Open(Path, Encoding.GetEncoding("Shift-JIS"));
        }
        public override void Open(string Path, Encoding StringEncoding)
        {
            base.Open(Path, StringEncoding);
            ReloadRecords();
        }
        public override void SaveToFile(string Path)
        {
            UpdateRawData();
            base.SaveToFile(Path);
        }

        private void ReloadRecords()
        {
            m_records.Clear();
            string[] raw_records = RawData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string i in raw_records)
            {
                if (i[0] == '#') continue;
                m_records.Add(new cv1DataLine(i));
            }
        }
        public void UpdateRawData()
        {
            base.SetData(m_records.ToString());
        }

        public override object Data
        {
            get
            {
                return m_records;
            }
        }

        public override void Dispose()
        {
            m_records.Clear();
            base.Dispose();
        }

        public override string ToString()
        {
            return m_records.ToString();
        }
    }
    
    class cvn
    {
        public static cvnBase Open(string Path, cvnType FileType = cvnType.Auto)
        {
            cvnType file_type = FileType;
            if (file_type == cvnType.Auto)
            {
                string extension = Path.Substring(Path.IndexOf('.') + 1);
                switch (extension)
                {
                    case "cv0": file_type = cvnType.Text; break;
                    case "cv1": file_type = cvnType.CSV; break;
                    case "cv2": file_type = cvnType.Graphic; break;
                    case "cv3": file_type = cvnType.Audio; break;
                    default: throw new FormatException("지원하지 않는 확장자입니다.");
                }
            }
            switch (file_type)
            {
                case cvnType.Text: return new cv0(Path);
                case cvnType.CSV: return new cv1(Path);
                default: return null;
            }
        }
    }
}
