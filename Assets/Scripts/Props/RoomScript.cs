using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine.Examples;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    private bool activateDoorSlam = false;

    [SerializeField]
    private GameObject door;

    [SerializeField]
    private GameObject priest;

    public AudioSource doorSound;
    public GameObject turnaround;
    private float timer = 0;
    bool uuu = false;
    private void Update()
    {
        if (activateDoorSlam)
        timer += Time.deltaTime;

        if (timer > 4f && !uuu)
        {
            FadeMechanic.instance.StartFadeIn();
            timer = 0;
            uuu = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!activateDoorSlam)
            {
                turnaround.SetActive(true);
                GameManager.instance.AtRoom = true;
                priest.SetActive(true);
                StartCoroutine(RotateDoor());
                activateDoorSlam = true;
            }

            
        }
    }

    private IEnumerator RotateDoor()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second

        Quaternion initialRotation = door.transform.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, -90, 0); // Rotate 90 degrees around the Y axis

        float elapsedTime = 0f;
        float duration = 1f; // Duration of the rotation

        while (elapsedTime < duration)
        {
            door.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
            if (elapsedTime / duration >= 0.9f && !doorSound.isPlaying)
            {
                doorSound.Play();
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.transform.rotation = targetRotation; // Ensure final rotation is set to the target rotation
    }
}
