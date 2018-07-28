using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resource
{
    public class ImageResource : IResource
    {

        public override object GetFile()
        {
            throw new NotImplementedException();
        }

        public override string GetPath()
        {
            throw new NotImplementedException();
        }

        public override ResourceType GetType()
        {
            return ResourceType.Image;
        }
    }
}
