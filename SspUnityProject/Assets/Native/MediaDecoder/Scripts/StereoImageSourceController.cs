﻿//========= Copyright 2015-2018, WhaleyVR. All rights reserved. ===========
//========= Written by whitespeed =========

using UnityEngine;
using System.Collections;

namespace UnityPlugin.Decoder
{
	public class StereoImageSourceController : ImageSourceController {
		public StereoProperty stereoProperty;

		protected override void Start() {
			LOG_TAG = "[StereoImageSourceController]";
			base.Start();
			StereoHandler.SetStereoPair(stereoProperty, GetComponent<MeshRenderer>().material);
		}

		protected override void adaptResolution() {
			int width = texture.width;
			int height = texture.height;
			if (stereoProperty.stereoType == StereoHandler.StereoType.TOP_DOWN) {
				height /= 2;
			} else {
				width /= 2;
			}
			Vector3 adaptReso = oriScale;
			adaptReso.x *= ((float) width / height);
			transform.localScale = adaptReso;
		}
	}
}