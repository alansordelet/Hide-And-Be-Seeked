using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PortalCam : MonoBehaviour
{

    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform portalPos;
    [SerializeField] private Transform otherPortalPos;
    [SerializeField] private float whichSideX_Rotation;
    [SerializeField] private float whichSideZ_Rotation;
    [SerializeField] private float whichSideX_Position;
    [SerializeField] private float whichSideZ_Position;
    [SerializeField] public bool left;
    [SerializeField] public bool right;
    [SerializeField] public bool opposite;
    private void Start()
    {
        transform.position = new Vector3(300,100,300);
    }
    //void LateUpdate()
    //{
    //    if (portalPos != null)
    //    {

    //        Vector3 playeroffset = playerCam.position - portalPos.position;
    //        Vector3 newPositionOffset = Vector3.one;

    //        //if (right)
    //        //{
    //        //    whichSideX_Rotation = -1;
    //        //    whichSideZ_Rotation = -1;
    //        //    whichSideX_Position = -1;
    //        //    newPositionOffset = new Vector3(playeroffset.z * whichSideX_Position, playeroffset.y, playeroffset.x * whichSideZ_Position);
    //        //}
    //        //else if (left)
    //        //{
    //        //    whichSideX_Rotation = 1;
    //        //    whichSideZ_Rotation = 1;
    //        //    whichSideX_Position = 1;
    //        //    whichSideZ_Position = -1;
    //        //    newPositionOffset = new Vector3(playeroffset.z * whichSideX_Position, playeroffset.y, playeroffset.x * whichSideZ_Position);
    //        //}
    //        //else if (opposite)
    //        //{
    //        //    whichSideX_Rotation = -1;
    //        //    whichSideZ_Rotation = -1;
    //        //    whichSideX_Position = 1;
    //        //    whichSideZ_Position = 1;
    //        //    newPositionOffset = new Vector3(playeroffset.x * whichSideX_Position, playeroffset.y, playeroffset.z * whichSideZ_Position);
    //        //}

    //        newPositionOffset = new Vector3(playeroffset.x, playeroffset.y, playeroffset.z);
    //        transform.position = otherPortalPos.position + newPositionOffset;

    //        float angleDifference = Quaternion.Angle(portalPos.rotation, otherPortalPos.rotation);
    //        Quaternion portalRotDiff = Quaternion.AngleAxis(angleDifference, Vector3.up);
    //        Vector3 newRot = portalRotDiff * new Vector3(playerCam.forward.x * whichSideX_Rotation, playerCam.forward.y, playerCam.forward.z * whichSideZ_Rotation);
    //        transform.rotation = Quaternion.LookRotation(newRot, Vector3.up);
    //    }

    //}
    public float radius = 5.0f;  // The radius of the circular motion
    public float speed = 1.0f;   // The speed of the circular motion

    private float angle = 0.0f;  // The current angle in radians

    void Update()
    {
        angle += speed * Time.deltaTime;

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, transform.position.y, z);
        transform.Rotate(Vector3.up, 150 * Time.deltaTime);

    }


    //[SerializeField] private Transform playerCam;



    //void Update()
    //{
    //    Vector3 newRot = new Vector3(playerCam.forward.x, playerCam.forward.y, playerCam.forward.z);
    //    transform.rotation = Quaternion.LookRotation(newRot, Vector3.up);
    //}
}
