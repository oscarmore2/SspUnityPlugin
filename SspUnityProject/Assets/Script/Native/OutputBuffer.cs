using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputConfig
{
	public int Width;
	public int Height;
	public int FPS;
	public string OutputPath;
}

public class OutputBuffer : MonoBehaviour, IConfigable {

	public static OutputBuffer Instance { get; private set;}

	public OutputConfig Config;

	public void LoadConfig ()
	{
		
	}

	public void SetConfig()
	{
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
