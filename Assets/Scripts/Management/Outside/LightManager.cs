using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light[] lights;
    private int currentLight;
    private bool isExecutingCoroutineOffLights = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Light light in lights)
        {
            light.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isExecutingCoroutineOffLights && CameraManager.instance.FirstTransition)
        {
            StartCoroutine(CoroutineTurnOfflights());
        }
        
    }

    private IEnumerator CoroutineTurnOfflights()
    {
        isExecutingCoroutineOffLights = true;
        foreach (Light light in lights)
        {
            yield return new WaitForSeconds(0.5f);
            light.enabled = false;
        }
        yield return new WaitUntil(() => !lights[lights.Length - 1].enabled);
        foreach (Light light in lights)
        {
            yield return new WaitForSeconds(0.5f);
            light.enabled = true;
        }
        isExecutingCoroutineOffLights = false;
    }
}
