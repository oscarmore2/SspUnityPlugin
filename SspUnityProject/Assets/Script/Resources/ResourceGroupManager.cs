using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resource
{
    public class ResourceGroupManager : MonoBehaviour
    {
        public Transform ResourceGroupContainor;

        ResourcesManager resourceManager;

        public void OnInit(ResourcesManager resManager)
        {
            ResourceGroupeGenerator.GenerateResourceGroupList(Paths.RESOURCE_GROUP, resManager);
            resourceManager = resManager;
            gameObject.SetActive(true);
        }

        public void OnAddToGroup()
        {
            var toggles = resourceManager.TaggleGroup.ActiveToggles();
            List<IResource> res = new List<IResource>();
            foreach (var t in toggles)
            {
                var comp = t.GetComponentInParent<ResourceItem>();
                res.Add(comp.Resource);
                t.isOn = false;
            }
            OnAddResourceGroup(res);
        }

        public void OnAddResourceGroup(List<IResource> ListRes, string Name = "")
        {
            var resGroup = ResourceGroupeGenerator.GenerateResourceGroup(ListRes);
            resGroup.Name = Name;
            AddItem(resGroup);
            ResourceGroupList.Instance.AddResourceGroup(resGroup);
        }

        void OnEnable()
        {
            if (ResourceGroupContainor == null || ResourceGroupList.Instance == null)
            {
                gameObject.SetActive(false);
                return;
            }

            if (ResourceGroupContainor.childCount < ResourceGroupList.Instance.Count)
            {
                for (int i = 0; i < ResourceGroupList.Instance.Count; i++)
                {
                    AddItem(ResourceGroupList.Instance[i]);
                }
            }
        }

        void AddItem(ResourceGroup rg)
        {
            var o = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/ResourceGroupItem"));
            var rect = o.GetComponent<Transform>();
            rect.parent = ResourceGroupContainor;
            o.GetComponent<ResourceGroupItem>().SetContent(rg);
        }

    }


    public class ResourceGroupeGenerator
    {
        public static void GenerateResourceGroupList(string config, ResourcesManager resManager)
        {
            var groupConfig = new JsonConfiguration(config);
            var listGroup = groupConfig["list"];
            for (int i = 0; i < listGroup.Count; i++)
            {
                var re = LitJson.JsonMapper.ToObject<ResourceGroupSerializer>(listGroup[i].ToJson());
                ResourceGroupList.Instance.AddResourceGroup(re.ConvertToResourceGroup(resManager));
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
}