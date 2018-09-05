using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace Resource
{
    public class TextResoure : IResource
    {
		string Content = null;
        public TextResoure()
        {

        }

        public TextResoure (ResourcesManager _manager)
		{
            manager = _manager;
		}

        public override string GetPath()
        {
            return Path;
        }

        public override object GetFile()
        {
			if (null != fileRef) {   
				Content = System.Text.Encoding.UTF8.GetString(fileRef);
			}

			return (object)Content;
        }

		public override void LoadConfig(JsonData data)
		{
			if (data.Keys.Contains ("Path")) {
				var _Path = data ["Path"].ToString ();
				_Path = _Path.Replace ("%streamingAssetsPath%", Application.streamingAssetsPath);
				this.Path = _Path;
			} else {
				Content = data ["Text"].ToString();
			}
		}

		public override void SetConfig(JsonData data)
		{
			base.SetConfig(data);
		}

		public override byte[] GetBytes ()
		{
			return fileRef;
		}

		public override IEnumerator LoadFile(System.Action callback)
        {
            WaitUntil wait = new WaitUntil(() =>
            {
				if (System.IO.File.Exists(Path))
				{
					fileRef = System.IO.File.ReadAllBytes(Path);
					return fileRef != null;
				}
				else{
					return true;	
				}
            });
            yield return wait;
			callback();
        }

        public override ResourceType GetType()
        {
            return ResourceType.Text;
        }

		public class TextResoureGenerator
		{
			public static JsonData data;
			public static TextResoure Generate(JsonData _data)
			{
				var res = JsonConfiguration.GetData<TextResoure>(_data);
				res.LoadConfig (_data);
				return res;
			}
		}
    }
}
