using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceAttributeFields : MonoBehaviour {

    public Transform AttrContainor;

    public void SetContent(Dictionary<string, string> AttrKeyPair)
    {
        foreach (var kp in AttrKeyPair)
        {
            GameObject obKey = new GameObject(kp.Key + ":key");
            var txKey = obKey.AddComponent<Text>();
            txKey.fontSize = 12;
            txKey.alignment = TextAnchor.MiddleLeft;
            txKey.text = kp.Key;

            GameObject obValue = new GameObject(kp.Value + ":value");
            var txValue = obValue.AddComponent<Text>();
            txValue.fontSize = 12;
            txValue.alignment = TextAnchor.MiddleLeft;
            txValue.text = kp.Value;

            txKey.transform.parent = AttrContainor;
            txValue.transform.parent = AttrContainor;
        }
    }
}
