using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GoTobed : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text goToBedText; // Reference to the UI Text element
    private bool isPlayerNear = false;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerInBed;
    [SerializeField] private AudioSource tapping;
    [SerializeField] private AudioSource screamer;
    [SerializeField] private AudioSource groundbreak;
    [SerializeField] private GameObject groundbreaker;
    bool isPLayerInBed = false;
    // Start is called before the first frame update
    void Start()
    {
        if (onFadeInComplete == null)
            onFadeInComplete = new UnityEvent();

        if (onFadeOutComplete == null)
            onFadeOutComplete = new UnityEvent();

        if (goToBedText != null)
        {
            goToBedText.gameObject.SetActive(false); // Initially hide the text
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            StartFadeIn();
            GoToSleep();
            isPLayerInBed = true;
        }

        if (isPLayerInBed)
            timers += Time.deltaTime;

        if (timers > 3f && !tapping.isPlaying && timers < 4f)
            tapping.Play();
        //if (timers > 5f  && timers < 6f)
           

        if (isFadingIn)
        {
            FadeIn();
        }
        if (isFadingOut && timers > 10f)
        {
            playerInBed.gameObject.SetActive(true);
            FadeOut();
        }

        if ( timers > 13.6f && timers < 14f)
        {
            
            if (!screamer.isPlaying)
            {
                FearManager.instance.IncreaseFearToMax(200, 30);
                screamer.Play();
            }
           

          
        }

        if (timers > 27f)
        {
            if (!groundbreak.isPlaying)
            groundbreak.Play();

            groundbreaker.SetActive(false);
            isPLayerInBed = false;
            timers = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (goToBedText != null)
            {
                goToBedText.text = "Press E to go to bed";
                goToBedText.gameObject.SetActive(true); // Show the text
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (goToBedText != null)
            {
                goToBedText.gameObject.SetActive(false); // Hide the text
            }
        }
    }

    private void GoToSleep()
    {
        if (goToBedText != null)
        {
            goToBedText.gameObject.SetActive(false);
        }
    }

    public Image blackFadeImage;
    [SerializeField] private float fadeSpeed = 1f;
    private float timers = 0f;
    public bool isFadingIn = false;
    public bool isFadingOut = false;

    // Events
    private UnityEvent onFadeInComplete;
    private UnityEvent onFadeOutComplete;
    public void StartFadeIn()
    {
        isFadingIn = true;
        isFadingOut = false;
    }

    public void StartFadeOut()
    {
        isFadingOut = true;
        isFadingIn = false;
    }

    void FadeIn()
    {
        Color currentColor = blackFadeImage.color;
        currentColor.a = Mathf.MoveTowards(currentColor.a, 1.0f, 0.5f * Time.deltaTime);
        blackFadeImage.color = currentColor;

        if (Mathf.Approximately(currentColor.a, 1.0f))
        {
           
            isFadingIn = false;
            isFadingOut = true;


            onFadeInComplete?.Invoke();
        }
    }

    void FadeOut()
    {
        Color currentColor = blackFadeImage.color;
        currentColor.a = Mathf.MoveTowards(currentColor.a, 0.0f, 1f * Time.deltaTime);
        blackFadeImage.color = currentColor;

        if (Mathf.Approximately(currentColor.a, 0.0f))
        {
            isFadingOut = false;
    
            onFadeOutComplete?.Invoke();
        }
    }
}
