using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRenderTarget : MonoBehaviour{

    public int Prioirty;
    public CommonResourceRenderer renderer;
	public Resource.ResourceGroup resourceGroup;

    public static ResourceRenderTarget Create(GameObject parent, int _priority)
    {
        var obj = new GameObject("render target");
        var comonent = obj.AddComponent<ResourceRenderTarget>();
        comonent.transform.parent = parent.transform;
        comonent.Prioirty = _priority;
        comonent.transform.SetSiblingIndex(_priority);
        return comonent;
    }

	public void SetTarget(Resource.ResourceGroup rg)
	{
		transform.localScale = new Vector3(rg.Scale, rg.Scale, rg.Scale);
		RectTransform rect = GetComponent<RectTransform>();
		rect.anchoredPosition = rect.anchoredPosition + new Vector2 (rg.XAxis, rg.YAxis);
		rect.sizeDelta = new Vector2 (rect.sizeDelta.x * rg.Scale, rect.sizeDelta.y * rg.Scale);
		transform.SetSiblingIndex (renderer.Priority * rg.Priority);
	}
}
