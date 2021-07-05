using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public Transform targetTransform;
    public static event System.Action EnterMinigame;
    public static event System.Action ExitMinigame;
    public static event System.Action MinigameFailed;
    public static event System.Action InRangeToHack;
    public static event System.Action NotInRangeToHack;

    public GameObject cameraOne;
    public GameObject cameraTwo;

    AudioListener cameraOneAudioLis;
    AudioListener cameraTwoAudioLis;

    public GameObject miniGame;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        cameraOneAudioLis = cameraOne.GetComponent<AudioListener>();
        cameraTwoAudioLis = cameraTwo.GetComponent<AudioListener>();
        cameraTwoAudioLis.enabled = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 displacementFromTarget = targetTransform.position - transform.position;
        float distanceToTarget = displacementFromTarget.magnitude;
        if (distanceToTarget < 5f)
        {
            if (InRangeToHack != null)
            {
                InRangeToHack();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (NotInRangeToHack != null)
                {
                    NotInRangeToHack();
                }
                if (EnterMinigame != null)
                {
                    PlayerPrefs.SetFloat("startTime", Time.timeSinceLevelLoad);
                    EnterMinigame();
                }
                PlayMiniGame();
            }
        }
        else {
            if (NotInRangeToHack != null)
            {
                NotInRangeToHack();
            }
        }
    }

    void PlayMiniGame() {
        cameraOne.SetActive(false);
        cameraOneAudioLis.enabled = false;
        miniGame.SetActive(true);
        cameraTwoAudioLis.enabled = true;
        FindObjectOfType<PlayerController>().OnDoorHacked += UnlockDoor;
        FindObjectOfType<PlayerController>().OnPlayerDeath += SoundAlarm;
    }

    void UnlockDoor() {
        //transform.Translate(Vector3.back * 100 * Time.deltaTime);
        animator.SetBool("Open",true);
        miniGame.SetActive(false);
        cameraTwoAudioLis.enabled = false;
        cameraOne.SetActive(true);
        cameraOneAudioLis.enabled = true;
        if (ExitMinigame != null) {
            ExitMinigame();
        }
    }

    void SoundAlarm() {
        print("you triggered an alarm");
        if (MinigameFailed != null) {
            MinigameFailed();
        }
    }
}
