using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransitionManager : Singleton<TransitionManager> {

	public Material Stage;

	public System.Action<Material> OnTransitionStart;

	public System.Action OnTransitionEnd;


	float count = 0f;

	public void Transition (Transition transition)
	{
		count = 0f;
        if (transition != null)
		    Stage = new Material (transition.TransitionSahder);

		StartCoroutine (OnTransition (transition));
	}

	IEnumerator OnTransition(Transition transition)
	{
        yield return new WaitForEndOfFrame();
        if (transition != null)
        {
            List<float> tempValue = new List<float>();
            for (int i = 0; i < transition.ValueField.Count; i++)
            {
                var value = transition.ValueField[i];
                Stage.SetFloat(value.fieldName, value.StartValue);
                tempValue.Add(0f);
            }

            for (int i = 0; i < transition.TextureField.Count; i++)
            {
                Stage.SetTexture(transition.TextureField[i].fieldName, transition.TextureField[i].EndValue);
            }

            if (OnTransitionStart.GetInvocationList().Length > 0)
                OnTransitionStart(Stage);

            while (true)
            {
                yield return new WaitForEndOfFrame();
                count += Time.deltaTime;
                if (count > transition.duration)
                {
                    break;
                }

                for (int i = 0; i < transition.ValueField.Count; i++)
                {
                    Transition.ShaderParams<float> value = transition.ValueField[i];
                    tempValue[i] += value.Step * Time.deltaTime;
                    Stage.SetFloat(value.fieldName, tempValue[i]);
                    Debug.Log(value.fieldName);
                    Debug.Log(tempValue[i]);
                }
            }

            for (int i = 0; i < transition.ValueField.Count; i++)
            {
                var value = transition.ValueField[i];
                Stage.SetFloat(value.fieldName, value.EndValue);
            }
        }
        else {
            if (OnTransitionStart.GetInvocationList().Length > 0)
                OnTransitionStart(null);
        }

		if (OnTransitionEnd.GetInvocationList().Length > 0)
			OnTransitionEnd ();
	}

	public override void OnInitialize ()
	{
		
	}

	public override void OnUninitialize ()
	{
		StopCoroutine ("OnTransition");
	}
}
