using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resource
{
    public class ResourceGroupList : Singleton<ResourceGroupList>, IEnumerable
    {

        public event Action OnResourceGroupChange;

        List<ResourceGroup> containor = new List<ResourceGroup>();

        public int CurrentSelection = -1;

        public IEnumerator GetEnumerator()
        {
            return containor.GetEnumerator();
        }

        public void AddResourceGroup(ResourceGroup group)
        {
            OnResourceGroupChange();
            containor.Add(group);
        }

        public ResourceGroup this[int id]
        {
            get
            {
                return containor[id];
            }
        }

        public void Remove(ResourceGroup rg)
        {
            OnResourceGroupChange();
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
