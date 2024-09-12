using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightFlicker : MonoBehaviour
{
    public Transform targetTransform;
    private float detectionDistance = 8f;
    public Light flickeringLight;
    private float flickerIntensity = 7f;
    private float flickerSpeed = 5f;
    private bool isFlickering = false;
    private float originalIntensity;
    //private bool CoroutineStarted = false;

    private void Start()
    {
        if (flickeringLight == null)
        {

            flickeringLight = GetComponent<Light>();
        }

        if (flickeringLight != null)
        {
            originalIntensity = flickeringLight.intensity;
        }
        else
        {
            Debug.LogError("Light component not found or set on the script.");
        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetTransform.position) < detectionDistance)
        {
            if (!isFlickering)
            {
                StartFlicker();
            }
        }
        else
        {

            if (isFlickering)
            {
                StopFlicker();
            }
        }


        if (isFlickering)
        {
            float flickerValue = Mathf.PerlinNoise(Time.time * flickerSpeed, -5f) * flickerIntensity;
            flickeringLight.intensity = originalIntensity + flickerValue;
         
        }
    }

    private void StartFlicker()
    {
        isFlickering = true;
    }

    private void StopFlicker()
    {
        isFlickering = false;
        flickeringLight.intensity = originalIntensity;
    }

    //private IEnumerator CoroutineFlicker()
    //{
    //    CoroutineStarted = true;
    //    flickeringLight.gameObject.SetActive(false);
    //    yield return new WaitForSeconds(0.5f);
    //    flickeringLight.gameObject.SetActive(true);
    //    CoroutineStarted = false;
    //}
}
