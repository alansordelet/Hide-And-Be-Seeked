using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FearManager : MonoBehaviour
{
    public static FearManager instance { get; private set; }
    public Image fillingImage;
    public Image leftVeins, rightVeins, vignette;
    private SpotlightController spotlightController;
    public float maxFear = 100f;
    public float currentFear = 10f;
    private void Start()
    {
        currentFear = 10f;
        UpdateFearBar();
    }

    public static FearManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {     
        //if (PriestVisibilityCheck.instance.priestIsInCameraView )
        //    ModifyHealth(-10f * Time.deltaTime);
        if (Flashlight.instance != null)
        if (Flashlight.instance.priestIsInFlashlight && currentFear <= maxFear)
            ModifyFear(25f * Time.deltaTime);
        //else if (spotlightController.playerInLightRay && currentFear <= maxFear)
        //    ModifyFear(15f * Time.deltaTime);
       if (currentFear <= maxFear && currentFear >= 10f)
            ModifyFear(-5f * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.P) && currentFear <= maxFear)
        {
            ModifyFear(100f * Time.deltaTime);
        }       
        if (Input.GetKeyDown(KeyCode.L))
        {
            ModifyFear(-100f * Time.deltaTime);
        }

        if (currentFear >= maxFear - 1)
        {
            //GameManager.instance.Death = true;
        }

        leftVeins.fillAmount = currentFear / maxFear;
        rightVeins.fillAmount = currentFear / maxFear;
        float alpha = currentFear / maxFear;
        alpha = Mathf.Clamp01(alpha);
        Color currentColor = vignette.color;
        currentColor.a = alpha;
        vignette.color = currentColor;
    }
    private void UpdateFearBar()
    {    
        float fillAmount = currentFear / maxFear;
        fillingImage.fillAmount = fillAmount;
    }
    public void ModifyFear(float amount)
    {
        currentFear = Mathf.Clamp(currentFear + amount, 0, maxFear);
        UpdateFearBar();
        
        if (currentFear <= 0)
        {
            
        }
    }

    public void IncreaseFearToMax(float speed, float maxReduction)
    {
        StartCoroutine(IncreaseFearRoutine(speed, maxReduction));
    }

    private IEnumerator IncreaseFearRoutine(float speed, float maxReduction)
    {
        float targetFear = maxFear - maxReduction;
        while (currentFear < targetFear)
        {
            ModifyFear(speed * Time.deltaTime);
            yield return null;
        }
    }
}

