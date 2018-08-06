using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace Resource
{
    public class TextResoure : IResource
    {
        public override string GetPath()
        {
            return Path;
        }

        public override object GetFile()
        {
            if (null == fileRef)
            {   
                return null;
            }

            return (object)System.Text.Encoding.Default.GetString(fileRef);
        }

		public override void LoadConfig(JsonData data)
		{
			var _Path = data ["Path"].ToString();
			_Path = _Path.Replace ("%streamingAssetsPath%", Application.streamingAssetsPath);
			this.Path = _Path;
		}

		public override void SetConfig(JsonData data)
		{
			
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
