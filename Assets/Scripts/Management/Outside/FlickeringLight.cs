using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    private Light myLight;
    public float timer = 0;
    private void Start()
    {

        myLight = GetComponent<Light>();
        if (myLight == null)
        {
            Debug.LogError("No Light component found on the GameObject. Attach this script to a GameObject with a Light component.");
        }

        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            myLight.enabled = false;
            yield return new WaitForSeconds(timer);
            myLight.enabled = true;
            yield return new WaitForSeconds(timer);
        }
    }
}
