using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{

    public GameObject gameLoseUI;
    public GameObject gameWinUI;
    public GameObject instructions;
    public AudioSource loseAudio;
    public AudioSource winAudio;

    bool gameIsOver;
    bool lvlPassed;
    bool hasAudioPlayed;

    int sceneCount;
    int currentScene;
    int nextScene;

    void Start()
    {
        hasAudioPlayed = false;
        Guard.OnGuardHasSpotedPlayer += ShowGameLoseUI;

        Door.MinigameFailed += ShowGameLoseUI;
        Door.InRangeToHack += ShowInstructionsDoor;
        Door.EnterMinigame += UnsubscribeInstructionsDoor;

        EMP.EnterMinigame += UnsubscribeInstructionsEMP;
        EMP.MinigameFailed += ShowGameLoseUI;
        EMP.InRangeToHack += ShowInstructionsEMP;

        FindObjectOfType<Player>().OnReachedEndOfLvl += ShowGameWinUI;
        sceneCount = SceneManager.sceneCountInBuildSettings;
        currentScene = SceneManager.GetActiveScene().buildIndex;
        if (sceneCount - 1 == currentScene)
        {
            nextScene = 0;
        }
        else {
            nextScene = currentScene + 1;
        }
    }


    void Update()
    {

        if (gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(currentScene);
            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                SceneManager.LoadScene(0);
            }
        }
        else if (lvlPassed)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(nextScene);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }

    }

    void ShowGameWinUI() {
        if (!hasAudioPlayed) {
            winAudio.Play();
            hasAudioPlayed = true;
        }
        OnGameOver(gameWinUI);
        lvlPassed = true;
    }

    void ShowGameLoseUI()
    {
        if (!hasAudioPlayed)
        {
            loseAudio.Play();
            hasAudioPlayed = true;
        }
        OnGameOver(gameLoseUI);
        gameIsOver = true;
    }

    void OnGameOver(GameObject gameOverUI) {
        gameOverUI.SetActive(true);
        Guard.OnGuardHasSpotedPlayer -= ShowGameLoseUI;
        Door.MinigameFailed -= ShowGameLoseUI;
        EMP.MinigameFailed -= ShowGameLoseUI;
        FindObjectOfType<Player>().OnReachedEndOfLvl += ShowGameWinUI;
    }

    void ShowInstructionsDoor() {
        if (instructions != null) {
            instructions.SetActive(true);
            Door.InRangeToHack -= ShowInstructionsDoor;
            Door.NotInRangeToHack += HideInstructionsDoor;
        }
    }

    void ShowInstructionsEMP()
    {
        if (instructions != null)
        {
            instructions.SetActive(true);
            EMP.InRangeToHack -= ShowInstructionsEMP;
            EMP.NotInRangeToHack += HideInstructionsEMP;
        }
    }

    void HideInstructionsDoor() {
        instructions.SetActive(false);
        Door.InRangeToHack += ShowInstructionsDoor;
        Door.NotInRangeToHack -= HideInstructionsDoor;
    }

    void HideInstructionsEMP()
    {
        instructions.SetActive(false);
        EMP.InRangeToHack += ShowInstructionsEMP;
        EMP.NotInRangeToHack -= HideInstructionsEMP;
    }

    void UnsubscribeInstructionsDoor() {
        Door.InRangeToHack -= ShowInstructionsDoor;
        Door.NotInRangeToHack -= HideInstructionsDoor;
    }

    void UnsubscribeInstructionsEMP()
    {
        EMP.InRangeToHack -= ShowInstructionsEMP;
        EMP.NotInRangeToHack -= HideInstructionsEMP;
    }
}
