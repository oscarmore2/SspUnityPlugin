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
			for (int i = 0; i < listGroup.Count; i++)
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
			JsonConfiguration.WriteData (Data, Paths.RESOURCE_GROUP);
		}

        public int getIndex(ResourceGroup rg)
        {
            return containor.IndexOf(rg);
        }

		public void Sort()
		{
			containor.Sort ((left, right) => {
				if (left.Priority > right.Priority)
					return 1;
				else if (left.Priority == right.Priority)
					return 0;
				else
					return -1;
			});
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

        public override void OnInitialize()
        {

        }

        public override void OnUninitialize()
        {
            containor.Clear();
        }
    }
}
