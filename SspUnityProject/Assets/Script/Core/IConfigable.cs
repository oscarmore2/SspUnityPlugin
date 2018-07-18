using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using IniParser;
using IniParser.Model;

public interface IConfigable {


	void LoadConfig ();
	void SetConfig();
    void SetDefaultConfig();

}

public class Configuration : IniData
{
	public FileIniDataParser Parser;

	public Configuration()
	{
	}

	public Configuration (string path) 
	{
		Parser = new FileIniDataParser ();
		Merge(Parser.ReadFile (path));
	}
    
}

public class JsonConfiguration : JsonData
{

    JsonData data = new JsonData();

    public JsonConfiguration(string path)
    {
        var s = LoadFromFile(path);
        data = JsonMapper.ToObject(s);
    }

    string LoadFromFile(string path)
    {
        if (path == string.Empty)
            throw new System.ArgumentException("Bad filename.");

        try
        {
            // (FileAccess.Read) we want to open the ini only for reading 
            // (FileShare.ReadWrite) any other process should still have access to the ini file 
            using (System.IO.FileStream fs = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fs, System.Text.Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        catch (System.IO.IOException ex)
        {
            throw new System.Exception(System.String.Format("Could not parse file {0}", path), ex);
        }
    }

    public JsonData this[int id]
    {
        get {
            return data[id];
        }
        set
        {
            data[id] = value;
        }
    }

    public JsonData this[string str]
    {
        get
        {
            return data[str];
        }
        set
        {
            data[str] = value;
        }
    }

    public static T GetData<T>(JsonData data)
    {
        return JsonMapper.ToObject<T>(data.ToJson());
    }
}
