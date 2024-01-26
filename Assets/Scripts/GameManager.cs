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
        ChangeRisa(carta.risaEffect);
        ChangeAudiencia(carta.audienciaEffect);
        ChangeFamilyFriendly(carta.familyFriendlyEffect);
    }

    public void ChangeRisa(float quantity)
    {
        risa += quantity;
        if (risa <= 0)
        {
            StartCoroutine(EndGame(0));
            return;
        }
        ShowNextQuestion();
    }

    public void ChangeAudiencia(float quantity)
    {
        audiencia += quantity;
        if (audiencia <= 0)
        {
            StartCoroutine(EndGame(1));
            return;
        }
        ShowNextQuestion();
    }

    public void ChangeFamilyFriendly(float quantity)
    {
        familyFriendly += quantity;
        if (familyFriendly <= 0)
        {
            StartCoroutine(EndGame(2));
            return;
        }
        ShowNextQuestion();
    }

     private IEnumerator EndGame(int motivo)
     {
        Debug.Log("¡Se acabó la partida! " + motivo);
        yield return null;
     }

    public void ShowNextQuestion()
    {
        roundCounter++;
        int randomIndex = UnityEngine.Random.Range(0, possibleQuestions.Count);
        Question question = possibleQuestions[randomIndex];
        UIManager.instance.PrepareCardsUI(question);
    }

}
