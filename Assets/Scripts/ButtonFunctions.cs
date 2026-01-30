using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] private AudioSource[] sources;
    private bool isPausing = false;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Boss;
    [SerializeField] AudioSource BGM;
    private void Update()
    {
        if(InputManager.PauseIsPressed && !isPausing)
        {
            isPausing = true;
            pauseGame();
        }
    }
    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void quitGame()
    {
        Debug.Log("quit game");
        Application.Quit();
    }

    public void playSound()
    {
        float random = Random.Range(0,100);
        if(random<50)
        {
            sources[0].Play();
        }
        else
        {
            sources[1].Play();
        }
    }

    void pauseGame()
    {
        PauseMenu.SetActive(true);
        Player.GetComponent<PlayerMovement>().StopMoving();
        BGM.Pause();

        Time.timeScale = 0f;

        if (Boss != null)
        {
            Boss.GetComponent<AudioSource>().enabled = false;
        }
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        isPausing = false;
        Time.timeScale = 1f;
        BGM.Play();

        if (Boss != null)
        {
            //Boss.GetComponent<AudioSource>().enabled = true;
        }
    }
}
