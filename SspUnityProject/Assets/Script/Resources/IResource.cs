using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace Resource
{
    public enum ResourceType
    {
        Text, Image, Sequence, Video
    }

    public abstract class IResource : IJsonConfigable
    {
		protected ResourcesManager manager;
        public string Name;
        public string Path;
        public Dictionary<string, string> Attrs;

		public IResource (ResourcesManager _manager)
		{
			manager = _manager;
		}

        protected byte[] fileRef;

		public string GUID;

        public int Priority;


        public abstract ResourceType GetType();
        public abstract string GetPath();

		public abstract IEnumerator LoadFile (Action callback);
        public abstract object GetFile();

		public abstract byte[] GetBytes ();

        public abstract void LoadConfig(JsonData data);

        public abstract void SetConfig(JsonData data);
    }
}
