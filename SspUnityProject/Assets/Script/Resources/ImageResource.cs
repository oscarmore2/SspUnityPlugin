using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resource
{
    public class ImageResource : IResource
    {
        Texture2D Data;
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

        public IEnumerator LoadFile()
        {
            WaitUntil wait = new WaitUntil(() =>
            {
                fileRef = System.IO.File.ReadAllBytes(Path);
                return fileRef != null;
            });
            yield return wait;
            Data.LoadImage(fileRef);
        }
    }
}
