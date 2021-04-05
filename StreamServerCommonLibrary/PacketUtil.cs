using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using CommonLibrary.Exception;

namespace CommonLibrary
{
    public static class PacketUtil
    {
        /// <summary>
        /// 2進数文字列に変換
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static string toBinary(int n)
        {
            string r = "";
            while (n != 0)
            {
                r += (n % 2 == 0 ? "0" : "1");
                n /= 2;
            }

            string reverse = "";
            for (int i = r.Length - 1; i >= 0; --i) reverse += r[i];
            return reverse;
        }

        private static int toInt(string str)
        {
            var chars = str.ToArray();
            int index = 1;
            while (str[index] == 0 && index < chars.Length) index++;
            char[] newStr = new char[chars.Length - index];

            for (int i = index; i < chars.Length; i++) newStr[i - index] = chars[i];
            return Convert.ToInt32(new string(newStr), 2);
        }


        /// <summary>
        /// float値を1byteに変換する
        /// Range : 127 ～ -127
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ConvertByte(int value)
        {
            if (Math.Abs(value) > 127)
            {
                throw new MinimumAvatarPacketCreativeException(
                    "the value must be less than or equal to 127 in absolute value.");
            }

            var h = value >= 0 ? 0 : 1; //符号 
            var s = toBinary(value);
            var str = h.ToString() + s.PadLeft(7, '0');
            byte buf = Convert.ToByte(str, 2);
            return buf;
        }

        /// <summary>
        /// byteをfloatに変換する
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static int ConvertInt(byte buf)
        {
            var body = Convert.ToString(buf, 2).PadLeft(8, '0');
            var c = int.Parse(body.First().ToString());
            var c1 = toInt(body);
            var value = c1 * (c == 1 ? -1 : 1);
            return value;
        }
    }
}