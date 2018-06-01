using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System;

public class IniFiles : IDictionary
{
    public string inipath;

    private string val;

    Dictionary<string, string> storage = new Dictionary<string, string>();
    Dictionary<string, List<string>> SectionKeyMap = new Dictionary<string, List<string>>();

    object IDictionary.this[object key]
    {
        get {
            if (!(key is string))
            {
                throw new ArgumentException("The key has to be a string");
            }
            return storage[(string)key]; }
        set
        {
            if (!(key is string))
            {
                throw new ArgumentException("The key has to be a string");
            }
            if (!(value is string))
            {
                throw new ArgumentException("The value has to be a string");
            }
            val = (string)value;
            storage[(string)key] = (string)value;
        }
    }

    public string this[string name]
    {
        get
        {
            return (string)storage[name];
        }
        set
        {
            storage[name] = value;
        }
    }

    public static IniFiles LoadFile(string url)
    {
        var iniFile = new IniFiles(url);
        if (iniFile.ExistINIFile())
        {
            return iniFile;
        }
        else
        {
            return null;
        }
    }

    public int this[object name]
    {
        get
        {
            int i = int.MinValue;
            int.TryParse(this[name].ToString(), out i);
            return i;
        }
        set
        {
            this[(string)name] = value.ToString();
        }
    }

    public int ToInt()
    {
        int i = int.MinValue;
        if (val != null)
        {
            int.TryParse(val.ToString(), out i);
        }
        return i;
    }

    public bool IsFixedSize
    {
        get
        {
            return ((IDictionary)storage).IsFixedSize;
        }
    }

    public bool IsReadOnly
    {
        get
        {
            return ((IDictionary)storage).IsReadOnly;
        }
    }

    public ICollection Keys
    {
        get
        {
            return storage.Keys;
        }
    }

    public ICollection Values
    {
        get
        {
            return storage.Values;
        }
    }

    public int Count
    {
        get
        {
            return storage.Count;
        }
    }

    public bool IsSynchronized
    {
        get
        {
            return ((IDictionary)storage).IsSynchronized;
        }
    }

    public object SyncRoot
    {
        get
        {
            return ((IDictionary)storage).SyncRoot;
        }
    }

    //声明API函数

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
    /// <summary> 
    /// 构造方法 
    /// </summary> 
    /// <param name="INIPath">文件路径</param> 
    public IniFiles(string INIPath)
    {
        inipath = INIPath;
    }

    public IniFiles() { }

    /// <summary> 
    /// 写入INI文件 
    /// </summary> 
    /// <param name="Section">项目名称(如 [TypeName] )</param> 
    /// <param name="Key">键</param> 
    /// <param name="Value">值</param> 
    public void IniWriteValue(string Section, string Key, string Value)
    {
        WritePrivateProfileString(Section, Key, Value, this.inipath);
    }

    public void WriteAll()
    {
        foreach (var section in SectionKeyMap.Keys)
        {
            foreach (var val in SectionKeyMap[section])
            {
                IniWriteValue(section, val, storage[val]);
            }
        }
    }
    /// <summary> 
    /// 读出INI文件 
    /// </summary> 
    /// <param name="Section">项目名称(如 [TypeName] )</param> 
    /// <param name="Key">键</param> 
    public string IniReadValue(string Section, string Key)
    {
        StringBuilder temp = new StringBuilder(500);
        int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
        if (SectionKeyMap.ContainsKey(Section))
        {
            SectionKeyMap[Section].Add(Key);
        }
        else
        {
            SectionKeyMap[Section] = new List<string>();
            SectionKeyMap[Section].Add(Key);
        }
        storage[Key] = temp.ToString();
        return temp.ToString();
    }
    /// <summary> 
    /// 验证文件是否存在 
    /// </summary> 
    /// <returns>布尔值</returns> 
    public bool ExistINIFile()
    {
        return File.Exists(inipath);
    }

    public void Add(object key, object value)
    {
        storage.Add((string)key, (string)value);
    }

    public void Clear()
    {
        storage.Clear();
    }

    public bool Contains(object key)
    {
        return storage.ContainsKey((string)key);
    }

    public IDictionaryEnumerator GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Remove(object key)
    {
        storage.Remove((string)key);
    }

    public void CopyTo(Array array, int index)
    {
        ((IDictionary)storage).CopyTo(array, index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IDictionary)storage).GetEnumerator();
    }
}

