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

		public ResourceGroupManager manager { get; protected set;}


		public void SortGroup(bool inverse = false)
		{
			if (!inverse) {
				ResourceRefs.Sort ((left, right) => {
					if (left.Resources.Priority > right.Resources.Priority)
						return 1;
					else if (left.Resources.Priority == right.Resources.Priority)
						return 0;
					else
						return -1;
				});
			} else {
				ResourceRefs.Sort ((left, right) => {
					if (left.Resources.Priority < right.Resources.Priority)
						return 1;
					else if (left.Resources.Priority == right.Resources.Priority)
						return 0;
					else
						return -1;	
				});
			}
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
            var temp = this;
			ResourceGroupSerializer resSerializer = JsonMapper.ToObject<ResourceGroupSerializer> (data.ToJson());
			foreach (var r in resSerializer.ResourceRefs)
			{
				IResourceRef resRef;
				resRef = new IResourceRef(manager.ResourceManager.Containor.GetResourceByGUID(r.GUID));
				ResourceRefs.Add(resRef);
			}
            resSerializer.ConvertToResourceGroup(ref temp);
        }


		public void SetConfig(JsonData data)
		{
			if (data != null) {
				if (data.Keys.Contains ("Name"))
					this.Name = data ["Name"].ToString ();
				if (data.Keys.Contains ("Priority"))
					this.Priority = int.Parse (data ["Priority"].ToString ());
				if (data.Keys.Contains ("ActivateState"))
					this.ActivateState = new bool[] {data ["ActivateState"] [0].GetBoolean (), data ["ActivateState"] [1].GetBoolean (), 
						data ["ActivateState"] [2].GetBoolean ()
					};
				if (data.Keys.Contains ("IsAfterTransition"))
					this.IsAfterTransition = bool.Parse (data ["IsAfterTransition"].ToString ());
				if (data.Keys.Contains ("Scale"))
					this.Scale = float.Parse (data ["Scale"].ToString ());
				if (data.Keys.Contains ("XAxis"))
					this.XAxis = float.Parse (data ["XAxis"].ToString ());
				if (data.Keys.Contains ("YAxis"))
					this.YAxis = float.Parse (data ["YAxis"].ToString ());
				if (data.Keys.Contains ("Duration"))
					this.Duration = float.Parse (data ["Duration"].ToString ());

				if (data.Keys.Contains ("VCamBiding")) {
					object bools = data ["VCamBiding"];
					for (int i = 0; i < data ["VCamBiding"].Count; i++) {
						this.VCamBiding [i] = data ["VCamBiding"] [i].GetBoolean ();
					}
				}
			}

            //Call SetConfig in resource list
            var json = new JsonData();
            json["index"] = ResourceGroupList.Instance.getIndex(this);
            json["data"] = new JsonData();
            json["data"]["Name"] = Name;
			json["data"]["Priority"] = Priority;
            json["data"]["IsAfterTransition"] = IsAfterTransition;
			json["data"]["Scale"] = Scale;
			json["data"]["XAxis"] = XAxis;
			json["data"]["YAxis"] = YAxis;
			json["data"]["Duration"] = Duration;
            json["data"]["ActivateState"] = new JsonData();
            for (int i = 0; i < ActivateState.Length; i++)
            {
				json["data"]["ActivateState"].Add(false);
            }
            json["data"]["VCamBiding"] = new JsonData();
            for (int i = 0; i < VCamBiding.Length; i++)
            {
                json["data"]["VCamBiding"].Add(VCamBiding[i]);
            }
            json["data"]["ResourceRefs"] = new JsonData();
            for(int i = 0; i < ResourceRefs.Count; i++)
            {
				JsonData d = new JsonData ();
				d ["GUID"] = ResourceRefs [i].Resources.GUID;
				json["data"]["ResourceRefs"].Add(d);
            }
			Debug.Log (json.ToJson());
            ResourceGroupList.Instance.SetConfig(json);
        }
    }

    public class ResourceGroupSerializer
    {
        public class ResourceRefSerializer
        {
			public string GUID;
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

		public void ConvertToResourceGroup(ref ResourceGroup rg)
        {
            rg.Name = this.Name;
            rg.Priority = this.Priority;
            rg.VCamBiding = this.VCamBiding;
            rg.ActivateState = this.ActivateState;
            rg.IsAfterTransition = this.IsAfterTransition;

            rg.Scale = this.Scale;
            rg.XAxis = this.XAxis;
            rg.YAxis = this.YAxis;
            rg.Duration = this.Duration;
        }
    }
}
