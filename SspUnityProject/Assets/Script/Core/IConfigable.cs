using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public interface IConfigable {

	void LoadConfig ();
	void SetConfig();
    void SetDefaultConfig();

}

public class Configuration : IniFiles
{
}
