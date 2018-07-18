using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGroupManager : MonoBehaviour {
    

    public void OnInit()
    {
        ResourceGroupeGenerator.GenerateResourceGroupList(Paths.RESOURCE_GROUP);
        gameObject.SetActive(true);
    }

    public void OnAddResourceGroup(List<IResource> ListRes, string Name = "")
    {
        var resGroup = ResourceGroupeGenerator.GenerateResourceGroup(ListRes);
        resGroup.Name = Name;
        ResourceGroupList.Instance.AddResourceGroup(resGroup);
    }
}


public class ResourceGroupeGenerator
{
    public static void GenerateResourceGroupList(string config)
    {
        var groupConfig = new JsonConfiguration(config);
        for (int i = 0; i < groupConfig.Count; i++)
        {
            var re = LitJson.JsonMapper.ToObject<ResourceGroupSerializer>(groupConfig[i].ToJson());
            ResourceGroupList.Instance.AddResourceGroup(re.ConvertToResourceGroup(Paths.RESOURCE));
        }
    }

    public static ResourceGroup GenerateResourceGroup(List<IResource> resList)
    {
        List<IResourceRef> RefList = new List<IResourceRef>();
        for (int i = 0; i < resList.Count; i++)
        {
            IResourceRef refs = new IResourceRef(resList[i]);
            RefList.Add(refs);
        }
        ResourceGroup rg = new ResourceGroup(RefList);
        return rg;
    } 
}