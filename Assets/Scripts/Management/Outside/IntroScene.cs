using System.Collections;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerBehaviour playerScript;
    [SerializeField] Transform player;
    [SerializeField] Collider playerCollider;
    [SerializeField] GameObject taxi;
    [SerializeField] GameObject taxiPos;
    [SerializeField] AudioSource taxiDoorAudio;
    [SerializeField] AudioSource taxiAudio;
    [SerializeField] Transform[] wheels;

    private bool fadeIn = false;
    // [SerializeField] FadeMechanic fade;
    float rotationWheel;
 
    private static float timer = 0f;
    private static float timer2 = 0f;
    // Start is called before the first frame update
    private void Awake()
    {
        timer = 0;
        timer2 = 0;
       GameManager.instance.outTaxi = false;
        fadeIn = false;
    }

    private void Start()
    {
        taxiAudio.Play();
        FadeMechanic.instance.StartFadeOut();
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 4.5f)
        {
            taxi.transform.position -= new Vector3(0, 0, 2 * Time.deltaTime);
            characterController.enabled = false;
            playerCollider.enabled = false;
            player.position = new Vector3(taxiPos.transform.position.x, taxiPos.transform.position.y, taxiPos.transform.position.z);
            player.rotation = taxiPos.transform.rotation;
            rotationWheel -= 10f * Time.deltaTime;
            rotationWheel = Mathf.Clamp(rotationWheel, -5f, 0f);
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].RotateAround(wheels[i].transform.position, Vector3.right, rotationWheel);
            }
        }

        if (timer > 3.5f)
        {
            if (!fadeIn)
            {
                FadeMechanic.instance.isFadingIn = true;
                fadeIn = true;
            }
        }

        if (timer > 4.5f)
        {

            timer2 += Time.deltaTime;
            if (timer2 <= 1f)
            {
                Vector3 moveDirection = Vector3.right * -2.5f;
                characterController.Move(moveDirection * Time.deltaTime);
            }
            playerCollider.enabled = true;
            characterController.enabled = true;

            if (!GameManager.instance.outTaxi)
            {
                taxiDoorAudio.Play();
                GameManager.instance.IntroFinished = true;
                GameManager.instance.outTaxi = true;
            }
        }

        if (timer > 8f && timer < 12f)
        {

            rotationWheel += 5f * Time.deltaTime;
            rotationWheel = Mathf.Clamp(rotationWheel, 0f, 5f);
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].RotateAround(wheels[i].transform.position, Vector3.right, rotationWheel);
            }
            taxi.transform.position += new Vector3(0, 0, 2.25f * Time.deltaTime);
        }
    }

}
