using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resource
{
    public enum ResourceType
    {
        Text, Image, Sequence, Video
    }

    public abstract class IResource
    {

        public string Name;
        public string Path;
        public Dictionary<string, string> Attrs;

        protected byte[] fileRef;

        public abstract ResourceType GetType();
        public abstract string GetPath();

        public abstract object GetFile();
    }
}
