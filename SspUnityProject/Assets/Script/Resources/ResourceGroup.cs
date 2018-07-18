using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ResourceGroup  {
    public string Name;
    public int Priority;
    public bool[] VCamBiding = new bool[] { false, false, false, false, false, false, false, false, false };
    public List<IResourceRef> ResourceRefs = new List<IResourceRef>();
    public bool[] ActivateState = new bool[] { false, false, false };
    public bool IsAfterTransition;

    public ResourceGroup(List<IResourceRef> refList)
    {
        ResourceRefs = refList;
    }

    public ResourceGroup(List<IResourceRef> refList, int priority)
    {
        foreach (var item in refList)
        {
            ResourceRefs.Add(item);
        }
        Priority = priority;
    }
}

public class ResourceGroupSerializer
{
    public class ResourceRefSerializer
    {
        public string type;
        public int index;
    }
    public string Name;
    public int Priority;
    public bool[] VCamBiding = new bool[] { false, false, false, false, false, false, false, false, false };
    public ResourceRefSerializer[] ResourceRefs;
    public bool[] ActivateState = new bool[] { false, false, false };
    public bool IsAfterTransition;

    public ResourceGroup ConvertToResourceGroup(string ResourceConfigPath)
    {
        var ResourceConfig = new JsonConfiguration(ResourceConfigPath);
        List<IResourceRef> resList = new List<IResourceRef>();
        foreach (var r in ResourceRefs)
        {
            IResourceRef resRef;
            if (r.type == "Texts")
            {
                var resource = ResourceConfig["Texts"];
                var res = JsonConfiguration.GetData<TextResoure>(resource[r.index]);
                resRef = new IResourceRef(res);
            }
            else if (r.type == "Images")
            {
                var resource = ResourceConfig["Images"];
                var res = JsonConfiguration.GetData<ImageResource>(resource[r.index]);
                resRef = new IResourceRef(res);
            }
            else
            {
                resRef = new IResourceRef(null);
            }
            resList.Add(resRef);
        }
        ResourceGroup rg = new ResourceGroup(resList);
        rg.Name = this.Name;
        rg.Priority = this.Priority;
        rg.VCamBiding = this.VCamBiding;
        rg.ActivateState = this.ActivateState;
        rg.IsAfterTransition = this.IsAfterTransition;
        return rg;
    }
}
