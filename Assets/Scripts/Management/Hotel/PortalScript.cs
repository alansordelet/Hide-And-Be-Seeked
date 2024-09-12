using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public Transform playerPos;
    public Transform[] receivers;
    public AudioSource tp;
    private bool playerIsOverlapping = false;

    void FixedUpdate()
    {
        if (playerIsOverlapping)
        {
            var playerBehaviour = playerPos.GetComponent<PlayerBehaviour>();

            Transform selectedReceiver = receivers[Random.Range(0, receivers.Length)];

            playerPos.position = selectedReceiver.position;
            playerBehaviour.SetRotationValues(selectedReceiver.rotation.eulerAngles.x, selectedReceiver.rotation.eulerAngles.y);

            playerIsOverlapping = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tp.Play();
            playerIsOverlapping = true;
        }
    }
}
