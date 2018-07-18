using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGroupItem : MonoBehaviour {

    public ResourceGroup ResGroup;

    public Text Title;
    public ToggleGroup StateSelection;

    public InputField Scale;
    public InputField XAxis;
    public InputField YAxis;
    public InputField Duration;

    public Transform Containor;

    public void SetContent(ResourceGroup g)
    {
        for (int i = 0; i < g.ResourceRefs.Count; i++)
        {
            var gameobj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(""));
            var objAttr = gameobj.GetComponent<ResourceAttributeFields>();
            objAttr.SetContent(g.ResourceRefs[i].Resources.Attrs);
        }
    }
}
