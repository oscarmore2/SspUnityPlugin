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

		public virtual void SetConfig(JsonData data){
			if (data.Keys.Contains("GUID"))
				this.GUID = data ["GUID"].ToString ();
			if (data.Keys.Contains("Path"))
				this.Path = data ["Path"].ToString ();
			if (data.Keys.Contains("Name"))
				this.Name = data ["Name"].ToString ();
			if (data.Keys.Contains("Priority"))
				this.Priority = int.Parse (data ["Priority"].ToString());

			var json = new JsonData();
			if (data.Keys.Contains ("index"))
				json ["index"] = data ["index"];
			else {
				json ["index"] = manager.Containor.getIndex (this);
			}

			json ["data"] = new JsonData ();
			json["data"]["Type"] = "Text";
			json ["data"] ["Name"] = this.Name;
			json ["data"] ["Path"] = this.Path;
			json["data"]["GUID"] = this.GUID;
			json ["data"] ["Priority"] = this.Priority;

			manager.Containor.SetConfig (json);
		}
    }
}
