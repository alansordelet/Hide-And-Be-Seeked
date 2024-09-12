using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] GameObject Bar;
    private bool playerIsInTriggerZone = false;
    float currentRotation;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.AtActivatedBarrier == true || Input.GetKeyDown(KeyCode.O))
        {
            float rotationThisFrame = 100 * Time.deltaTime;
            if (currentRotation - rotationThisFrame < -90f)
            {
                rotationThisFrame = currentRotation + 90f;
            }
            Bar.transform.Rotate(new Vector3(-rotationThisFrame, 0, 0));
            currentRotation -= rotationThisFrame;
            if (currentRotation < -90f) currentRotation = -90f;
        }

        if (playerIsInTriggerZone)
        {
            playerIsInTriggerZone = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !playerIsInTriggerZone && !GameManager.instance.AtActivatedBarrier)
        {
            GameManager.instance.AtDeactivatedBarrier = true;
            playerIsInTriggerZone = true;
        }

        if (other.gameObject.CompareTag("Player") && GameManager.instance.GotBarrierKey)
        {
            GameManager.instance.AtActivatedBarrier = true;
            playerIsInTriggerZone = true;
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {           
    //        GameManager.instance.AtDeactivatedBarrier = false;
    //    }
    //}
}
