using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace Resource
{
	public class ResourceGroupList : Singleton<ResourceGroupList>, IEnumerable, IJsonConfigable
    {
		public JsonData Data { get; private set;}

		protected ResourceGroupManager manager;

        public Action<ResourceGroup> OnResourceGroupChange;

        List<ResourceGroup> containor = new List<ResourceGroup>();

        public int CurrentSelection = -1;

        public IEnumerator GetEnumerator()
        {
            return containor.GetEnumerator();
        }

		public void Init(ResourceGroupManager _manager)
		{
			manager = _manager;	
		}

        public void AddResourceGroup(ResourceGroup group)
        {
            containor.Add(group);
        }

        public ResourceGroup this[int id]
        {
            get
            {
                return containor[id];
            }
        }

		public void LoadConfig(JsonData data)
		{
			Data = data;
			var listGroup = data["list"];
			for (int i = 0; containor.Count < listGroup.Count; i++)
			{
				ResourceGroup rg = new ResourceGroup (manager);
				rg.LoadConfig (listGroup [i]);
				AddResourceGroup(rg);
			}
		}


		public void SetConfig(JsonData data)
		{
			int index = -1;
			int.TryParse(data ["index"].ToString(), out index);
			if (index >= containor.Count) {
				ResourceGroup rg = new ResourceGroup (manager);
				rg.LoadConfig (data["data"]);
				AddResourceGroup(rg);
				Data["list"].Add (data ["data"]);
			} else {
				Data ["list"][index] = data ["data"];
			}
			Debug.Log (Data.ToJson ());
			JsonConfiguration.WriteData (Data, Paths.RESOURCE_GROUP);
		}

		public int RemoveItem(ResourceGroup rg)
		{
			int index = containor.IndexOf (rg);
			Data ["list"] [index] = new JsonData ();
			containor.Remove (rg);
			JsonConfiguration.WriteData (Data, Paths.RESOURCE_GROUP);
			return index;
		}

        public int getIndex(ResourceGroup rg)
        {
            return containor.IndexOf(rg);
        }

		public List<ResourceGroup> Sort(bool inverse = false)
		{
			List<ResourceGroup> sorted = new List<ResourceGroup> ();
			for (int i = 0; i < containor.Count; i++) {
				sorted.Add (containor[i]);
			}
			if (inverse) {
				sorted.Sort ((left, right) => {
					if (left.Priority < right.Priority)
						return 1;
					else if (left.Priority == right.Priority)
						return 0;
					else
						return -1;
				});
			} else {
				sorted.Sort ((left, right) => {
					if (left.Priority > right.Priority)
						return 1;
					else if (left.Priority == right.Priority)
						return 0;
					else
						return -1;
				});
			}
			return sorted;
		}

        public void Remove(ResourceGroup rg)
        {
            containor.Remove(rg);
        }

        public int Count
        {
            get
            {
                return containor.Count;
            }
        }

        public ResourceGroup Select(int id)
        {
            CurrentSelection = id;
            return containor[id];
        }

		public ResourceGroup GetLast()
		{
			return containor [containor.Count - 1];	
		}

        public override void OnInitialize()
        {

        }

        public override void OnUninitialize()
        {
            containor.Clear();
        }
    }
}
