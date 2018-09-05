using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaEreaseTransitionCreater : TransitionCreater {

	[SerializeField]
	string key;

	[SerializeField]
	float startValue;

	[SerializeField]
	float endValue;

	[SerializeField]
	Texture2D Mask;

	[SerializeField]
	string textureKey;

	// Use this for initialization
	void Start () {
		transition = new Transition ();
		transition.duration = duration;
		float step = Mathf.Abs(endValue - startValue) / duration * Time.deltaTime;
		transition.ValueField.Add (new Transition.ShaderParams<float> (key,startValue, endValue, step));
		transition.TextureField.Add (new Transition.ShaderParams<Texture> (textureKey, Mask, Mask));
		transition.SetShader(shaderName);
	}

	public override void OnChangeDuration ()
	{
		base.OnChangeDuration ();
        float step = Mathf.Abs(endValue - startValue) / duration * Time.deltaTime;
        transition.ValueField[0]= new Transition.ShaderParams<float>(key, startValue, endValue, step);
    }

}
