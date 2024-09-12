using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightController : MonoBehaviour
{
    public float panSpeed = 30.0f;
    public Transform player;
    private float currentRotation = 0.0f;
    [SerializeField]
    private float currentLookAngle = 45f;
    [SerializeField]
    private Transform priest;
    [SerializeField]
    private Animator priestAnimation;
    [SerializeField] private float angle = 10f;
    [SerializeField] private float seperation = 3f;
    [SerializeField] PriestVisibilityCheck priestVision;

    public float addAngleX = 5f;
    public float addAngleY = 5f;

    public bool playerInLightRay = false;
    public Light thisLight;
    public float maxDistance = 20f;
    private Vector3 direction;

    void Update()
    {

        int layer = 1 << LayerMask.NameToLayer("Player");
        int wallsLayer = 1 << LayerMask.NameToLayer("Walls");
        playerInLightRay = false;

        for (float angleX = (-angle + addAngleX); angleX <= (angle + addAngleX); angleX += seperation)
        {
            for (float angleY = (-angle + addAngleY); angleY <= (angle + addAngleY); angleY += seperation)
            {
                Quaternion rotation = Quaternion.AngleAxis(angleY, Vector3.up) * Quaternion.AngleAxis(angleX, Vector3.right);
                Vector3 coneDirection = rotation * direction;
                Ray ray = new Ray(thisLight.transform.position, coneDirection);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, maxDistance, wallsLayer | layer))
                {
                    if (hit.collider != null)
                    {
                        // If the hit collider belongs to the walls layer, stop processing this ray
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
                        {
                            break;
                        }
                        // If the hit collider belongs to the player layer, set playerInView to true
                        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            playerInLightRay = true;
                        }
                    }
                }

            }
        }


        if (playerInLightRay)
        {
            priestAnimation.SetBool("PlayerInView", true);
            Vector3 playerPosition = player.position;
            priest.transform.LookAt(playerPosition);
            priestVision.spotlightView = true;
            priestVision.playerInView = true;
            timeInViewCounter = 0.0f;
            FearManager.instance.ModifyFear(50f * Time.deltaTime);
        }
        else
        {
            if (!isResetting)
            {
                timeInViewCounter += Time.deltaTime;
                if (timeInViewCounter >= timeInView)
                {
                    // Resetting after a certain duration
                    isResetting = true;
                    StartCoroutine(ResetPlayerInView());
                }
            }
            priestVision.spotlightView = false;
            priestAnimation.SetBool("PlayerInView", false);
            currentRotation += panSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);

            Quaternion newRotation = Quaternion.Euler(currentLookAngle, currentRotation, 0);
            transform.rotation = newRotation;
            priest.transform.rotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);
            direction = thisLight.transform.forward;
        }
    }

    IEnumerator ResetPlayerInView()
    {
        // Delay before resetting playerInView
        yield return new WaitForSeconds(1.0f); // Adjust as needed
        priestVision.playerInView = false;
        isResetting = false;
    }
    [SerializeField] float timeInView = 2.0f; // Adjust the duration as needed
    float timeInViewCounter = 0.0f;
    bool isResetting = false;

    private void OnDrawGizmos()
    {
        for (float angleX = (-angle + addAngleX); angleX <= (angle + addAngleX); angleX += seperation)
        {
            for (float angleY = (-angle + addAngleY); angleY <= (angle + addAngleY); angleY += seperation)
            {
                Quaternion rotation = Quaternion.AngleAxis(angleX, Vector3.up) * Quaternion.AngleAxis(angleY, Vector3.right);
                Vector3 coneDirection = rotation * direction;
                Vector3 coneTip = thisLight.transform.position + coneDirection * maxDistance;
                Gizmos.DrawLine(thisLight.transform.position, coneTip);

            }
        }
    }
}
