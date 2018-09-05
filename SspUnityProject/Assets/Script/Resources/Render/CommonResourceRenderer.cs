using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommonResourceRenderer : MonoBehaviour {

    public int Priority;

    public string GUID;

    public bool IsAfterTransition;

    public List<GameObject> renderTarget = new List<GameObject>();

    protected UnityEngine.RectTransform rectTranfrom;

    public Dictionary<string, string> Attrs;

    public virtual void ApplyAttrs(Dictionary<string, string> _Attrs = null)
    {
        if (_Attrs == null)
            _Attrs = Attrs;
        else
            Attrs = _Attrs;

        float posX = _Attrs.ContainsKey("X") ? float.Parse(_Attrs["X"]) : 0;
        float posY = _Attrs.ContainsKey("Y") ? float.Parse(_Attrs["Y"]) : 0;
        rectTranfrom.anchoredPosition = new Vector2(posX, posY);
    }

	public virtual T AttachRenderTarget<T>(GameObject obj, Resource.ResourceGroup rg) where T : UnityEngine.UI.Graphic
    {
        var target = obj.GetComponent<ResourceRenderTarget>();
        target.renderer = this;
        return null;
    }

    public abstract void ChangeContent(Resource.IResource contentData);

    public abstract Resource.ResourceType GetType();
}
