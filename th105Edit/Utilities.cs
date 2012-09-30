/*
Copyright VBChunguk  2012

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
*/

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
