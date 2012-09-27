using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Media;
using System.Drawing;
using System.Drawing.Imaging;
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
            string result = string.Empty;
            bool first = true;
            foreach (string i in fields)
            {
                if (!first) result += ",";
                first = false;
                if (i.IndexOf(',') != -1)
                    result += "\"" + i + "\"";
                else
                    result += i;
            }
            return result;
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
        public abstract void SetData(object Data);

        protected cvnBase()
        {
        }

        public abstract void Open(string Path);
        public abstract void Open(Stream fp);
        public abstract void SaveToFile(string Path);
        public abstract Stream ToStream();
        public abstract void Extract(string Path);

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
            : this()
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
            Open(fp, StringEncoding);
        }
        public override void Open(Stream fp)
        {
            Open(fp, Encoding.GetEncoding("Shift-JIS"));
        }
        public virtual void Open(Stream fp, Encoding StringEncoding)
        {
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
        public override Stream ToStream()
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
            return new MemoryStream(m_newbuf);
        }
        public override void Extract(string Path)
        {
            File.WriteAllBytes(Path, m_buf);
        }

        public override object Data
        {
            get
            {
                if (m_encoding == null || m_buf == null) return string.Empty;
                return m_encoding.GetString(m_buf);
            }
        }
        public override void SetData(object Data)
        {
            m_buf = m_encoding.GetBytes(Data as string);
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
        public override void SetData(object Data)
        {
            m_records = Data as cv1DataCollection;
            ReloadRecords();
            UpdateRawData();
        }

        public override void Open(string Path)
        {
            Open(Path, Encoding.GetEncoding("Shift-JIS"));
        }
        public override void Open(string Path, Encoding StringEncoding)
        {
            base.Open(Path, StringEncoding);
            ReloadRecords();
        }
        public override void Open(Stream fp)
        {
            Open(fp, Encoding.GetEncoding("Shift-JIS"));
        }
        public override void Open(Stream fp, Encoding StringEncoding)
        {
            base.Open(fp, StringEncoding);
            ReloadRecords();
        }

        public override void SaveToFile(string Path)
        {
            UpdateRawData();
            base.SaveToFile(Path);
        }
        public override Stream ToStream()
        {
            UpdateRawData();
            return base.ToStream();
        }
        public override void Extract(string Path)
        {
            UpdateRawData();
            base.Extract(Path);
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
    public class cv2 : cvnBase
    {
        private enum enum_graphic_format
        {
            WithPalette,
            General,
            Unknown,
        }

        private enum_graphic_format m_format;
        private byte m_raw_format;
        private int m_width_actual, m_height, m_width_data;
        private int m_unknown_field;
        private int m_length
        {
            get
            {
                int result = 0;
                switch (m_format)
                {
                    case enum_graphic_format.WithPalette:
                        result = m_width_actual * m_height;
                        break;
                    case enum_graphic_format.General:
                        result = m_width_actual * m_height * 4;
                        break;
                }
                return result;
            }
        }
        private int m_width_in_bytes
        {
            get
            {
                int result = 0;
                switch (m_format)
                {
                    case enum_graphic_format.WithPalette:
                        result = m_width_data;
                        break;
                    case enum_graphic_format.General:
                        result = m_width_data * 4;
                        break;
                }
                return result;
            }
        }
        private int m_actual_width_in_bytes
        {
            get
            {
                int result = 0;
                switch (m_format)
                {
                    case enum_graphic_format.WithPalette:
                        result = m_width_actual;
                        break;
                    case enum_graphic_format.General:
                        result = m_width_actual * 4;
                        break;
                }
                return result;
            }
        }

        private Bitmap m_graphic;
        private bool m_generated_graphic;

        public cv2()
            : base()
        {
            m_type = cvnType.Graphic;
            m_format = enum_graphic_format.Unknown;
            m_width_actual = m_height = m_width_data = 0;
            m_graphic = null;
        }
        public cv2(string Path)
            : this()
        {
            Open(Path);
        }

        public override object Data
        {
            get { return m_graphic; }
        }
        public override void SetData(object Data)
        {
            Bitmap _Data = Data as Bitmap;
            if (_Data.PixelFormat != PixelFormat.Format32bppArgb && _Data.PixelFormat != PixelFormat.Format8bppIndexed)
                throw new FormatException("지원하지 않는 포맷의 이미지입니다.");
            enum_graphic_format NewFormat = enum_graphic_format.Unknown;
            switch (_Data.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed: NewFormat = enum_graphic_format.WithPalette; break;
                case PixelFormat.Format32bppArgb: NewFormat = enum_graphic_format.General; break;
            }
            if (m_format != NewFormat)
                throw new FormatException("포맷이 일치하지 않습니다.");
            if (m_width_actual != _Data.Width || m_height != _Data.Height)
                throw new FormatException("이미지 크기가 일치하지 않습니다.");
            m_graphic = _Data;
            m_generated_graphic = false;
        }
        
        public override void Open(string Path)
        {
            Open(Path, "");
        }
        public void Open(string Path, string PalettePath)
        {
            FileStream fp = new FileStream(Path, FileMode.Open);
            FileStream pp = null;
            if (PalettePath != "") pp = new FileStream(PalettePath, FileMode.Open);
            Open(fp, pp);
        }
        public override void Open(Stream fp)
        {
            Open(fp, null);
        }
        public void Open(Stream fp, Stream PalettePath)
        {
            byte[] header = new byte[0x11];
            fp.Read(header, 0, 0x11);
            m_raw_format = header[0];
            switch (m_raw_format)
            {
                case 0x08:
                    m_format = enum_graphic_format.WithPalette;
                    break;
                case 0x18:
                case 0x20:
                    m_format = enum_graphic_format.General;
                    break;
                default:
                    m_format = enum_graphic_format.Unknown;
                    break;
            }
            byte[] buf = new byte[4];
            Array.Copy(header, 1, buf, 0, 4);
            m_width_actual = endian(buf);
            Array.Copy(header, 5, buf, 0, 4);
            m_height = endian(buf);
            Array.Copy(header, 9, buf, 0, 4);
            m_width_data = endian(buf);
            Array.Copy(header, 13, buf, 0, 4);
            m_unknown_field = endian(buf);

            byte[] m_raw = new byte[m_length];
            fp.Read(m_raw, 0, m_length);
            m_graphic = new Bitmap(m_width_actual, m_height);
            m_generated_graphic = true;
            BitmapData raw_data;
            switch (m_format)
            {
                case enum_graphic_format.General:
                    raw_data = m_graphic.LockBits(
                        new Rectangle(0, 0, m_width_actual, m_height),
                        ImageLockMode.WriteOnly,
                        PixelFormat.Format32bppArgb);
                    break;
                case enum_graphic_format.WithPalette:
                    raw_data = m_graphic.LockBits(
                        new Rectangle(0, 0, m_width_actual, m_height),
                        ImageLockMode.WriteOnly,
                        PixelFormat.Format8bppIndexed);
                    break;
                default:
                    throw new FormatException("지원되지 않는 포맷의 cv2입니다.");
            }
            int bitmap_len = raw_data.Stride * raw_data.Height;
            byte[] bitmap_buffer = new byte[bitmap_len];
            System.Runtime.InteropServices.Marshal.Copy(raw_data.Scan0, bitmap_buffer, 0, bitmap_len);
            for (int i = 0; i < m_height; i++)
            {
                int startAt_buf = i * m_width_in_bytes;
                int startAt_image = i * raw_data.Stride;
                for (int j = 0; j < m_width_in_bytes; j++)
                    bitmap_buffer[startAt_image + j] = m_raw[startAt_buf + j];
            }
            System.Runtime.InteropServices.Marshal.Copy(bitmap_buffer, 0, raw_data.Scan0, bitmap_len);
            m_graphic.UnlockBits(raw_data);
            if (m_format == enum_graphic_format.WithPalette)
            {
                if (PalettePath == null)
                {
                    m_graphic.Dispose();
                    throw new ArgumentException("팔레트 파일이 지정되지 않았습니다.", "PalettePath");
                }
                m_graphic.Dispose();
                throw new NotImplementedException("Indexed 형식은 아직 지원하지 않습니다.");
            }
        }
        
        public override void SaveToFile(string Path)
        {
            FileStream fp = new FileStream(Path, FileMode.Create);
            byte[] header = new byte[0x11];
            header[0] = m_raw_format;
            Array.Copy(deendian(m_width_actual), 0, header, 1, 4);
            Array.Copy(deendian(m_height), 0, header, 5, 4);
            Array.Copy(deendian(m_width_data), 0, header, 9, 4);
            Array.Copy(deendian(m_unknown_field), 0, header, 13, 4);
            fp.Write(header, 0, 0x11);

            BitmapData raw_data;
            switch (m_format)
            {
                case enum_graphic_format.General:
                    raw_data = m_graphic.LockBits(
                        new Rectangle(0, 0, m_width_actual, m_height),
                        ImageLockMode.WriteOnly,
                        PixelFormat.Format32bppArgb);
                    break;
                case enum_graphic_format.WithPalette:
                    raw_data = m_graphic.LockBits(
                        new Rectangle(0, 0, m_width_actual, m_height),
                        ImageLockMode.WriteOnly,
                        PixelFormat.Format8bppIndexed);
                    break;
                default:
                    throw new FormatException("지원되지 않는 포맷의 cv2입니다.");
            }
            int bitmap_len = raw_data.Stride * raw_data.Height;
            byte[] bitmap_buffer = new byte[bitmap_len];
            System.Runtime.InteropServices.Marshal.Copy(raw_data.Scan0, bitmap_buffer, 0, bitmap_len);
            for (int i = 0; i < m_height; i++)
            {
                int startAt_image = i * raw_data.Stride;
                fp.Write(bitmap_buffer, startAt_image, m_actual_width_in_bytes);
                for (int j = m_actual_width_in_bytes; j < m_width_in_bytes; j++)
                    fp.WriteByte(0);
            }
            m_graphic.UnlockBits(raw_data);
            fp.Close();
        }
        public override Stream ToStream()
        {
            MemoryStream fp = new MemoryStream();
            byte[] header = new byte[0x11];
            header[0] = m_raw_format;
            Array.Copy(deendian(m_width_actual), 0, header, 1, 4);
            Array.Copy(deendian(m_height), 0, header, 5, 4);
            Array.Copy(deendian(m_width_data), 0, header, 9, 4);
            Array.Copy(deendian(m_unknown_field), 0, header, 13, 4);
            fp.Write(header, 0, 0x11);

            BitmapData raw_data;
            switch (m_format)
            {
                case enum_graphic_format.General:
                    raw_data = m_graphic.LockBits(
                        new Rectangle(0, 0, m_width_actual, m_height),
                        ImageLockMode.WriteOnly,
                        PixelFormat.Format32bppArgb);
                    break;
                case enum_graphic_format.WithPalette:
                    raw_data = m_graphic.LockBits(
                        new Rectangle(0, 0, m_width_actual, m_height),
                        ImageLockMode.WriteOnly,
                        PixelFormat.Format8bppIndexed);
                    break;
                default:
                    throw new FormatException("지원되지 않는 포맷의 cv2입니다.");
            }
            int bitmap_len = raw_data.Stride * raw_data.Height;
            byte[] bitmap_buffer = new byte[bitmap_len];
            System.Runtime.InteropServices.Marshal.Copy(raw_data.Scan0, bitmap_buffer, 0, bitmap_len);
            for (int i = 0; i < m_height; i++)
            {
                int startAt_image = i * raw_data.Stride;
                fp.Write(bitmap_buffer, startAt_image, m_actual_width_in_bytes);
                for (int j = m_actual_width_in_bytes; j < m_width_in_bytes; j++)
                    fp.WriteByte(0);
            }
            m_graphic.UnlockBits(raw_data);
            return new MemoryStream(fp.GetBuffer());
        }
        public override void Extract(string Path)
        {
            m_graphic.Save(Path);
        }

        public override void Dispose()
        {
            if (m_generated_graphic) m_graphic.Dispose();
        }

        public override string ToString()
        {
            return "cvn_helper.cv2";
        }

        private static int endian(byte[] bytes)
        {
            return bytes[0] + (bytes[1] << 8) + (bytes[2] << 16) + (bytes[3] << 24);
        }
        private static byte[] deendian(int target)
        {
            byte[] result = new byte[4];
            result[0] = (byte)(target >> 0  & 0xFF);
            result[1] = (byte)(target >> 8  & 0xFF);
            result[2] = (byte)(target >> 16 & 0xFF);
            result[3] = (byte)(target >> 24 & 0xFF);
            return result;
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
                case cvnType.Graphic:
                    cv2 result = new cv2();
                    try
                    {
                        result.Open(Path);
                    }
                    catch (System.ArgumentException)
                    {
                        result.Open(Path, "palette000.pal");
                    }
                    return result;
                default: return null;
            }
        }
    }
}
