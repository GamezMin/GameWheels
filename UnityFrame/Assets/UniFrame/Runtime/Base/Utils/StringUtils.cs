#define DEBUG
using System;
using System.Globalization;
using System.Text;
using UnityEngine;

public static class StringUtils
{
    private static StringBuilder _strBuilder = new StringBuilder();

    private static byte[] _bytes = new byte[1024];

    public static StringBuilder StringBuilder => _strBuilder;

    public static StringBuilder NewStringBuilder
    {
        get
        {
            _strBuilder.Length = 0;
            return _strBuilder;
        }
    }

    public static void SplitFilename(string qualifiedName, out string outBasename, out string outPath)
    {
        string text = qualifiedName.Replace('\\', '/');
        int num = text.LastIndexOf('/');
        if (num == -1)
        {
            outPath = string.Empty;
            outBasename = qualifiedName;
        }
        else
        {
            outBasename = text.Substring(num + 1, text.Length - num - 1);
            outPath = text.Substring(0, num + 1);
        }
    }

    public static string StandardisePath(string init)
    {
        string text = init.Replace('\\', '/');
        if (text.Length > 0 && text[text.Length - 1] != '/')
        {
            text += "/";
        }

        return text;
    }

    public static string StandardisePathWithoutSlash(string init)
    {
        string text = init.Replace('\\', '/');
        while (text.Length > 0 && text[text.Length - 1] == '/')
        {
            text = text.Remove(text.Length - 1);
        }

        return text;
    }

    public static void SplitBaseFilename(string fullName, out string outBasename, out string outExtention)
    {
        int num = fullName.LastIndexOf('.');
        if (num == -1)
        {
            outExtention = string.Empty;
            outBasename = fullName;
        }
        else
        {
            outExtention = fullName.Substring(num);
            outBasename = fullName.Substring(0, num);
        }
    }

    public static int CountOf(string str, char what)
    {
        int num = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == what)
            {
                num++;
            }
        }

        return num;
    }

    public static string SafeFormat(string format, params object[] args)
    {
        if (format != null && args != null)
        {
            try
            {
                return string.Format(format, args);
            }
            catch (Exception ex)
            {
                UniLogger.LogException(ex);
            }
        }

        return string.Empty;
    }

    public static void SplitFullFilename(string qualifiedName, out string outBasename, out string outExtention,
        out string outPath)
    {
        string outBasename2 = string.Empty;
        SplitFilename(qualifiedName, out outBasename2, out outPath);
        SplitBaseFilename(outBasename2, out outBasename, out outExtention);
    }

    public static string DecodeFromUtf8(string utf8String)
    {
        if (utf8String == null)
        {
            return string.Empty;
        }

        if (utf8String.Length > 0)
        {
            Debug.Assert(_bytes.Length > utf8String.Length);
            for (int i = 0; i < utf8String.Length; i++)
            {
                _bytes[i] = (byte)utf8String[i];
            }

            return Encoding.UTF8.GetString(_bytes, 0, utf8String.Length);
        }

        return utf8String;
    }

    public static string EncodingEscape(string str, int startIndex = 0, int endIndex = -1)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }

        StringBuilder stringBuilder = new StringBuilder(str.Length + str.Length / 10 + 4);
        if (endIndex < 0)
        {
            endIndex = str.Length;
        }

        for (int i = startIndex; i < endIndex; i++)
        {
            char c = str[i];
            switch (c)
            {
                case '\'':
                    stringBuilder.Append('\\');
                    stringBuilder.Append('\'');
                    break;
                case '"':
                    stringBuilder.Append('\\');
                    stringBuilder.Append('"');
                    break;
                case '\a':
                    stringBuilder.Append('\\');
                    stringBuilder.Append('a');
                    break;
                case '\b':
                    stringBuilder.Append('\\');
                    stringBuilder.Append('b');
                    break;
                case '\f':
                    stringBuilder.Append('\\');
                    stringBuilder.Append('f');
                    break;
                case '\n':
                    stringBuilder.Append('\\');
                    stringBuilder.Append('n');
                    break;
                case '\r':
                    stringBuilder.Append('\\');
                    stringBuilder.Append('r');
                    break;
                case '\t':
                    stringBuilder.Append('\\');
                    stringBuilder.Append('t');
                    break;
                case '\v':
                    stringBuilder.Append('\\');
                    stringBuilder.Append('v');
                    break;
                case '\\':
                    stringBuilder.Append('\\');
                    stringBuilder.Append('\\');
                    break;
                default:
                    stringBuilder.Append(c);
                    break;
            }
        }

        return stringBuilder.ToString();
    }

    public static string CombineString(params string[] strs)
    {
        _strBuilder.Length = 0;
        for (int i = 0; i < strs.Length; i++)
        {
            _strBuilder.Append(strs[i]);
        }

        return _strBuilder.ToString();
    }

    public static string WordToUnicode(string word)
    {
        string text = "";
        if (!string.IsNullOrEmpty(word))
        {
            for (int i = 0; i < word.Length; i++)
            {
                text = text + "\\u" + ((int)word[i]).ToString("x");
            }
        }

        return text;
    }

    public static string UnicodeToWord(string unicode)
    {
        string text = "";
        if (!string.IsNullOrEmpty(unicode))
        {
            string[] array = unicode.Replace("\\", "").Split('u');
            try
            {
                for (int i = 1; i < array.Length; i++)
                {
                    text += (char)int.Parse(array[i], NumberStyles.HexNumber);
                }
            }
            catch (Exception ex)
            {
                UniLogger.LogException(ex);
            }
        }

        return text;
    }

    public static string Reverse(string original)
    {
        char[] array = original.ToCharArray();
        Array.Reverse(array);
        return new string(array);
    }

    public static string LangConvert(string fullStr, string lang)
    {
        if (string.IsNullOrEmpty(fullStr) || string.IsNullOrEmpty(lang))
        {
            return "";
        }

        string text = fullStr;
        int num = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '[' && i + 4 <= text.Length && text[i + 3] == ']')
            {
                string text2 = text.Substring(i + 1, 2);
                if (text2 == lang)
                {
                    num = i + 4;
                    break;
                }
            }
        }

        if (num > 0)
        {
            text = text.Remove(0, num);
        }

        num = 0;
        for (int j = 0; j < text.Length; j++)
        {
            if (text[j] == '[' && j + 5 <= text.Length && text[j + 4] == ']')
            {
                string text3 = text.Substring(j + 2, 2);
                if (text3 == lang)
                {
                    num = j;
                    break;
                }
            }
        }

        if (num > 0)
        {
            text = text.Remove(num, text.Length - num);
        }

        return text;
    }

    public static int CalcStringLenByUCS(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }

        int num = 0;
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        int num2;
        for (num2 = 0; num2 < bytes.Length; num2++)
        {
            int num3 = OneCharHasByteCountByUCS(bytes[num2]);
            num2 += num3 - 1;
            num += Math.Min(2, num3);
        }

        return num;
    }

    public static string SubStringByUCS(string text, int startIndex, int len = 0, bool thinner = true)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        int num = 0;
        bool flag = false;
        StringBuilder newStringBuilder = NewStringBuilder;
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        int num2;
        for (num2 = 0; num2 < bytes.Length; num2++)
        {
            byte b = bytes[num2];
            int num3 = OneCharHasByteCountByUCS(b);
            num += Math.Min(2, num3);
            if (!flag && num >= startIndex)
            {
                flag = true;
                startIndex = num - Math.Min(2, num3);
            }

            if (flag)
            {
                switch (num3)
                {
                    case 1:
                        newStringBuilder.Append((char)b);
                        break;
                    case 2:
                    {
                        byte b7 = bytes[num2 + 1];
                        int num6 = (b & 0x1F) << 6;
                        num6 |= b7 & 0x3F;
                        break;
                    }
                    case 3:
                    {
                        byte b5 = bytes[num2 + 1];
                        byte b6 = bytes[num2 + 2];
                        if (b != 239 || b5 != 187 || b6 != 191)
                        {
                            int num5 = (b & 0xF) << 12;
                            num5 |= (b5 & 0x3F) << 6;
                            num5 |= b6 & 0x3F;
                            newStringBuilder.Append((char)num5);
                        }

                        break;
                    }
                    case 4:
                    {
                        byte b2 = bytes[num2 + 1];
                        byte b3 = bytes[num2 + 2];
                        byte b4 = bytes[num2 + 3];
                        int num4 = (b & 7) << 18;
                        num4 |= (b2 & 0x3F) << 12;
                        num4 |= (b3 & 0x3F) << 6;
                        num4 |= b4 & 0x3F;
                        newStringBuilder.Append((char)num4);
                        break;
                    }
                }
            }

            num2 += num3 - 1;
            if (len > 0)
            {
                if (startIndex + len == num)
                {
                    break;
                }

                if (startIndex + len < num)
                {
                    if (thinner)
                    {
                        newStringBuilder.Remove(newStringBuilder.Length - 1, 1);
                    }

                    break;
                }
            }
        }

        return newStringBuilder.ToString();
    }

    public static int OneCharHasByteCountByUCS(byte firstByte)
    {
        if ((firstByte & 0x80) == 0)
        {
            return 1;
        }

        if ((firstByte & 0xE0) == 192)
        {
            return 2;
        }

        if ((firstByte & 0xF0) == 224)
        {
            return 3;
        }

        if ((firstByte & 0xF8) == 240)
        {
            return 4;
        }

        return 1;
    }
}