using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
	Text, Image, Sequence, Video
}

public abstract class IResource   {

	protected byte[] fileRef;
	protected string Path;

    public abstract ResourceType GetType();
    public abstract string GetPath();

	public abstract object GetFile ();
}
