using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IView : MonoBehaviour {

    [SerializeField]
    protected RawImage ViewImage;

    public virtual void ShowView()
    {
        OnShown();
    }

    public virtual void HideView()
    {
        OnHided();
    }

    protected abstract void OnShown();
    protected abstract void OnHided();
}
