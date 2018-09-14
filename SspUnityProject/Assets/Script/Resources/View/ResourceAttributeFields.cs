using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resource
{
    public class ResourceAttributeFields : MonoBehaviour
    {

        public Transform AttrContainor;

        [SerializeField]
        Text Name;

        [SerializeField]
        GridLayoutGroup grid;

        public void SetContent(Resource.IResource res)
        {
            Name.text = res.Name;
            foreach (var kp in res.Attrs)
            {
                GameObject obKey = new GameObject(kp.Key + ":key");
                var txKey = obKey.AddComponent<Text>();
                txKey.font = Font.CreateDynamicFontFromOSFont("Arial", 12);
                txKey.alignment = TextAnchor.MiddleLeft;
                txKey.text = kp.Key;
                txKey.color = Color.black;

                GameObject obValue = new GameObject(kp.Value + ":value");
                var txValue = obValue.AddComponent<Text>();
                txValue.font = Font.CreateDynamicFontFromOSFont("Arial", 12);
                txValue.alignment = TextAnchor.MiddleLeft;
                txValue.text = kp.Value;
                txValue.color = Color.black;

                txKey.transform.parent = AttrContainor;
                txValue.transform.parent = AttrContainor;
            }

            grid.CalculateLayoutInputHorizontal();
            grid.CalculateLayoutInputHorizontal();
        }
    }
}
