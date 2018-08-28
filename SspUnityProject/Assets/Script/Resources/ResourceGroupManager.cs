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

		public ResourcesManager ResourceManager
		{
			get{ return resourceManager; }
		}

        [SerializeField]
        ViewVertiheightCalculator heightCalculator;

        public void OnInit(ResourcesManager resManager)
        {
            GenerateResourceGroupList(Paths.RESOURCE_GROUP, resManager);
            resourceManager = resManager;
            gameObject.SetActive(true);
        }

		void GenerateResourceGroupList(string config, ResourcesManager resManager)
		{
			var groupConfig = new JsonConfiguration(config);
			var listGroup = groupConfig["list"];
			for (int i = 0; i < listGroup.Count; i++)
			{
				ResourceGroup rg = new ResourceGroup (resManager);
				rg.LoadData (listGroup [i].ToJson ());
				ResourceGroupList.Instance.AddResourceGroup(rg);
			}
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
                heightCalculator.doCalculate();
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


}