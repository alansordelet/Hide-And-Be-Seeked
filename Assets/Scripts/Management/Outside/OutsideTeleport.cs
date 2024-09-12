using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutsideTeleport : MonoBehaviour
{
    public Image blackFadeImage;

    public Transform playerPos;
    public Transform reciever;

    public GameObject NextWorld;
    public GameObject CurrentWorld;

    public float fadeSpeed;
    private bool playerisOverlapping = false;
    private bool isFadingIn = false;
    private bool isFadingOut = false;
    [SerializeField] AudioSource musicChange;
    [SerializeField] AudioSource currentMusic;
    // Start is called before the first frame update
    void Start()
    {
      //  blackFadeImage.color = Color.black;
    }

    // Update is called once per frame
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
        Color color = blackFadeImage.color;
        if (playerisOverlapping)
        {
            StartFadeIn();
            playerisOverlapping = false;
        }
    }

    void StartFadeIn()
    {
        isFadingIn = true;
        isFadingOut = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            musicChange.Play();
            currentMusic.Stop();
            playerisOverlapping = true;
            GameManager.instance.isTeleporting = true;
        }
          
    }


    void FadeIn()
    {
        Color currentColor = blackFadeImage.color;
        float alpha = Mathf.MoveTowards(currentColor.a, 1.0f, fadeSpeed * Time.deltaTime);
        blackFadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

        if (Mathf.Approximately(alpha, 1.0f))
        {
            playerPos.GetComponent<CharacterController>().enabled = false;
            playerPos.position = new Vector3(reciever.position.x , reciever.position.y, reciever.position.z);
            playerPos.GetComponent<CharacterController>().enabled = true;
            CurrentWorld.SetActive(false);
            NextWorld.SetActive(true);

            isFadingIn = false;
            isFadingOut = true;
        }
    }

    void FadeOut()
    {
        Color currentColor = blackFadeImage.color;
        float alpha = Mathf.MoveTowards(currentColor.a, 0.0f, fadeSpeed * Time.deltaTime);
        blackFadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

        if (Mathf.Approximately(alpha, 0.0f))
        {
            isFadingOut = false;
            GameManager.instance.isTeleporting = false;
        }
    }
}
