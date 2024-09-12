using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] CinemachineVirtualCamera camVirtual;
    Transform selectedDoor;
    GameObject dragPointGameobject;
    int leftDoor = 0;
    [SerializeField] LayerMask doorLayer;
    [SerializeField] GameObject grabDoorSprite;
    [SerializeField] GameObject holdDoorSprite;
    RaycastHit hit;
    HingeJoint joint;
    JointMotor motor;
    private void Update()
    {
        grabDoorSprite.SetActive(false);
        if (Physics.Raycast(camVirtual.transform.position, camVirtual.transform.forward, out hit, 4f, doorLayer))
        {
            grabDoorSprite.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                selectedDoor = hit.collider.gameObject.transform;
            }
        }
        if (selectedDoor != null)
        {
            grabDoorSprite.SetActive(false);
            holdDoorSprite.SetActive(true);
            if (Input.GetMouseButtonUp(0))
            {
                selectedDoor = null;
                motor.targetVelocity = 0;
                joint.motor = motor;
                Destroy(dragPointGameobject);
            }
        }
        else holdDoorSprite.SetActive(false);
    }
    void FixedUpdate()
    {
        if (selectedDoor != null)
        {
            joint = selectedDoor.GetComponent<HingeJoint>();
             motor = joint.motor;

            //Create drag point object for reference where players mouse is pointing
            if (dragPointGameobject == null)
            {
                dragPointGameobject = new GameObject("Ray door");
                dragPointGameobject.transform.parent = selectedDoor;

                //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //sphere.transform.parent = dragPointGameobject.transform;
                //sphere.transform.localScale = new Vector3(2f, 2f, 2f);
                //sphere.GetComponent<Renderer>().material.color = Color.red;

                //Collider sphereCollider = sphere.GetComponent<Collider>();
                //if (sphereCollider != null)
                //{
                //    sphereCollider.isTrigger = true;
                //}
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            dragPointGameobject.transform.position = ray.GetPoint(Vector3.Distance(selectedDoor.position, transform.position));
            dragPointGameobject.transform.rotation = selectedDoor.rotation;


            float delta = Mathf.Pow(Vector3.Distance(dragPointGameobject.transform.position, selectedDoor.position), 3);

            //Deciding if it is left or right door
            if (selectedDoor.GetComponent<MeshRenderer>().localBounds.center.x > selectedDoor.localPosition.x)
            {
                leftDoor = 1;
            }
            else
            {
                leftDoor = -1;
            }

            //Applying velocity to door motor
            float speedMultiplier = 60000;
            if (Mathf.Abs(selectedDoor.parent.forward.z) > Mathf.Abs(selectedDoor.parent.transform.position.z))
            {
                if (dragPointGameobject.transform.position.x > selectedDoor.position.x)
                {
                    motor.targetVelocity = delta * -speedMultiplier * Time.deltaTime * leftDoor;
                }
                else
                {
                    motor.targetVelocity = delta * speedMultiplier * Time.deltaTime * leftDoor;
                }
            }
            else
            {
                if (dragPointGameobject.transform.position.z > selectedDoor.position.z)
                {
                    motor.targetVelocity = delta * -speedMultiplier * Time.deltaTime * leftDoor;
                }
                else
                {
                    motor.targetVelocity = delta * speedMultiplier * Time.deltaTime * leftDoor;
                }
            }
            joint.motor = motor;

           
        }
    }


}

