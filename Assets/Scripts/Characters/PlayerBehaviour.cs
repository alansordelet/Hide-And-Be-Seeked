using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using Cinemachine;


public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] CinemachineVirtualCamera MainCamera;
    [SerializeField] GameObject Priest;
    [SerializeField] GameObject Flashlight;
    [SerializeField] CharacterController Ccontroller;
    [SerializeField] GameObject grabPNG;
    [SerializeField] GameObject holdPNG;
    [SerializeField] GameObject Portal1, Portal2, Portal3;
    [SerializeField] AudioSource grabbedWine;
    private float rotationY = 0f;
    private float rotationX = 0f;

    private float rotationLimitY = 0f;
    private float rotationLimitX = 0f;

    public Vector2 dirLook;

    public bool priestIsInCameraView;

    private bool isCrouching = false;

    private float originalHeight;

    private Rigidbody hitRigidbody;

    private bool hasGivenWine = false;

    private Vector3 targetPosition;

    float baseDistance;

    private float speed = 4f;
    [SerializeField] private float mouseSpeed = 4f;

    public float stepRate = 0.5f;
    public float stepVolume = 1f;
    public SphereCollider playerVolumeCollider;

    [SerializeField] float maxSpeedReductionPercentage = 0.5f; // 50% reduction at max fear level
    private void Start()
    {

        mouseSpeed = PlayerPrefs.GetFloat("Sensitivity", 500f) / 100f;
        originalHeight = Ccontroller.height;
        // Ccontroller.enabled = false;

        rotationLimitX = 25;
        rotationLimitY = 50;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (!PauseManager.instance.isPaused)
        {
            CamRotationAndMovement();

            if (Input.GetKeyDown(KeyCode.LeftControl))
                ToggleCrouch();

            Sprint();
            InteractionWithObjects();
            DeletePortals();
        }
      
    }

    private void CamRotationAndMovement()
    {
        dirLook.x = Input.GetAxisRaw("Mouse X");
        dirLook.y = Input.GetAxisRaw("Mouse Y");

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        if (Ccontroller != null || Ccontroller.enabled == true && !GameManager.instance.Death/* && GameManager.instance.outTaxi*/)

        {
            float speedReductionPercentage = (FearManager.instance.currentFear / FearManager.instance.maxFear) * maxSpeedReductionPercentage;
            float modifiedSpeed = speed - (speed * speedReductionPercentage);


            Ccontroller.SimpleMove(moveDirection * modifiedSpeed);
        }

        if (GameManager.instance.Death)
        {
            Ccontroller.enabled = false;
        }

        SetRotation();
    }

    private void SetRotation()
    {
        float sensitivityValue = PlayerPrefs.GetInt("Sensitivity", 500) / 100f;
        mouseSpeed = sensitivityValue * 5f;

        if (GameManager.instance.outTaxi)
        {
            rotationLimitX = 80;
            rotationLimitY = Mathf.Infinity;
        }

        rotationX -= dirLook.y * mouseSpeed;
        rotationX = Mathf.Clamp(rotationX, -rotationLimitX, rotationLimitX);

        rotationY += dirLook.x * mouseSpeed;
        rotationY = Mathf.Clamp(rotationY, -rotationLimitY, rotationLimitY);

        // Apply the rotation to the player's transform and camera
        transform.localRotation = Quaternion.Euler(0f, rotationY, 0f);
        MainCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    public void SetRotationValues(float newRotationX, float newRotationY)
    {
        rotationX = newRotationX;
        rotationY = newRotationY;
        SetRotation();
    }
    private bool soundPlayed = false;
    private void InteractionWithObjects()
    {
        int layer = LayerMask.NameToLayer("MovableObject");
        int layerMask = 1 << layer;
        Vector3 direction = MainCamera.transform.forward;
        if (Physics.Raycast(new Ray(MainCamera.transform.position, direction), out RaycastHit hit, 10f, layerMask))
        {
            grabPNG.SetActive(true);
            if (Input.GetMouseButtonDown(1))
            {
                hitRigidbody = hit.rigidbody;
                if (hitRigidbody != null)
                {
                    hitRigidbody.useGravity = false;
                    hitRigidbody.isKinematic = false;
                    baseDistance = Vector3.Distance(MainCamera.transform.position, hitRigidbody.transform.position);
                }
            }
        }
        if (hitRigidbody != null)
        {
            if (hitRigidbody.CompareTag("WineBottle") && !soundPlayed)
            {
                grabbedWine.Play();
                soundPlayed = true;
            }

            if (Puppet.instance.hasWorked && !hasGivenWine)
            {
                Destroy(hitRigidbody.gameObject);
                hasGivenWine = true;
            }
            grabPNG.SetActive(false);
            holdPNG.SetActive(true);
            targetPosition = MainCamera.transform.forward * baseDistance + MainCamera.transform.position;
            Vector3 objectToTarget = targetPosition - hitRigidbody.transform.position;
            Vector3 objectToTargetNormalized = objectToTarget.normalized;
            hitRigidbody.velocity = objectToTarget * 10f;
            baseDistance = Mathf.Clamp(baseDistance, 1f, 100f);
            if (Input.GetMouseButtonUp(1))
            {
                holdPNG.SetActive(false);
                hitRigidbody.AddForce(MainCamera.transform.forward * 1f, ForceMode.Impulse);
                hitRigidbody.useGravity = true;
                hitRigidbody = null;
                soundPlayed = false;
            }

        }
        if (Input.GetMouseButtonUp(1))
            soundPlayed = false;


    }

    private void ToggleCrouch()
    {
        isCrouching = !isCrouching;

        if (isCrouching)
        {
            Crouch();
            speed = 2.5f;
            stepVolume = 0.3f;
            playerVolumeCollider.radius = 0.75f;
        }
        else
        {
            StandUp();
            speed = 4f;
            stepVolume = 1f;
            playerVolumeCollider.radius = 2f;
        }
    }

    private void Crouch()
    {
        Ccontroller.height = originalHeight / 3f;
    }

    private void StandUp()
    {
        Ccontroller.height = originalHeight;
    }

    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching)
        {
            speed = 6.5f;
            stepRate = 0.35f;
            playerVolumeCollider.radius = 4f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 4f;
            stepRate = 0.5f;
            playerVolumeCollider.radius = 2f;
        }
    }

    private void DeletePortals()
    {
        if (Puppet.instance != null)
        {
            if (Puppet.instance.hasWorked)
            {
                Portal1.SetActive(false);
                Portal2.SetActive(false);
                Portal3.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        int layer = LayerMask.NameToLayer("MovableObject");
        int layerMask = 1 << layer;
        Vector3 direction = MainCamera.transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(MainCamera.transform.position, MainCamera.transform.position + direction * 10f);

        if (Physics.Raycast(MainCamera.transform.position, direction, out RaycastHit hit, 10f, layerMask))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(MainCamera.transform.position, hit.point);
        }
    }
}
