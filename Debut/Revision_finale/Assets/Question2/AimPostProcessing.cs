﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class AimPostProcessing : MonoBehaviour {
    private Material mat;
    void Awake()
    {
        mat = new Material(Shader.Find("Hidden/ShaderQ2"));
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
