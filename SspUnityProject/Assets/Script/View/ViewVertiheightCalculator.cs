using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class ViewVertiheightCalculator : MonoBehaviour {

	public float height = 0f;

	public float pedding;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Calculate()
	{
		doCalculate ();
	}

	public float doCalculate()
	{
		var layout = transform.GetComponent<LayoutElement> ();
		var contentsizeFilter = transform.GetComponent<ContentSizeFitter> ();
		if (contentsizeFilter != null) {
			contentsizeFilter.enabled = false;
		}

		if (transform.childCount > 0) {
			for (int i = 0; i < transform.childCount; i++) {
				var sub_calc = transform.GetChild (i).gameObject.AddComponent<ViewVertiheightCalculator> ();
				height += (sub_calc.doCalculate () + pedding);
				Destroy (sub_calc);
			}
			height += 2 * pedding;
		} else {
			RectTransform rt = GetComponent<RectTransform> ();
			height = rt.sizeDelta.y;
		}

		layout.preferredHeight = height;

		return height;
	}
}
