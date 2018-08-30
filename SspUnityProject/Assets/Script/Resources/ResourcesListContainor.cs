using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using LitJson;

namespace Resource
{
	public class ResourcesListContainor : IEnumerable, IJsonConfigable
    {
		public JsonData Data { public get; private set;}

		public delegate IResource Creator(LitJson.JsonData _data); 
		public static Dictionary<string, Creator> InitionMapping = new Dictionary<string, Creator>();

		ResourcesManager manager;

		public ResourcesListContainor (ResourcesManager resManager)
		{
			manager = resManager;
		}

        List<IResource> containorList = new List<IResource>();

        public int CurrentSelection = -1;

        public IEnumerator GetEnumerator()
        {
            return containorList.GetEnumerator();
        }

        public void AddResource(IResource res)
        {
            containorList.Add(res);
        }

		public void LoadConfig(JsonData data)
		{
			Data = data;
			for (int i = 0; i < data.Count; i++)
			{
				var res  = InitionMapping [data [i] ["Type"].ToString()] (data [i]);
				this.AddResource(res);
			}
		}


		public void SetConfig(JsonData data)
		{
			int index = -1;
			int.TryParse(data ["index"].ToString(), out index);
			if (index >= containorList.Count) {
				var res = InitionMapping [data ["data"] ["Type"].ToString ()] (data ["data"]);
				this.AddResource (res);
				Data.Add (data ["data"]);
			} else {
				Data [index] = data ["data"];
			}
			JsonConfiguration.WriteData (Data, Paths.RESOURCE);
		}

        public IResource this[int id]
        {
            get
            {
                return containorList[id];
            }
        }

        public int Count
        {
            get { return containorList.Count; }
        }

        public T GetResource<T>(int id) where T : IResource
        {
            IResource res = containorList[id];
            if (typeof(T).BaseType == typeof(IResource))
            {
                T des = (T)(containorList[id]);
                return des;
            }
            return null;
        }

		public IResource GetResourceByGUID(string GUID)
		{
			foreach (var res in containorList) {
				if (res.GUID == GUID) {
					return res;
				}
			}
			return null;
		}

		public int getIndex(IResource resource)
		{
			return containorList.IndexOf (resource);
		}
    }
}
