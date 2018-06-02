using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextResoure : IResource {

	public override void SetConfig ()
	{
		
	}

	public override void LoadConfig ()
	{
		
	}

	public override string GetPath ()
	{
		return Path;
	}

	public override void SetDefaultConfig ()
	{
		
	}

	public override object GetFile ()
	{
		if (null == fileRef) {
			return null;
		}

		return (object)System.Text.Encoding.Default.GetString ( fileRef );
	}

	IEnumerator LoadFile()
	{
		WaitUntil wait = new WaitUntil (() => {
			fileRef = System.IO.File.ReadAllBytes (Path);
			return fileRef != null;
		});
		yield return wait;
	}

	public override ResourceType GetType ()
	{
		return ResourceType.Text;
	}
}
