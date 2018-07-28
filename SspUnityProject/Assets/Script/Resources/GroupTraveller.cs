using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Resource;

public class GroupTraveller {

    public GroupTraveller()
    {
        ResourceGroupList.Instance.OnResourceGroupChange += OnTravel;
    }


    public void OnTravel()
    {

    }
}
