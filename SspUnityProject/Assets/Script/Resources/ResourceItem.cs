using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resource
{
    public class ResourceItem : MonoBehaviour
    {

        public Text Label;
        public IResource Resource { get; set; }

        public void SetContent(IResource res)
        {
            Resource = res;
            Label.text = res.Name;
        }
    }
}
