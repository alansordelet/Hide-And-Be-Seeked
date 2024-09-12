using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class KeyBarrier : MonoBehaviour
{
    [SerializeField] GameObject grabKeySprite;
    [SerializeField] CinemachineVirtualCamera cam;
    RaycastHit hit;
    [SerializeField] LayerMask keyLayer;
    void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4f, keyLayer))
        {
            grabKeySprite.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                grabKeySprite.SetActive(false);
                GameManager.instance.GotBarrierKey = true;
                gameObject.SetActive(false);    
            }
        }
        else
        {
            grabKeySprite.SetActive(false);
        }
    }

   
}
