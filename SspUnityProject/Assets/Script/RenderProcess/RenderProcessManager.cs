using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderProcessManager : MonoBehaviour {

    List<IRenderProcess> ProcessPath = new List<IRenderProcess>();
    int currentProcessIndex = -1;

	public EffectRenderProcess EffectProcess;

	public PreRenderProcess EarlyProcess;

	public TransitionRenderPrecess TransitionProcess;

	public PostRenderProcess PostProcess;

	public void RegisterProcess(int position, IRenderProcess process)
    {
		if (position > ProcessPath.Count + 1) {
			ProcessPath.Add (process);
		} else if (position < ProcessPath.Count - 1) {
			ProcessPath.Insert (0, process);
		} else {
			ProcessPath.Insert (position, process);
		}
    }

    public IRenderProcess GetCurRenderProcess()
    {
        return ProcessPath[currentProcessIndex];
    }
		
	void Awake()
	{
		
	}

	void CreateRenderProcess()
	{
		
	}

	void Update()
	{
		EffectProcess.DoRenderProcess ();

		EarlyProcess.DoRenderProcess ();

		TransitionProcess.DoRenderProcess ();

		PostProcess.DoRenderProcess ();

		foreach (var p in ProcessPath) {
			p.DoRenderProcess ();
		}
	}
}
