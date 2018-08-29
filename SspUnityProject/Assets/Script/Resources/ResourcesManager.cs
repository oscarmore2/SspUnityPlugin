using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resource
{
    public class ResourcesManager : MonoBehaviour
    {
		public static readonly string RESOURCE_CONFIG = Paths.CONFIG + "resourceConfig_new.json";
        private ResourcesListContainor containor;
        public ResourcesListContainor Containor {
            get {
                return containor;
            }
        }

        public void Init()
        {
			containor = new ResourcesListContainor(this);
			var ResourceConfig = new JsonConfiguration(RESOURCE_CONFIG);
			containor.InitionMapping ["Text"] = Resource.TextResoure.TextResoureGenerator.Generate;
			containor.InitionMapping ["Image"] = Resource.ImageResource.ImageResourceGenerator.Generate;
			containor.LoadConfig(ResourceConfig);
            gameObject.SetActive(true);
        }

        public Transform ResourcePool;

        public ToggleGroup TaggleGroup;

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
                    var t = o.GetComponentInChildren<Toggle>();
                    TaggleGroup.RegisterToggle(t);
                }
            }
        }
    }
}

