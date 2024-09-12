using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlightSound : MonoBehaviour, IPointerEnterHandler
{
   // public AudioManager audioManager;
    public AudioClip highlightSound;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (audioManager != null && highlightSound != null)
        //{
        //    audioManager.PlaySound(highlightSound);
        //}
    }
}
