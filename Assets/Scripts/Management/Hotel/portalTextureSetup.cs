using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class portalTextureSetup : MonoBehaviour
{
    public Camera VirtualCameraB;
    public Camera VirtualCameraC;
    public Material materialCam;
    public Material materialCamC;

    // Start is called before the first frame update
    void Start()
    {

        if (VirtualCameraC.targetTexture != null)
        {
            VirtualCameraC.targetTexture.Release();
        }
        VirtualCameraC.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        materialCamC.mainTexture = VirtualCameraC.targetTexture;

        if (VirtualCameraB.targetTexture != null) 
        {
            VirtualCameraB.targetTexture.Release();
        }
        VirtualCameraB.targetTexture = new RenderTexture( Screen.width, Screen.height, 24 );
        materialCam.mainTexture = VirtualCameraB.targetTexture;
    }

}
