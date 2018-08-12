using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Resource;

public class GroupTraveller : Singleton<GroupTraveller> {

    public event Action ResourceChange;


    public void OnTravel(ResourceGroup group, bool[] activateState)
    {

    }

    public void OnInit()
    {
		ResourceGroupList.Instance.Sort ();
        for (int i = 0; i < ResourceGroupList.Instance.Count; i++)
        {
            var ResGroup = ResourceGroupList.Instance[i];
			ResGroup.SortGroup ();
            if (ResGroup.ActivateState[0] == true)
            {
				int layer = 0;
                for (int j = 0; i < ResGroup.ResourceRefs.Count; j++)
                {
					var resource = ResGroup.ResourceRefs [j].Resources;
					var rend = RendererContainor.Instance.PVWRender(resource, ResGroup.IsAfterTransition);
					ResourceRenderTarget target = null;
					if (ResGroup.IsAfterTransition) {
						target = ResourceRenderTarget.Create (ResourceDisplayList.Instance.PVWPostRenderPipeLineCamera.gameObject, layer);
					} else {
						target = ResourceRenderTarget.Create (ResourceDisplayList.Instance.PVWPreRenderPipeLineCamera.gameObject, layer);
					}
					if (resource.GetType () == ResourceType.Image) {
						rend.AttachRenderTarget<UnityEngine.UI.RawImage> (target.gameObject);
					} else if (resource.GetType () == ResourceType.Text) {
						rend.AttachRenderTarget<UnityEngine.UI.Text> (target.gameObject);
					}
					layer++;
                }
            }
            else if (ResGroup.ActivateState[1] == true)
            {
				int layer = 0;
                for (int j = 0; i < ResGroup.ResourceRefs.Count; j++)
                {
					var resource = ResGroup.ResourceRefs [j].Resources;
					var rend = RendererContainor.Instance.PGMRender(resource, ResGroup.IsAfterTransition);
					ResourceRenderTarget target = null;
					if (ResGroup.IsAfterTransition) {
						target = ResourceRenderTarget.Create (ResourceDisplayList.Instance.PGMPostRenderPipeLineCamera.gameObject, layer);
					} else {
						target = ResourceRenderTarget.Create (ResourceDisplayList.Instance.PGMPreRenderPipeLineCamera.gameObject, layer);
					}
					if (resource.GetType () == ResourceType.Image) {
						rend.AttachRenderTarget<UnityEngine.UI.RawImage> (target.gameObject);
					} else if (resource.GetType () == ResourceType.Text) {
						rend.AttachRenderTarget<UnityEngine.UI.Text> (target.gameObject);
					}
					layer++;
                }
            }
            else if (ResGroup.ActivateState[2] == true)
            {
                //TODO : add logic in this toggle.
            }
            else
            {
                for (int j = 0; i < ResGroup.ResourceRefs.Count; j++)
                {
                    
                }
            }

            if (ResourceChange != null)
                ResourceChange();
        }
    }

    public override void OnInitialize()
    {
        ResourceGroupList.Instance.OnResourceGroupChange += OnTravel;
    }

    public override void OnUninitialize()
    {
        ResourceGroupList.Instance.OnResourceGroupChange -= OnTravel;
    }
}

