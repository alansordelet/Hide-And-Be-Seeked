using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamerPuppets : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 initialpos = Vector3.zero;
    public GameObject screamer;
    public AudioSource screamerSound;
    private bool hasActivated = false;
    private bool hasplayedSound = false;
    void Start()
    {
        initialpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x != initialpos.x && transform.position.z != initialpos.z && !hasActivated)
        {
            hasActivated = true;
            screamer.gameObject.SetActive(true);
        }

        if (hasActivated)
        {
            if (!hasplayedSound)
            {
                FearManager.instance.IncreaseFearToMax(100f, 20f);
                screamerSound.Play();
                hasplayedSound = true;
            }

        }
        if (screamer.activeSelf)
        {
            StartCoroutine(Delay(0.8f));          
        }
    }

    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        screamer.gameObject.SetActive(false);
    }
}
