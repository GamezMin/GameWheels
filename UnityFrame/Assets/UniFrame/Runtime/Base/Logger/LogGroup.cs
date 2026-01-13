using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

internal class LogGroup : IComparable<LogGroup>
{
    internal class LogTextList : List<string>
    {
        private static string strInfo1 = "< ";

        private static string strInfo2 = ">";

        private static string strInfo3 = "---Time:";

        private StringBuilder _sb = new StringBuilder();

        private int _totalCount = 0;

        public new void Add(string item)
        {
            lock (_sb)
            {
                if (_sb != null)
                {
                    _sb.Remove(0, _sb.Length);
                    _sb.Append(strInfo1);
                    _sb.Append(_totalCount);
                    _sb.Append(strInfo2);
                    _sb.Append(strInfo3);
                    _sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss (ffff)"));
                    _sb.AppendLine(item);
                    base.Add(_sb.ToString());
                    _totalCount++;
                }
            }
        }
    }
    
    public string GroupName = string.Empty;

    private LogTextList _content = new LogTextList();

    private StreamWriter _output = null;

    private string _filePath = "";

    private object _lockerObj = new object();

    public LogGroup(string name, string fullPath)
    {
        GroupName = name;
        _filePath = fullPath;
        try
        {
            _output = new StreamWriter(_filePath, append: true, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public int CompareTo(LogGroup other)
    {
        return GroupName.CompareTo(other.GroupName);
    }

    public void FreeHandler()
    {
        lock (_lockerObj)
        {
            try
            {
                if (_output != null)
                {
                    Debug.Log((object)("FreeHandler:" + GroupName));
                    _output.Close();
                    _output = null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            finally
            {
                _output = null;
            }
        }
    }

    public void AllocHandler()
    {
        lock (_lockerObj)
        {
            try
            {
                if (_output == null)
                {
                    Debug.Log((object)("AllocHandler:" + GroupName));
                    _output = new StreamWriter(_filePath, append: true, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    public void Add(string text)
    {
        lock (_lockerObj)
        {
            _content.Add(text);
        }
    }

    public void ToFile()
    {
        lock (_lockerObj)
        {
            if (_output == null || _content.Count <= 0)
            {
                return;
            }

            if (_content != null)
            {
                for (int i = 0; i < _content.Count; i++)
                {
                    _output.WriteLine(_content[i]);
                }

                _content.Clear();
            }

            _output.Flush();
        }
    }
}