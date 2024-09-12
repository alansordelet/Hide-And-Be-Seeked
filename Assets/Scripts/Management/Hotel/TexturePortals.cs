using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextures : MonoBehaviour
{
   
    public Camera VirtualCamA;
    public Camera VirtualCamB;

    
    public Material materialCamA;
    public Material materialCamB;
   
   
   

    // Start is called before the first frame update
    void Start()
    {

        if (VirtualCamA.targetTexture != null)
        {
            VirtualCamA.targetTexture.Release();
        }
        VirtualCamA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        materialCamA.mainTexture = VirtualCamA.targetTexture;

        if (VirtualCamB.targetTexture != null)
        {
            VirtualCamB.targetTexture.Release();
        }
        VirtualCamB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        materialCamB.mainTexture = VirtualCamB.targetTexture;
    }
}
