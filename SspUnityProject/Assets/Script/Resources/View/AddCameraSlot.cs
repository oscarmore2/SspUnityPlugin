using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCameraSlot : MonoBehaviour {

	[SerializeField]
	VCamView view;

	// Use this for initialization
	void Start () {
		if (view == null) {
			view = transform.parent.GetComponent<VCamView> ();
		}
		gameObject.SetActive (false);
	}

	public void Active()
	{
		if (view.BufferTexture == null) {
			gameObject.SetActive (true);
		}
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
