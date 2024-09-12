using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppet : MonoBehaviour
{
    public static Puppet instance;
    public Animator puppetAnimator;
    public GameObject wineBottle;
    public bool hasWorked = false;
    public AudioSource laughKid;
    public static Puppet GetInstance()
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AtPuppet = true;
        }
        if (other.CompareTag("WineBottle") && !hasWorked)
        {
            laughKid.Play();
            Debug.Log("working");
            transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y + 0.3f, transform.position.z);
            puppetAnimator.SetBool("GotWine", true);
            wineBottle.SetActive(true);
            hasWorked = true;
        }
    }
}
