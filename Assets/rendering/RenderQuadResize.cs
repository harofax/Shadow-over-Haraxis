using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderQuadResize : MonoBehaviour
{
    public Transform quad;
    public RenderTexture renderTexture;
    public int render_height = 180;

    private Camera orthoCam;

    private void Awake()
    {
        orthoCam = GetComponent<Camera>();

        InitRender();
    }
    

    public void InitRender()
    {
        float aspect = (float) Screen.width / Screen.height;
        
        float rend_width = Mathf.Round(render_height * aspect);

        renderTexture.width = (int) rend_width;
        renderTexture.height = render_height;

        int cam_height = Screen.height;

        orthoCam.orthographicSize = cam_height / 2.0f;
        
        var quad_height = orthoCam.orthographicSize * 2.0f;
        var quad_width = quad_height * Screen.width / Screen.height;

        quad.localScale = new Vector3(quad_width, quad_height, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
