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
    public JsonMapper Parser;

    public JsonConfiguration()
    {
    }

    public JsonConfiguration(string path)
    {
        this.Add(JsonMapper.ToObject(LoadFromFile(path)));
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

    public JsonConfiguration this[int id]
    {
        get {
            return (JsonConfiguration)this[id];
        }
    }

    public T GetData<T>()
    {
        return JsonMapper.ToObject<T>(this.ToJson());
    }
}
