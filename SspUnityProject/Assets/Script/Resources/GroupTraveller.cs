using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Resource;

public class GroupTraveller {

    public event Action PreviewListChange;

    public GroupTraveller()
    {
        ResourceDisplayList.Create();
        ResourceGroupList.Instance.OnResourceGroupChange += OnTravel;
    }


    public void OnTravel()
    {
        for (int i = 0; i < ResourceGroupList.Instance.Count; i++)
        {
            var ResGroup = ResourceGroupList.Instance[i];
            if (ResGroup.ActivateState[0] == true)
            {
                if (ResGroup.IsAfterTransition)
                {
                    ResourceDisplayList.Instance.PreviewPostRenderList.Clear();
                    for (int j = 0; i < ResGroup.ResourceRefs.Count; j++)
                    {
                        ResourceDisplayList.Instance.PreviewPostRenderList.Add(ResGroup.ResourceRefs[j].Resources);
                    }
                }
                else
                {
                    ResourceDisplayList.Instance.PreviewPreRenderList.Clear();
                    for (int j = 0; i < ResGroup.ResourceRefs.Count; j++)
                    {
                        ResourceDisplayList.Instance.PreviewPostRenderList.Add(ResGroup.ResourceRefs[j].Resources);
                    }
                }
                
            }
            else if (ResGroup.ActivateState[1] == true)
            {
                for (int j = 0; i < ResGroup.ResourceRefs.Count; j++)
                {

                }
            }
            else if (ResGroup.ActivateState[2] == true)
            {
                for (int j = 0; i < ResGroup.ResourceRefs.Count; j++)
                {

                }
            }
            
        }
    }
}

