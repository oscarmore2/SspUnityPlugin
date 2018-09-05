using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTransitionCreater : TransitionCreater {
	

	[SerializeField]
	string key;

	[SerializeField]
	float startValue;

	[SerializeField]
	float endValue;


	// Use this for initialization
	void Start () {
		transition = new Transition ();
		transition.duration = duration;
		float step = Mathf.Abs(endValue - startValue) / duration;
		transition.ValueField.Add (new Transition.ShaderParams<float> (key,startValue, endValue, step));
        transition.SetShader(shaderName);
	}

	public override void OnChangeDuration ()
	{
		base.OnChangeDuration ();
	}

}
