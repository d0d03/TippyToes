using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EMP : MonoBehaviour
{
    public Transform targetTransform;
    public static event System.Action EnterMinigame;
    public static event System.Action ExitMinigame;
    public static event System.Action MinigameFailed;
    public static event System.Action InRangeToHack;
    public static event System.Action NotInRangeToHack;
    public static event System.Action TriggerEMP;

    public GameObject cameraOne;
    public GameObject cameraTwo;

    AudioListener cameraOneAudioLis;
    AudioListener cameraTwoAudioLis;

    public GameObject miniGame;
    AudioSource audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        cameraOneAudioLis = cameraOne.GetComponent<AudioListener>();
        cameraTwoAudioLis = cameraTwo.GetComponent<AudioListener>();
        cameraTwoAudioLis.enabled = false;
        audioSource = GetComponent<AudioSource>();
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
        else
        {
            if (NotInRangeToHack != null)
            {
                NotInRangeToHack();
            }
        }
    }

    void PlayMiniGame()
    {
        cameraOne.SetActive(false);
        cameraOneAudioLis.enabled = false;
        miniGame.SetActive(true);
        cameraTwoAudioLis.enabled = true;
        FindObjectOfType<PlayerController>().OnDoorHacked += DisableGuards;
        FindObjectOfType<PlayerController>().OnPlayerDeath += SoundAlarm;
    }

    void DisableGuards()
    {
        if (TriggerEMP != null) {
            TriggerEMP();
        }
        audioSource.Play();
        miniGame.SetActive(false);
        cameraTwoAudioLis.enabled = false;
        cameraOne.SetActive(true);
        cameraOneAudioLis.enabled = true;
        if (ExitMinigame != null)
        {
            ExitMinigame();
        }
    }

    void SoundAlarm()
    {
        if (MinigameFailed != null)
        {
            MinigameFailed();
        }
    }
}
