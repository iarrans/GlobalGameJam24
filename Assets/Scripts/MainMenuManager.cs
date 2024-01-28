using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public GameObject mainMenu;
    public GameObject creditsMenu;

    public string gameScene;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }

    public void StartGame()
    {
        Debug.Log("Start");
        //SceneManager.LoadScene(gameScene);
    }

    public void StartTutorial()
    {
        Debug.Log("Tutorial");
    }

    public void CreditsRoll()
    {
        Debug.Log("Cr�ditos");
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void BackMenu()
    {
        Debug.Log("Vuelta al men�");
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Astro la vista");
        Application.Quit();
    }

}
