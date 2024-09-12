using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyScript : MonoBehaviour
{
    private bool activateScariness = false;
    public AudioSource screamerDeadBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activateScariness)
        {
            transform.Translate(-transform.right * 8f * Time.deltaTime);


            if (transform.position.x > 20)
                Destroy(gameObject);
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FearManager.instance.IncreaseFearToMax(100, 30);
            screamerDeadBody.Play();
            activateScariness = true;
        }
    }
}
