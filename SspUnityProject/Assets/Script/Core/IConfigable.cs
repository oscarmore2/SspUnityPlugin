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
