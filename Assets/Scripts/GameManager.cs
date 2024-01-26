using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float risa = 100;
    public float audiencia = 100;
    public float familyFriendly = 100;

    public float roundCounter = 0;
    public bool alive = true;

    public List<Question> possibleQuestions;

    private void Awake()
    {
        instance = this;
        roundCounter = 0;
    }

    private void Start()
    {
        ShowNextQuestion();
    }

    public void ApplyCardResults(Card carta)
    {
        UIManager.instance.questionsCanvas.SetActive(false);
        ChangeRisa(carta.risaEffect);
        ChangeAudiencia(carta.audienciaEffect);
        ChangeFamilyFriendly(carta.familyFriendlyEffect);
        ShowNextQuestion();
    }

    public void ChangeRisa(float quantity)
    {
        risa += quantity;
        if (risa <= 0)
        {
            alive = false;
            return;
        }       
    }

    public void ChangeAudiencia(float quantity)
    {
        audiencia += quantity;
        if (audiencia <= 0)
        {
            alive = false;
            return;
        }
    }

    public void ChangeFamilyFriendly(float quantity)
    {
        familyFriendly += quantity;
        if (familyFriendly <= 0)
        {
            alive = false;
            return;
        }
    }

     private IEnumerator EndGame()
     {
        Debug.Log("¡Se acabó la partida! ");
        UIManager.instance.questionsCanvas.SetActive(false);
        yield return null;
     }

    public void ShowNextQuestion()
    {
        if (alive) {
            int randomIndex = UnityEngine.Random.Range(0, possibleQuestions.Count);
            Question question = possibleQuestions[randomIndex];
            UIManager.instance.questionsCanvas.SetActive(true);
            UIManager.instance.PrepareCardsUI(question);
        }
        else
        {
            StartCoroutine(EndGame());
        }
    }

}
