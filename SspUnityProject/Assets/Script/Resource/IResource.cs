using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{

}

interface IResource  {

    ResourceType GetType();
    string GetPath();

}
