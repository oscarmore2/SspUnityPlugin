﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resource;

public class ResTest : MonoBehaviour {

    [SerializeField]
    ResourcesManager manager;

	// Use this for initialization
	void Start () {
        manager.Init();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
