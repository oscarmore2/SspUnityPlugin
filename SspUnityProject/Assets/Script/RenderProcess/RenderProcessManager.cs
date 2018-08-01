using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderProcessManager: MonoBehaviour {

    List<CommonRenderProcess> branchProcessPath = new List<CommonRenderProcess>();
    int currentProcessIndex = -1;

	public EffectRenderProcess EffectProcess;

	public PreRenderProcess EarlyProcess;

	public TransitionRenderPrecess TransitionProcess;

	public PostRenderProcess PostProcess;

	public void RegisterProcess(int position, CommonRenderProcess process)
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
        EffectProcess = RenderProcessFactory.CreateProcess<EffectRenderProcess>(this.gameObject);
        EarlyProcess = RenderProcessFactory.CreateProcess<PreRenderProcess>(this.gameObject);
        TransitionProcess = RenderProcessFactory.CreateProcess<TransitionRenderPrecess>(this.gameObject);
        PostProcess = RenderProcessFactory.CreateProcess<PostRenderProcess>(this.gameObject);
    }

    void OnDestroy()
    {
        StopCoroutine("OnRender");
        DestoryRenderProcess();
    }

    void DestoryRenderProcess()
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
}

public class RenderProcessFactory
{
    public static T CreateProcess<T>(GameObject processManager) where T : CommonRenderProcess
    {
        T process = null;
        GameObject obj = new GameObject ();
        obj.transform.parent = processManager.transform;
        obj.transform.localPosition = Vector3.zero;
        if (typeof(T) == typeof(EffectRenderProcess))
        {
            obj.name = "EffectRenderProcess";
            obj.transform.position += Vector3.up * 1000;
            CommonRenderProcess temp = obj.AddComponent<EffectRenderProcess>();
            process = (T)temp;
        }
        else if (typeof(T) == typeof(PreRenderProcess))
        {
            obj.name = "PreRenderProcess";
            obj.transform.position += Vector3.up * 2000;
            CommonRenderProcess temp = obj.AddComponent<PreRenderProcess>();
            process = (T)temp;
        }
        else if (typeof(T) == typeof(TransitionRenderPrecess))
        {
            obj.name = "TransitionRenderPrecess";
            obj.transform.position += Vector3.up * 3000;
            CommonRenderProcess temp = obj.AddComponent<TransitionRenderPrecess>();
            process = (T)temp;
        }
        else if (typeof(T) == typeof(PostRenderProcess))
        {
            obj.name = "PostRenderProcess";
            obj.transform.position += Vector3.up * 4000;
            CommonRenderProcess temp = obj.AddComponent<PostRenderProcess>();
            process = (T)temp;
        }
        return process;
    }

    public static RenderProcessManager CreateProcessManager(Vector3 offset)
    {
        GameObject obj = new GameObject("ProcessManager");
        obj.transform.position = offset + Vector3.left * 3000;
        RenderProcessManager process = obj.AddComponent<RenderProcessManager>();
        return process;
    }
}
