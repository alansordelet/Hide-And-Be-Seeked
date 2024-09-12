using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeathScreen : MonoBehaviour
{
    public GameObject deathScreenPanel;
    public float fadeInDuration = 1.0f;
    private bool isShowing = false;
    private float timer = 0;

    void Start()
    {
        if (deathScreenPanel != null)
        {
            // Initially hide the death screen panel
          //  SetAlphaOfPanel(0);
        }
    }

    void Update()
    {
        if (/*Input.GetKeyDown(KeyCode.Escape) && !isShowing ||*/ GameManager.instance.Death == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
           
            deathScreenPanel.SetActive(true);
            //  StartCoroutine(FadeInDeathScreen());
        }
    }

    IEnumerator FadeInDeathScreen()
    {
        isShowing = true;
        timer = 0;

        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            float alpha = timer / fadeInDuration;
            SetAlphaOfPanel(alpha);
            yield return null;
        }

        // Ensure panel is fully visible after fading in
        SetAlphaOfPanel(1);
    }

    void SetAlphaOfPanel(float alpha)
    {
        foreach (var canvasRenderer in deathScreenPanel.GetComponentsInChildren<CanvasRenderer>())
        {
            canvasRenderer.GetMaterial().color = new Color(1f, 1f, 1f, alpha);
        }
    }


    public void Retry()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        deathScreenPanel.SetActive(false);
        Time.timeScale = 1;
  SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

 

    public void Quit()
    {
#if UNITY_EDITOR
      
        EditorApplication.isPlaying = false;
#else
        // Quit the application
        Application.Quit();
#endif
    }
}
