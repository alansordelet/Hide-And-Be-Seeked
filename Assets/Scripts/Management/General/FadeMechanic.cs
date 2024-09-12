using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeMechanic : MonoBehaviour
{
    public Image blackFadeImage;
    [SerializeField] private float fadeSpeed = 1f;

    public bool isFadingIn = false;
    public bool isFadingOut = false;

    static public FadeMechanic instance;

    // Events
    private UnityEvent onFadeInComplete;
    private UnityEvent onFadeOutComplete;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        if (onFadeInComplete == null)
            onFadeInComplete = new UnityEvent();

        if (onFadeOutComplete == null)
            onFadeOutComplete = new UnityEvent();

        StartFadeOut();
    }
    public static FadeMechanic GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        StartFadeOut();
    }

    void Update()
    {
        if (isFadingIn)
        {
            FadeIn();
        }
        if (isFadingOut)
        {
            FadeOut();
        }
    }

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
        currentColor.a = Mathf.MoveTowards(currentColor.a, 1.0f, fadeSpeed * Time.deltaTime);
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
        currentColor.a = Mathf.MoveTowards(currentColor.a, 0.0f, fadeSpeed * Time.deltaTime);
        blackFadeImage.color = currentColor;

        if (Mathf.Approximately(currentColor.a, 0.0f))
        {
            isFadingOut = false;

            onFadeOutComplete?.Invoke();
        }
    } 
}

