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

		[SerializeField]
		int fileSize = 0;

        public void SetContent(IResource res)
        {
            Resource = res;
            Label.text = res.Name;
			StartCoroutine (res.LoadFile(()=>{
                if (res.GetBytes() == null)
                {
                    if (res.GetBytes() != null)
                    {
                        fileSize = res.GetBytes().Length;
                    }
                }
			}));
        }


    }
}
