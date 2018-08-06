using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resource
{
    public class ResourcesManager : MonoBehaviour
    {
        private ResourcesListContainor containor;
        public ResourcesListContainor Containor {
            get {
                return containor;
            }
        }

        public void Init()
        {
            containor = new ResourcesListContainor();
			ResourceGenerator.InitionMapping ["Text"] = Resource.TextResoure.TextResoureGenerator.Generate;
			ResourceGenerator.InitionMapping ["Image"] = Resource.ImageResource.ImageResourceGenerator.Generate;
            ResourceGenerator.OnGenerate(Paths.CONFIG + "resourceConfig_new.json", ref containor);
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

    public class ResourceGenerator
    {
		public delegate IResource Creator(LitJson.JsonData _data); 
		public static Dictionary<string, Creator> InitionMapping = new Dictionary<string, Creator>();
        public static void OnGenerate(string path, ref ResourcesListContainor containor)
        {
            var ResourceConfig = new JsonConfiguration(path);
            //var TextResource = ResourceConfig["Texts"];
            //var ImageResource = ResourceConfig["Images"];

            //for (int i = 0; i < TextResource.Count; i++)
            //{
            //    var res = JsonConfiguration.GetData<TextResoure>(TextResource[i]);
            //    containor.AddResource(res);
            //}

            //for (int i = 0; i < ImageResource.Count; i++)
            //{
            //    var res = JsonConfiguration.GetData<ImageResource>(ImageResource[i]);
            //    containor.AddResource(res);
            //}

            for (int i = 0; i < ResourceConfig.Count; i++)
            {
				var res  = InitionMapping [ResourceConfig [i] ["Type"].ToString()] (ResourceConfig [i]);
				containor.AddResource(res);
            }
        }

    }
}

