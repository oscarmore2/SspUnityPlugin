﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCreater : MonoBehaviour {


	[SerializeField]
	protected float duration;


	[SerializeField]
	protected UnityEngine.UI.InputField durationInput;

	public Transition transition{ get; protected set;}

	public void OnChangeDuration()
	{
		float _duration = 0f;
		float.TryParse (durationInput.text, out _duration);
		duration = _duration;
		transition.duration = _duration;
	}
}
