using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Resource;

public class GroupTraveller : Singleton<GroupTraveller> {

    public event Action ResourceChange;


    public void OnTravel(ResourceGroup group)
    {
        group.SortGroup();
        if (group.ActivateState[0] == true)
        {
            int layer = 0;
            for (int i = 0; i < group.ResourceRefs.Count; i++)
            {
                var resource = group.ResourceRefs[i].Resources;
                var rend = RendererContainor.Instance.PVWRender(resource, group.IsAfterTransition);
                ResourceRenderTarget target = null;
                if (group.IsAfterTransition)
                {
                    target = ResourceRenderTarget.Create(ResourceDisplayList.Instance.PVWPostRender.gameObject, layer);
                }
                else {
                    target = ResourceRenderTarget.Create(ResourceDisplayList.Instance.PVWPreRender.gameObject, layer);
                }
                if (resource.GetType() == ResourceType.Image)
                {
					((ImageRenderer)rend).AttachRenderTarget<UnityEngine.UI.RawImage>(target.gameObject, group);
                }
                else if (resource.GetType() == ResourceType.Text)
                {
					((TextRenderer)rend).AttachRenderTarget<UnityEngine.UI.Text>(target.gameObject, group);
                }
                layer++;
            }
        }
        else if (group.ActivateState[1] == true)
        {
            int layer = 0;
            for (int j = 0; j < group.ResourceRefs.Count; j++)
            {
                var resource = group.ResourceRefs[j].Resources;
                var rend = RendererContainor.Instance.PGMRender(resource, group.IsAfterTransition);
                ResourceRenderTarget target = null;
                if (group.IsAfterTransition)
                {
                    target = ResourceRenderTarget.Create(ResourceDisplayList.Instance.PGMPostRender.gameObject, layer);
                }
                else {
                    target = ResourceRenderTarget.Create(ResourceDisplayList.Instance.PGMPreRender.gameObject, layer);
                }
                if (resource.GetType() == ResourceType.Image)
                {
					((ImageRenderer)rend).AttachRenderTarget<UnityEngine.UI.RawImage>(target.gameObject, group);
                }
                else if (resource.GetType() == ResourceType.Text)
                {
					((TextRenderer)rend).AttachRenderTarget<UnityEngine.UI.Text>(target.gameObject, group);
                }
                layer++;
            }
        }
        else if (group.ActivateState[2] == true)
        {
            //TODO : add logic in this toggle.
        }
        else
        {
            for (int j = 0; j < group.ResourceRefs.Count; j++)
            {
                var resource = group.ResourceRefs[j].Resources;
                var rend = RendererContainor.Instance[resource.GUID + "@PGM"];
                if (rend != null)
                {
                    if (group.IsAfterTransition)
                        ResourceDisplayList.Instance.ReFlushComponent(rend, ResourceDisplayList.Instance.PGMPostRender);
                    else
                        ResourceDisplayList.Instance.ReFlushComponent(rend, ResourceDisplayList.Instance.PGMPreRender);
                }

                rend = RendererContainor.Instance[resource.GUID + "@PVW"];
                if (rend != null)
                {
                    if (group.IsAfterTransition)
                        ResourceDisplayList.Instance.ReFlushComponent(rend, ResourceDisplayList.Instance.PVWPostRender);
                    else
                        ResourceDisplayList.Instance.ReFlushComponent(rend, ResourceDisplayList.Instance.PVWPreRender);
                }
            }
        }

        if (ResourceChange != null)
            ResourceChange();
    }

    void SetupRenderer(IResource resource, ResourceGroup group, int layer)
    {
        
    }

    public void OnInit()
    {
		var list = ResourceGroupList.Instance.Sort (true);
		for (int i = 0; i < list.Count; i++)
        {
			var ResGroup = list[i];
            OnTravel(ResGroup);
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

