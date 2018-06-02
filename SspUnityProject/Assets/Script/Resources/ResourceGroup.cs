using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGroup : MonoBehaviour {


    public string Name;
    public int Priority;
    public bool[] VCamBiding = new bool[] { false, false, false, false, false, false, false, false, false };
    public List<IResourceRef> ResourceRefs = new List<IResourceRef>();
    public bool[] ActivateState = new bool[] { false, false, false, false, false };
    public bool IsAfterTransition;

    public float Scale;
    public Vector2 Postition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
