using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour {

    ResourcesListContainor containor;

    public Transform ResourcePool;

    void OnInit()
    {
        containor = new ResourcesListContainor();
        ResourceGenerator.OnGenerate("", ref containor);
        gameObject.SetActive(true);
    }

    void OnEnable()
    {
        if (containor != null)
        {
            gameObject.SetActive(false);
            return;
        }

        if (ResourcePool.childCount < containor.Count)
        {
            for (int i = 0; i < containor.Count; i++)
            {
                var o = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(""));
                var rect = o.GetComponent<Transform>();
                rect.parent = ResourcePool;


            }
        }
            
    }
}

public class ResourceGenerator
{
    public static void OnGenerate(string path, ref ResourcesListContainor containor)
    {
        var ResourceConfig = new JsonConfiguration(path);
        var TextResource = (JsonConfiguration) ResourceConfig["Texts"];
        var ImageResource = (JsonConfiguration) ResourceConfig["Images"];

        for (int i = 0; i < TextResource.Count; i++)
        {
            containor.AddResource(TextResource[i].GetData<TextResoure>());
        }

        for (int i = 0; i < TextResource.Count; i++)
        {
            containor.AddResource(ImageResource[i].GetData<TextResoure>());
        }
    }
}

