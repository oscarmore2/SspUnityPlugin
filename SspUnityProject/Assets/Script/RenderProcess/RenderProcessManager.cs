using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderProcessManager : MonoBehaviour {

    List<IRenderProcess> branchProcessPath = new List<IRenderProcess>();
    int currentProcessIndex = -1;

	public EffectRenderProcess EffectProcess;

	public PreRenderProcess EarlyProcess;

	public TransitionRenderPrecess TransitionProcess;

	public PostRenderProcess PostProcess;

	public void RegisterProcess(int position, IRenderProcess process)
    {
		if (position > branchProcessPath.Count + 1) {
			branchProcessPath.Add (process);
		} else if (position < branchProcessPath.Count - 1) {
			branchProcessPath.Insert (0, process);
		} else {
			branchProcessPath.Insert (position, process);
		}
    }

	void CreateBseicRenderProcess()
	{
        EffectProcess = new EffectRenderProcess();
        EarlyProcess = new PreRenderProcess();
        TransitionProcess = new TransitionRenderPrecess();
        PostProcess = new PostRenderProcess();
    }

    public void StartRender(Texture Input)
    {
        EffectProcess.SetupProcess(Input);

        EarlyProcess.SetupProcess(EffectProcess.ProcessResult);

        if (branchProcessPath.Count > 0)
        {
            for (int i = 0; i < branchProcessPath.Count; i++)
            {
                if (i == 0)
                {
                    branchProcessPath[i].SetupProcess(EarlyProcess.ProcessResult);
                }
                else
                {
                    branchProcessPath[i].SetupProcess(branchProcessPath[i - 1].ProcessResult);
                }
            }
            TransitionProcess.SetupProcess(branchProcessPath[branchProcessPath.Count - 1].ProcessResult);
        }
        else
        {
            TransitionProcess.SetupProcess(EarlyProcess.ProcessResult);
        }

        PostProcess.SetupProcess(TransitionProcess.ProcessResult);

        StartCoroutine(OnRender());
    }

	IEnumerator OnRender()
	{
        while (true)
        {
            yield return EffectProcess.DoRenderProcess();

            yield return EarlyProcess.DoRenderProcess();

            for (int i = 0; i < branchProcessPath.Count; i++)
            {
                yield return branchProcessPath[i].DoRenderProcess();
            }

            yield return TransitionProcess.DoRenderProcess();

            yield return PostProcess.DoRenderProcess();
        }
    }
}
