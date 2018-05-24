using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderProcessManager : Singleton<RenderProcessManager> {

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

	public void CreateBseicRenderProcess()
	{
        EffectProcess = RenderProcessFactory.CreateProcess<EffectRenderProcess>();
        EarlyProcess = RenderProcessFactory.CreateProcess<PreRenderProcess>();
        TransitionProcess = RenderProcessFactory.CreateProcess<TransitionRenderPrecess>();
        PostProcess = RenderProcessFactory.CreateProcess<PostRenderProcess>();
    }

    public void DestoryRenderProcess()
    {
        EffectProcess = null;
        EarlyProcess = null;
        TransitionProcess = null;
        PostProcess = null;
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
            EffectProcess.DoRenderProcess();

            EarlyProcess.DoRenderProcess();

            for (int i = 0; i < branchProcessPath.Count; i++)
            {
                branchProcessPath[i].DoRenderProcess();
            }

            TransitionProcess.DoRenderProcess();

            PostProcess.DoRenderProcess();

            yield return new WaitForEndOfFrame();
        }
    }

    public override void OnInitialize()
    {
        CreateBseicRenderProcess();
    }

    public override void OnUninitialize()
    {
        DestoryRenderProcess();
    }
}

public class RenderProcessFactory
{
    public static T CreateProcess<T>() where T : IRenderProcess
    {
        T process = null;
        GameObject obj = new GameObject ("RenderProcess");
        obj.transform.position = Vector3.left * 3000;
        if (typeof(T) == typeof(EffectRenderProcess))
        {
            obj.name = "EffectRenderProcess";
            obj.transform.position += Vector3.up * 1000;
            IRenderProcess temp = obj.AddComponent<EffectRenderProcess>();
            process = (T)temp;
        }
        else if (typeof(T) == typeof(PreRenderProcess))
        {
            obj.name = "PreRenderProcess";
            obj.transform.position += Vector3.up * 2000;
            IRenderProcess temp = obj.AddComponent<PreRenderProcess>();
            process = (T)temp;
        }
        else if (typeof(T) == typeof(TransitionRenderPrecess))
        {
            obj.name = "TransitionRenderPrecess";
            obj.transform.position += Vector3.up * 3000;
            IRenderProcess temp = obj.AddComponent<TransitionRenderPrecess>();
            process = (T)temp;
        }
        else if (typeof(T) == typeof(PostRenderProcess))
        {
            obj.name = "PostRenderProcess";
            obj.transform.position += Vector3.up * 4000;
            IRenderProcess temp = obj.AddComponent<PostRenderProcess>();
            process = (T)temp;
        }
        return process;
    }
}
