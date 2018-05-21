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

    public void RegisterProcess(int position)
    {

    }

    public IRenderProcess GetCurRenderProcess()
    {
        return ProcessPath[currentProcessIndex];
    }
}
