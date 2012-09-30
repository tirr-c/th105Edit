using System;
using System.Collections.Generic;
using System.Text;

namespace th105Edit
{
    public class LittleEndian
    {
        public static int FromEndian(byte[] bytes)
        {
            return bytes[0] + (bytes[1] << 8) + (bytes[2] << 16) + (bytes[3] << 24);
        }
        public static byte[] ToEndian(int target)
        {
            return ToEndian((uint)target);
        }
        public static byte[] ToEndian(uint target)
        {
            byte[] result = new byte[4];
            result[0] = (byte)(target >> 0 & 0xFF);
            result[1] = (byte)(target >> 8 & 0xFF);
            result[2] = (byte)(target >> 16 & 0xFF);
            result[3] = (byte)(target >> 24 & 0xFF);
            return result;
        }
        public static byte[] ToEndian(short target)
        {
            return ToEndian((ushort)target);
        }
        public static byte[] ToEndian(ushort target)
        {
            byte[] result = new byte[2];
            result[0] = (byte)(target >> 0 & 0xFF);
            result[1] = (byte)(target >> 8 & 0xFF);
            return result;
        }
    }
}
