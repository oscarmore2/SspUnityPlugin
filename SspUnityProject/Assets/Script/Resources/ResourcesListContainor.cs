using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Resource
{
    public class ResourcesListContainor : IEnumerable
    {
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
    }
}
