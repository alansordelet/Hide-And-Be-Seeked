using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TextWriter : MonoBehaviour
{
    private List<TextWriterSingle> textWriterSingle;
    [SerializeField] public GameObject textBox;
    public static TextWriter instance;
    private bool Key = true;
    private void Awake()
    {
        instance = this;
        textWriterSingle = new List<TextWriterSingle>();
    }
    public TextWriterSingle AddWriter(TMP_Text _uiText, string _textToWrite, float _timePerCharacter, bool _invisibleCharacters, Action onComplete)
    {
        TextWriterSingle textWriterSingles = new TextWriterSingle(_uiText, _textToWrite, _timePerCharacter, _invisibleCharacters, onComplete);
        textWriterSingle.Add(textWriterSingles);
        return textWriterSingles;
    }

    public static TextWriterSingle AddWriter_Static(TMP_Text _uiText, string _textToWrite, float _timePerCharacter, bool _invisibleCharacters, bool _removeWriter, Action onComplete)
    {
        if (_removeWriter)
        {
            instance.RemoveWriter(_uiText);
        }
        return instance.AddWriter(_uiText, _textToWrite, _timePerCharacter, _invisibleCharacters, onComplete);
    }
    private static void RemoveWriter_Static(TMP_Text _uiText)
    {
        instance.RemoveWriter(_uiText);
    }
    private void RemoveWriter(TMP_Text _uiText)
    {
        for (int i = 0; i < textWriterSingle.Count; i++)
        {
            if (textWriterSingle[i].GetUIText() == _uiText)
            {
                textWriterSingle.RemoveAt(i);
                i--;
            }
        }
    }
    private void Update()
    {
        for (int i = 0; i < textWriterSingle.Count; i++)
        {
            bool destroyInstance = textWriterSingle[i].Update();
            if (destroyInstance)
            {
                textWriterSingle.RemoveAt(i);
                i--;
            }
        }

        if (Input.GetKeyDown(KeyCode.I) || GameManager.instance.IntroFinished)
        {
            ActivateMessageBox();
        }

        if (Input.GetKeyDown(KeyCode.P) || GameManager.instance.AtDeactivatedBarrier && !GameManager.instance.GotBarrierKey)
        {
            ActivateMessageBox();
        }

        if (Input.GetKeyDown(KeyCode.P) || Key && GameManager.instance.GotBarrierKey)
        {
            ActivateMessageBox();
            Key = false;
        }

        if (Input.GetKeyDown(KeyCode.P) ||GameManager.instance.AtPuppet)
        {
            ActivateMessageBox();
        }

        if (Input.GetKeyDown(KeyCode.P) || GameManager.instance.AtRoom)
        {
            ActivateMessageBox();
        }
    }

    private void ActivateMessageBox()
    {
        if (textBox != null)
        {
            GameObject textBoxGameObject = textBox.gameObject;
            foreach (Transform child in textBoxGameObject.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("TextBox is not assigned in the TextWriter script.");
        }
    }

    public class TextWriterSingle
    {
        private TMP_Text uiText;
        private string textToWrite;
        private float timePerCharacter;
        private int characterIndex;
        private float timer;
        private bool invisibleCharacters;
        private Action onComplete;
        public TextWriterSingle(TMP_Text _uiText, string _textToWrite, float _timePerCharacter, bool _invisibleCharacters, Action onComplete)
        {
            this.uiText = _uiText;
            this.textToWrite = _textToWrite;
            this.timePerCharacter = _timePerCharacter;
            this.invisibleCharacters = _invisibleCharacters;
            this.onComplete = onComplete;
            characterIndex = 0;
        }
        public bool Update()
        {
            timer -= Time.deltaTime;
            while (timer <= 0)
            {
                timer += timePerCharacter;
                characterIndex++;
                string text = textToWrite.Substring(0, characterIndex);
                if (invisibleCharacters)
                {
                    text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
                }
                uiText.text = text;

                if (characterIndex >= textToWrite.Length)
                {
                    if (onComplete != null) onComplete();
                    return true;
                }
            }
            return false;
        }

        public TMP_Text GetUIText()
        {
            return uiText;
        }

        public bool isActive()
        {
            return characterIndex < textToWrite.Length;
        }

        public void WriteAllandDestroy()
        {
            uiText.text = textToWrite;
            characterIndex = textToWrite.Length;
            if (onComplete != null) onComplete();
            TextWriter.RemoveWriter_Static(uiText);
        }
    }
}
