using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
public class PriestVisibilityCheck : MonoBehaviour
{

    public Transform priestTransform;
    public bool priestIsInCameraView;
    public float coneAngle = 20f;
    public float coneLength = 30f;
    public Animator priestAnimator;
    public bool playerInView = false;
    public float timerChase = 0f;
    public float addAngleX = 40f;
    public float addAngleY = 5f;
    public float Separation = 5f;
    public bool spotlightView = false;
    int layer = 0;

    void Start()
    {
        playerInView = false;
    }
    void Update()
    {

        CheckForPlayer();
        // PriestChasing();
    }
    float RangeView = 90;
    void CheckForPlayer()
    {
        layer = 1 << LayerMask.NameToLayer("Player");
        int wallsLayer = 1 << LayerMask.NameToLayer("Walls");
        Vector3 direction = transform.forward;
        float maxConeLength = coneLength;

        if (!spotlightView)
        playerInView = false;

        for (float angleX = (-coneAngle - addAngleX); angleX <= (coneAngle + addAngleX); angleX += Separation)
        {
            for (float angleY = (-coneAngle + addAngleY); angleY <= (coneAngle - addAngleY); angleY += Separation)
            {
                Quaternion rotation = Quaternion.AngleAxis(angleX, Vector3.up) * Quaternion.AngleAxis(angleY, Vector3.right);
                Vector3 coneDirection = rotation * direction;
                RaycastHit hit;
                Ray ray = new Ray(transform.position, coneDirection);

                if (Physics.Raycast(ray, out hit, coneLength, wallsLayer | layer))
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
                        {
                            break;
                        }
                        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            playerInView = true;
                        }
                    }
                }
            }
        }


    }



    private void PriestChasing()
    {
        if (playerInView)
        {
            timerChase += Time.deltaTime;
            // priestAnimator.SetBool("isSprinting", true);         
        }
        if (!playerInView)
            timerChase = 0;

        //if (timerChase > 2 && )
        //{
        //    timerChase = 0;
        //}
    }

    //private void OnDrawGizmos()
    //{
    //    layer = 1 << LayerMask.NameToLayer("Player");
    //    Vector3 direction = transform.forward;

    //    for (float angleX = (-coneAngle - addAngleX); angleX <= (coneAngle + addAngleX); angleX += Separation)
    //    {
    //        for (float angleY = (-coneAngle + addAngleY); angleY <= (coneAngle - addAngleY); angleY += Separation)
    //        {
    //            Quaternion rotation = Quaternion.AngleAxis(angleX, Vector3.up) * Quaternion.AngleAxis(angleY, Vector3.right);
    //            Vector3 coneDirection = rotation * direction;
    //            Vector3 coneTip = transform.position + coneDirection * coneLength;
    //            Ray ray = new Ray(transform.position, coneDirection);

    //            if (Physics.Raycast(ray, out RaycastHit hit, coneLength, layer))
    //            {

    //                Gizmos.color = Color.green;
    //            }
    //            else
    //            {
    //                Gizmos.color = Color.red;
    //            }
    //            Gizmos.DrawLine(transform.position, coneTip);
    //        }
    //    }
    //}
}
