using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batteries : MonoBehaviour
{
    public Flashlight flashLight;
    public GameObject[] objectsToDestroy;
    public PlayerBehaviour playerBehaviour;
    public float interactionDistance = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (var obj in objectsToDestroy)
            {               
                float distance = Vector3.Distance(playerBehaviour.gameObject.transform.position, obj.transform.position);               
                if (distance <= interactionDistance)
                {
                    Destroy(obj);
                    flashLight.AddBatterPercentage(30f);     
                }
            }
        }
    }
}
