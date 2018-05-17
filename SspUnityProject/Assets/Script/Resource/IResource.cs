using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{

}

public abstract class IResource  {

    public abstract ResourceType GetType();
    public abstract string GetPath();

}
