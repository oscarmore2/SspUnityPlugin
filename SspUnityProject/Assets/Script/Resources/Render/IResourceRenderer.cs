using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IResourceRenderer : MonoBehaviour {

    public int Priority;

    public bool IsAfterTransition;

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

    public abstract void ChangeContent(Resource.IResource contentData);

    public abstract Resource.ResourceType GetType();
}
