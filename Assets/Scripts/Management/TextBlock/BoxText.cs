using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class BoxText : MonoBehaviour
{
    private TMP_Text m_messageText;
    int messageIndex = 0;
    [SerializeField] private AudioSource WritingSound;

    private TextWriter.TextWriterSingle m_writer;
    string[] messageArray = new string[] { "", "", "", "" };
    [SerializeField] TextAsset IntroTextFile;
    [SerializeField] TextAsset BarrierTextFile;
    [SerializeField] TextAsset keyTextFile;
    [SerializeField] TextAsset instructionTextFile;
    [SerializeField] TextAsset roomTextFile;

    private bool Key = true;
    // Start is called before the first frame update
    private List<string[]> messageLists = new List<string[]>();
    private void Awake()
    {
        m_messageText = transform.Find("Message").GetComponent<TMP_Text>();
        Button messageButton = transform.Find("Message").GetComponent<Button>();
        messageButton.onClick.AddListener(OnMessageButtonClick);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_messageText.fontSize = 36;
        if (Input.GetKeyDown(KeyCode.Return))
        {            
            OnMessagePass();
        }

        if (Input.GetKeyDown(KeyCode.I) || GameManager.instance.IntroFinished)
        {
            StartText(IntroTextFile);
            GameManager.instance.IntroFinished = false;
        }

        if (Input.GetKeyDown(KeyCode.P) || GameManager.instance.AtDeactivatedBarrier && !GameManager.instance.GotBarrierKey)
        {
            TextWriter.instance.textBox.SetActive(true);
            StartText(BarrierTextFile);
            GameManager.instance.AtDeactivatedBarrier = false;
        }

        if (Input.GetKeyDown(KeyCode.P) || Key && GameManager.instance.GotBarrierKey)
        {
            TextWriter.instance.textBox.SetActive(true);
            StartText(keyTextFile);
            Key = false;
        }

        if (Input.GetKeyDown(KeyCode.P) || GameManager.instance.AtPuppet)
        {
            TextWriter.instance.textBox.SetActive(true);
            StartText(instructionTextFile);
            GameManager.instance.AtPuppet = false;
        }


        if (Input.GetKeyDown(KeyCode.P) || GameManager.instance.AtRoom)
        {
            TextWriter.instance.textBox.SetActive(true);
            StartText(roomTextFile);
            GameManager.instance.AtRoom = false;
        }
    }

    private void StartText(TextAsset text)
    {
        messageArray = text.text.Split('\n');
        StartWritingSound();
        m_writer = TextWriter.AddWriter_Static(m_messageText, messageArray[0], 0.05f, true, true, StopWritingSound);
    }

    private void OnMessageButtonClick()
    {
        if (m_writer != null && m_writer.isActive())
        {
            m_writer.WriteAllandDestroy();
        }
        else
        {
            if (messageIndex < messageArray.Length - 1)
                messageIndex++;
            else
                messageIndex = 0;
            string message = messageArray[messageIndex];

            StartWritingSound();
            m_writer = TextWriter.AddWriter_Static(m_messageText, message, 0.05f, true, true, StopWritingSound);
        }
    }

    private void OnMessagePass()
    {
        if (m_writer != null && m_writer.isActive())
        {
            m_writer.WriteAllandDestroy();
        }
        else
        {
            if (messageIndex < messageArray.Length - 1)
                messageIndex++;
            else
                DeactivateMessageAndBox();
            string message = messageArray[messageIndex];

            StartWritingSound();
            m_writer = TextWriter.AddWriter_Static(m_messageText, message, 0.05f, true, true, StopWritingSound);
        }
    }

    private void DeactivateMessageAndBox()
    {
        GameObject textBoxGameObject = TextWriter.instance.textBox.gameObject;
        foreach (Transform child in textBoxGameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
        messageIndex = 0;
    }


    private void StopWritingSound()
    {
        WritingSound.Stop();
    }

    private void StartWritingSound()
    {
        WritingSound.Play();
    }
}


