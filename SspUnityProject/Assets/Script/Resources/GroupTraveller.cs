using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Resource;

public class GroupTraveller : Singleton<GroupTraveller> {

    public event Action ResourceChange;


    public void OnTravel()
    {
        for (int i = 0; i < ResourceGroupList.Instance.Count; i++)
        {
            var ResGroup = ResourceGroupList.Instance[i];
            if (ResGroup.ActivateState[0] == true)
            {
                for (int j = 0; i < ResGroup.ResourceRefs.Count; j++)
                {
                    RendererContainor.Instance.PVWRender(ResGroup.ResourceRefs[j].Resources, ResGroup.IsAfterTransition);
                }
            }
            else if (ResGroup.ActivateState[1] == true)
            {
                for (int j = 0; i < ResGroup.ResourceRefs.Count; j++)
                {
                    RendererContainor.Instance.PGMRender(ResGroup.ResourceRefs[j].Resources, ResGroup.IsAfterTransition);
                }
            }
            else if (ResGroup.ActivateState[2] == true)
            {
                //TODO : add login in this toggle.
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

