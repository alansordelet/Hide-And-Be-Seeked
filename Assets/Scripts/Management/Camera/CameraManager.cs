using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; }
    public Collider FirstTransition;
    public Collider SecondTransition;
    public Transform playerPos;
    public CinemachineVirtualCamera[] Cameras; 
    public Transform lookAtTarget; 
    public bool inFirstTransition = false;
    private bool inSecondTransition = false;
    private bool usingFirstCamera = true;
    private bool usingSecondCamera = true;


    public static CameraManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        if (FirstTransition.bounds.Contains(playerPos.position))
        {
            if (!inFirstTransition)
            {
                inFirstTransition = true;

                if (usingFirstCamera)
                {
                    Cameras[0].gameObject.SetActive(false);
                    Cameras[1].gameObject.SetActive(true);
                    usingFirstCamera = false;
                }
                else
                {
                    Cameras[0].gameObject.SetActive(true);
                    Cameras[1].gameObject.SetActive(false);
                    usingFirstCamera = true;
                }
            }
        }
        else if (SecondTransition.bounds.Contains(playerPos.position))
        {
            if (!inSecondTransition)
            {
                inSecondTransition = true;

                if (usingSecondCamera)
                {                  
                    Cameras[1].gameObject.SetActive(false);
                    Cameras[0].gameObject.SetActive(true);
                    usingSecondCamera = false;
                }
                else
                {                 
                    Cameras[1].gameObject.SetActive(true);
                    Cameras[0].gameObject.SetActive(false);
                    usingSecondCamera = true;
                }
            }
        }
       
    }


}
