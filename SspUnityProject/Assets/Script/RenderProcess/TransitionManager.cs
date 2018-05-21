using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransitionManager : MonoBehaviour {

	public TransitionStage Stage;

	public TransitionRenderPrecess TransitionRenderer;

	public UnityEvent OnTransitionStart;

	public UnityEvent OnTransitionEnd;

	public void Transition ()
	{
		
	}
}
