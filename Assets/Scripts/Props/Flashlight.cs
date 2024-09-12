using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Flashlight : MonoBehaviour
{
    public static Flashlight instance { get; private set; }
    public Light flashlightLight;
    public Light flashlightLight2;
    public Light flashlightLight3;
    public bool priestIsInFlashlight = false;
    public float coneAngle = 10f;
    public float coneLength = 30f;
    public Text textBatteryPercentage;
    private float maxBatteryPercentage = 100f;
    public float currentBatteryPercentage = 100f;

    void Update()
    {
        int layer = 1 << LayerMask.NameToLayer("Priest");
        ToggleFlashlight();

        priestIsInFlashlight = false;
        Vector3 direction = flashlightLight.transform.forward;

        if (flashlightLight.enabled)
        {
            currentBatteryPercentage -= 1f * Time.deltaTime;

            if (currentBatteryPercentage < 0f)
            {
                currentBatteryPercentage = 0f;
            }

            for (float angleX = (-coneAngle - 2); angleX <= (coneAngle + 2); angleX += 10f)
            {
                for (float angleY = (-coneAngle + 5); angleY <= (coneAngle - 5); angleY += 10f)
                {
                    Quaternion rotation = Quaternion.AngleAxis(angleX, Vector3.up) * Quaternion.AngleAxis(angleY, Vector3.right);
                    Vector3 coneDirection = rotation * direction;
                    Ray ray = new Ray(flashlightLight.transform.position, coneDirection);
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
                    {
                        priestIsInFlashlight = true;
                    }
                }
            }
        }
           

        if (!flashlightLight.enabled)
        {
            flashlightLight2.enabled = false;
            flashlightLight3.enabled = false;
        }
        else if (flashlightLight.enabled)
        {
            flashlightLight2.enabled = true;
            flashlightLight3.enabled = true;
        }

        int roundedValuePercentage = Mathf.RoundToInt((currentBatteryPercentage / maxBatteryPercentage) * 100);
        textBatteryPercentage.text = "Battery : " + roundedValuePercentage.ToString() + "%";
    }
    public void AddBatterPercentage(float _nbtoAdd)
    {
        currentBatteryPercentage = currentBatteryPercentage + _nbtoAdd;
    }
    void ToggleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlightLight.enabled = !flashlightLight.enabled;
        }
    }
    public static Flashlight GetInstance()
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
    private void Start()
    {
       
    }

    private void OnDrawGizmos()
    {
        Vector3 direction = flashlightLight.transform.forward;

        for (float angleX = (-coneAngle -2); angleX <= (coneAngle + 2); angleX+=10f)
        {
            for (float angleY = (-coneAngle + 5); angleY <= (coneAngle - 5); angleY+=10f)
            {
                Quaternion rotation = Quaternion.AngleAxis(angleX, Vector3.up) * Quaternion.AngleAxis(angleY, Vector3.right);
                Vector3 coneDirection = rotation * direction;
                Vector3 coneTip = flashlightLight.transform.position + coneDirection * coneLength;

                // Draw lines from the flashlight position to the cone tip
                Gizmos.DrawLine(flashlightLight.transform.position, coneTip);
            }
        }
    }
}
