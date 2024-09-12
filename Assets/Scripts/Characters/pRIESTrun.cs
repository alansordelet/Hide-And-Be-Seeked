using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pRIESTrun : MonoBehaviour
{
    private bool activateScariness = false;
    public AudioSource screamerDeadBody;
    public AudioSource run;

    void OnEnable()
    {

        screamerDeadBody.Play();
        run.Play();
        activateScariness = true;

    }
    float timer = 0;
    // Update is called once per frame
    void Update()
    {
        if (activateScariness)
        {
            
            timer += Time.deltaTime;
            transform.Translate(-transform.right * 3f * Time.deltaTime);

            if (timer > 3.2)
                Destroy(gameObject);
            //if (transform.position.z > 20)
            //    Destroy(gameObject);
        }

    }


}
