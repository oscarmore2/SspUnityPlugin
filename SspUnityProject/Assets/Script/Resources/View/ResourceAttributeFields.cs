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

		int priority;

		[SerializeField]
		RawImage backg;

		[SerializeField]
		GameObject LayerKey;

        [SerializeField]
        GridLayoutGroup grid;

        IResourceRef resource;
		ResourceGroupItem resourceGroup;

		public void SetContent(IResourceRef res, ResourceGroupItem rg)
        {
            resourceGroup = rg;
            resource = res;
            Name.text = res.Resources.Name;
            foreach (var kp in res.Resources.Attrs)
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

        public void OnMoveUp()
        {
            if (resource != null)
            {
				if (resource.Priority > 1) {
					resourceGroup.ResGroup.updatePriority (resource, resource.Priority - 1);
					resourceGroup.ReflreshUI ();
				}
            }
        }

		public void OnMoveDown()
		{
			if (resource != null) {
				if (resource.Priority < resourceGroup.ResGroup.ResourceRefs.Count) {
					resourceGroup.ResGroup.updatePriority (resource, resource.Priority + 1);
					resourceGroup.ReflreshUI ();
				}
			}
		}

    }
}
