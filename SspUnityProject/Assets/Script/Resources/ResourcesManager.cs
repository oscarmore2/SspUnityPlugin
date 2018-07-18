using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour {

    [SerializeField]
    ResourcesListContainor containor;

    public Transform ResourcePool;

    public void OnInit()
    {
        containor = new ResourcesListContainor();
        ResourceGenerator.OnGenerate(Paths.CONFIG+ "resourceConfig.json", ref containor);
        gameObject.SetActive(true);
    }

    void OnEnable()
    {
        if (containor == null)
        {
            gameObject.SetActive(false);
            return;
        }

        if (ResourcePool.childCount < containor.Count)
        {
            for (int i = 0; i < containor.Count; i++)
            {
                var o = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/ResourceItem"));
                var rect = o.GetComponent<Transform>();
                rect.parent = ResourcePool;
                o.GetComponent<ResourceItem>().SetContent(containor[i]);
            }
        }
    }
}

public class ResourceGenerator
{
    public static void OnGenerate(string path, ref ResourcesListContainor containor)
    {
        var ResourceConfig = new JsonConfiguration(path);
        var TextResource = ResourceConfig["Texts"];
        var ImageResource = ResourceConfig["Images"];

        for (int i = 0; i < TextResource.Count; i++)
        {
            var res = JsonConfiguration.GetData<TextResoure>(TextResource[i]);
            containor.AddResource(res);
        }

        for (int i = 0; i < ImageResource.Count; i++)
        {
            var res = JsonConfiguration.GetData<ImageResource>(ImageResource[i]);
            containor.AddResource(res);
        }
    }
    
}

