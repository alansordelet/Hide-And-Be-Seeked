using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FootStepScript : MonoBehaviour
{
    public PlayerBehaviour player;
    private float stepCoolDown = 0f;
    public AudioSource footSteps;
    public AudioClip[] footStepsClips_Pavement;


    // Update is called once per frame
    void Update()
    {
        stepCoolDown -= Time.deltaTime;
        if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && GameManager.instance.outTaxi)
        {
            if (stepCoolDown < 0f)
            {
                footSteps.pitch = 1f + Random.Range(-0.2f, 0.2f);
                int randomIndex = Random.Range(0, footStepsClips_Pavement.Length);
                AudioClip clipToPlay = footStepsClips_Pavement[randomIndex]; 

               // Debug.Log(clipToPlay.name + player.stepRate);
                footSteps.PlayOneShot(clipToPlay, player.stepVolume);
                stepCoolDown = player.stepRate;
            }
        }
        else
        {
            footSteps.Stop();
        }
    }
}
