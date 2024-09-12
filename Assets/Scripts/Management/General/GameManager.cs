using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject flashlight;
    public GameObject invisbleWall;

    [SerializeField] AudioSource backgroundNoise;
    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    public bool IntroFinished = false;
    public bool AtDeactivatedBarrier = false;
    public bool GotBarrierKey = false;
    public bool AtActivatedBarrier = false;
    public bool AtPuppet = false;
    public bool AtRoom = false;
    public bool outTaxi = false;
    public bool Death = false;
    public bool isTeleporting = false;
    public static GameManager GetInstance()
    {
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        backgroundNoise.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (IntroFinished)
        {
            flashlight.SetActive(true);
            invisbleWall.SetActive(true);
            backgroundNoise.Play();
        }     
    }
}
