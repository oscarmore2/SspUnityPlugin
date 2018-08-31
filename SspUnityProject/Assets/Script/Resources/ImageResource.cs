using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace Resource
{
    public class ImageResource : IResource
    {
        public ImageResource()
        {

        }

        public ImageResource(ResourcesManager _manager)
        {
            manager = _manager;
        }

        Texture2D Data;
		public int Width;
		public int Height;
        public override object GetFile()
        {
            if (null == fileRef)
            {
                return null;
            }

            return (object)Data;
        }

        public override string GetPath()
        {
            throw new NotImplementedException();
        }

        public override ResourceType GetType()
        {
            return ResourceType.Image;
        }

		public override byte[] GetBytes ()
		{
			return fileRef;
		}

		public override void LoadConfig(JsonData data)
		{
			var _Path = data ["Path"].ToString();
			_Path = _Path.Replace ("%streamingAssetsPath%", Application.streamingAssetsPath);
			this.Path = _Path;
		}

		public override IEnumerator LoadFile(Action callback)
        {
            WaitUntil loadByte = new WaitUntil(() =>
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
            
			WaitUntil convertImage = new WaitUntil(() =>
				{
					if (fileRef != null)
					{
						Data = new Texture2D(Width, Height);
						Data.LoadImage(fileRef);
						return Data.GetRawTextureData().Length > 0;
					}
					else{
						return true;	
					}
				});
			yield return loadByte;
			yield return 1;
			yield return convertImage;
			callback ();
        }

        public override void SetConfig(JsonData data)
        {
			base.SetConfig(data);
        }

		public class ImageResourceGenerator
		{
			public static JsonData data;
			public static ImageResource Generate(JsonData _data)
			{
				var res = JsonConfiguration.GetData<ImageResource>(_data);
				res.LoadConfig (_data);
				return res;
			}
		}
    }
}
