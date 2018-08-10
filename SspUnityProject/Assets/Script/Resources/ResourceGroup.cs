using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resource
{
    public class ResourceGroup
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

        public ResourceGroup(List<IResourceRef> refList)
        {
            ResourceRefs = refList;
        }

        public ResourceGroup(List<IResourceRef> refList, int priority)
        {
            foreach (var item in refList)
            {
                ResourceRefs.Add(item);
            }
            Priority = priority;
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

        public ResourceGroup ConvertToResourceGroup(ResourcesManager resManager)
        {
            List<IResourceRef> resList = new List<IResourceRef>();
            foreach (var r in ResourceRefs)
            {
                IResourceRef resRef;
				resRef = new IResourceRef(resManager.Containor.GetResourceByGUID(r.GUID));
                resList.Add(resRef);
            }
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
