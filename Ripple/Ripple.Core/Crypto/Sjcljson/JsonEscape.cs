using System;
using System.Text;

namespace Ripple.Core.Crypto.Sjcljson
{
    public class JsEscape
    {
        public static string Unescape(string escaped)
        {
            int length = escaped.Length;
            int i = 0;
            var sb = new StringBuilder(escaped.Length / 2);

            while (i < length)
            {
                char n = escaped[i++];

                if (n != '%')
                {
                    sb.Append(n);
                }
                else
                {
                    n = escaped[i++];
                    int code;

                    if (n == 'u')
                    {
                        int substringLength = (i + 4) - i;
                        string slice = escaped.Substring(i, substringLength);
                        code = Convert.ToInt32(slice, 16);
                        i += 4;
                    }
                    else
                    {
                        int begin = i - 1;
                        int substringLength = (i + 1) - begin;
                        string slice = escaped.Substring(begin, substringLength);
                        i++;
                        code = Convert.ToInt32(slice, 16);
                    }
                    sb.Append((char)code);
                }
            }

            return sb.ToString();
        }

        public static string Escape(string raw)
        {
            int length = raw.Length;
            int i = 0;
            var sb = new StringBuilder(raw.Length / 2);

            while (i < length)
            {
                char c = raw[i++];

                if (IsLetterOrDigit(c) || IsEscapeExempt(c))
                {
                    sb.Append(c);
                }
                else
                {
                    int i1 = CodePointAt(raw.ToCharArray(), i - 1);
                    string escape = i1.ToString("x");

                    sb.Append('%');

                    if (escape.Length > 2)
                    {
                        sb.Append('u');
                    }

                    sb.Append(escape.ToUpper());
                }
            }

            return sb.ToString();
        }

        private static int CodePointAt(char[] value, int index)
        {
            if ((index < 0) || (index >= value.Length))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return CodePointAtImpl(value, index, value.Length);
        }

        private static int CodePointAtImpl(char[] a, int index, int limit)
        {
            char c1 = a[index++];
            if (IsHighSurrogate(c1))
            {
                if (index < limit)
                {
                    char c2 = a[index];
                    if (IsLowSurrogate(c2))
                    {
                        return ToCodePoint(c1, c2);
                    }
                }
            }
            return c1;
        }

        /**
            * The minimum value of a
            * <a href="http://www.unicode.org/glossary/#high_surrogate_code_unit">
            * Unicode high-surrogate code unit</a>
            * in the UTF-16 encoding, constant {@code '\u005CuD800'}.
            * A high-surrogate is also known as a <i>leading-surrogate</i>.
            *
            * @since 1.5
            */
        private const char MinHighSurrogate = '\uD800';

        /**
            * The maximum value of a
            * <a href="http://www.unicode.org/glossary/#high_surrogate_code_unit">
            * Unicode high-surrogate code unit</a>
            * in the UTF-16 encoding, constant {@code '\u005CuDBFF'}.
            * A high-surrogate is also known as a <i>leading-surrogate</i>.
            *
            * @since 1.5
            */
        private const char MaxHighSurrogate = '\uDBFF';

        /**
         * The minimum value of a
         * <a href="http://www.unicode.org/glossary/#low_surrogate_code_unit">
         * Unicode low-surrogate code unit</a>
         * in the UTF-16 encoding, constant {@code '\u005CuDC00'}.
         * A low-surrogate is also known as a <i>trailing-surrogate</i>.
         *
         * @since 1.5
         */
        private const char MinLowSurrogate = '\uDC00';

        /**
         * The maximum value of a
         * <a href="http://www.unicode.org/glossary/#low_surrogate_code_unit">
         * Unicode low-surrogate code unit</a>
         * in the UTF-16 encoding, constant {@code '\u005CuDFFF'}.
         * A low-surrogate is also known as a <i>trailing-surrogate</i>.
         *
         * @since 1.5
         */
        private const char MaxLowSurrogate = '\uDFFF';

        /**
         * The minimum value of a
         * <a href="http://www.unicode.org/glossary/#supplementary_code_point">
         * Unicode supplementary code point</a>, constant {@code U+10000}.
         *
         * @since 1.5
         */
        private const int MinSupplementaryCodePoint = 0x010000;

        private static int ToCodePoint(char high, char low)
        {
            // Optimized form of:
            // return ((high - MIN_HIGH_SURROGATE) << 10)
            //         + (low - MIN_LOW_SURROGATE)
            //         + MIN_SUPPLEMENTARY_CODE_POINT;
            return ((high << 10) + low) + (MinSupplementaryCodePoint
                                           - (MinHighSurrogate << 10)
                                           - MinLowSurrogate);
        }

        private static bool IsLowSurrogate(char ch)
        {
            return ch >= MinLowSurrogate && ch < (MaxLowSurrogate + 1);
        }

        private static bool IsHighSurrogate(char ch)
        {
            // Help VM constant-fold; MAX_HIGH_SURROGATE + 1 == MIN_LOW_SURROGATE
            return ch >= MinHighSurrogate && ch < (MaxHighSurrogate + 1);
        }

        private static bool IsLetterOrDigit(char ch)
        {
            return (ch >= 'a' && ch <= 'z') ||
               (ch >= 'A' && ch <= 'Z') ||
               (ch >= '0' && ch <= '9');
        }

        private static bool IsEscapeExempt(char c)
        {
            switch (c)
            {
                case '*':
                case '@':
                case '-':
                case '_':
                case '+':
                case '.':
                case '/':
                    return true;
                default:
                    return false;
            }
        }
    }
}
