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
        RectTransform rt = GetComponent<RectTransform>();
        if (contentsizeFilter != null) {
			contentsizeFilter.enabled = false;
		}

		if (transform.childCount > 0 & GetComponent<HorizontalLayoutGroup>() == null && GetComponent<GridLayoutGroup>() == null) {
            var LayoutGroup = GetComponent<VerticalLayoutGroup>();
			for (int i = 0; i < transform.childCount; i++) {
                if (transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    var sub_calc = transform.GetChild(i).gameObject.AddComponent<ViewVertiheightCalculator>();
                    if (LayoutGroup == null)
                    {
                        height += (sub_calc.doCalculate() + pedding);
                    }
                    else {
                        height += (sub_calc.doCalculate() + LayoutGroup.spacing);
                    }
                    Destroy(sub_calc);
                }
			}
            if (LayoutGroup != null)
            {
                height += 2 * pedding;
            }
            else {
                height +=  (LayoutGroup.padding.top + LayoutGroup.padding.bottom);
            }
            
		} else {
			height = rt.sizeDelta.y;
		}

        
		layout.minHeight = height;
        var x = rt.sizeDelta.x == 0 ? 420 : rt.sizeDelta.x;
        rt.sizeDelta = new Vector2(x, height);


        return height;
	}
}
