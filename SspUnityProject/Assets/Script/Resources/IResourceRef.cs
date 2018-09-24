using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resource
{
    public class IResourceRef 
    {

        public int Priority;
        public IResource Resources;

        public IResourceRef(IResource res, int priority)
        {
            Resources = res;
            Priority = priority;
        }

        public IResourceRef(IResource res)
        {
            Resources = res;
        }
    }
}