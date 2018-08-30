using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace Resource
{
	public class ResourceGroup : IJsonConfigable
    {
        public string Name;
        public int Priority;
        public bool[] VCamBiding = new bool[] { false, false, false, false, false, false, false, false, false };
        public List<IResourceRef> ResourceRefs = new List<IResourceRef>();
        public bool[] ActivateState = new bool[] { false, false, false };
        public bool IsAfterTransition;

        public float Scale;
        public float XAxis;
        public float YAxis;
        public float Duration;

		ResourceGroupManager manager;


		public void SortGroup()
		{
			ResourceRefs.Sort ((left, right)=>{
				if (left.Resources.Priority > right.Resources.Priority)
					return 1;
				else if (left.Resources.Priority == right.Resources.Priority)
					return 0;
				else
					return -1;
			});
		}

		public ResourceGroup(ResourceGroupManager _manager)
		{
			manager = _manager;
		}

		public ResourceGroup(ResourceGroupManager _manager, List<IResourceRef> refList)
        {
			manager = _manager;
            ResourceRefs = refList;
        }

		public ResourceGroup(ResourceGroupManager _manager, List<IResourceRef> refList, int priority)
        {
            foreach (var item in refList)
            {
                ResourceRefs.Add(item);
            }
            Priority = priority;
        }

		public void LoadConfig(JsonData data)
		{
			ResourceGroupSerializer resManager = JsonMapper.ToObject<ResourceGroupSerializer> ();
			foreach (var r in resManager.ResourceRefs)
			{
				IResourceRef resRef;
				resRef = new IResourceRef(manager.ResourceManager.Containor.GetResourceByGUID(r.GUID));
				ResourceRefs.Add(resRef);
			}
		}


		public void SetConfig(JsonData data)
		{
			this.Name = data["Name"].ToString();
			this.Priority = int.Parse(data["Priority"].ToString());

			this.ActivateState = new bool[]{bool.Parse(data["ActivateState"][0]), bool.Parse(data["ActivateState"][1]), 
				bool.Parse(data["ActivateState"][2])};
			this.IsAfterTransition = bool.Parse(data["IsAfterTransition"].ToString());
			this.Scale = float.Parse(data["Scale"].ToString()) ;
			this.XAxis = float.Parse(data["XAxis"].ToString());
			this.YAxis = float.Parse(data["YAxis"].ToString());
			this.Duration = float.Parse(data["Duration"].ToString());

			if (data.Keys.Contains ("VCamBiding")) {
				object bools = data ["VCamBiding"];
				for (int i = 0; i < data ["VCamBiding"].Count; i++) {
					this.VCamBiding [i] = data ["VCamBiding"] [i];
				}
			}

			//Call SetConfig in resource list
		}
    }

    public class ResourceGroupSerializer
    {
        public class ResourceRefSerializer
        {
            public string type;
			public string GUID;
            public int index;
        }
        public string Name;
        public int Priority;
        public bool[] VCamBiding = new bool[] { false, false, false, false, false, false, false, false, false };
        public ResourceRefSerializer[] ResourceRefs;
        public bool[] ActivateState = new bool[] { false, false, false };
        public bool IsAfterTransition;

        public float Scale;
        public float XAxis;
        public float YAxis;
        public float Duration;

		public ResourceGroup ConvertToResourceGroup(ResourcesManager resManager, LitJson.JsonData)
        {
            List<IResourceRef> resList = new List<IResourceRef>();
            
            ResourceGroup rg = new ResourceGroup(resList);
            rg.Name = this.Name;
            rg.Priority = this.Priority;
            rg.VCamBiding = this.VCamBiding;
            rg.ActivateState = this.ActivateState;
            rg.IsAfterTransition = this.IsAfterTransition;

            rg.Scale = this.Scale;
            rg.XAxis = this.XAxis;
            rg.YAxis = this.YAxis;
            rg.Duration = this.Duration;
            return rg;
        }
    }
}
