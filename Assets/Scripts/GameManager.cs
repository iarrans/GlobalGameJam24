using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float maxSliderValue;
    public float risa = 100;
    public float audiencia = 100;
    public float familyFriendly = 100;

    public int roundCounter = 0;
    public bool alive = true;

    public GameObject currentCompanion = null; 

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
        ChangeRisa(carta.baseAmountChange, carta.risaEffect);
        ChangeAudiencia(carta.baseAmountChange, carta.audienciaEffect);
        ChangeFamilyFriendly(carta.baseAmountChange, carta.familyFriendlyEffect);
        ShowNextQuestion();
    }

    public void ChangeRisa(float quantity, RangeEffect effect)
    {
        quantity = CalculateQuantityRange(quantity, effect);
        risa += quantity;
        if (risa <= 0)
        {
            alive = false;
            return;
        }       
    }

    public void ChangeAudiencia(float quantity, RangeEffect effect)
    {
        quantity = CalculateQuantityRange(quantity, effect);
        audiencia += quantity;
        if (audiencia <= 0)
        {
            alive = false;
            return;
        }
    }

    public void ChangeFamilyFriendly(float quantity, RangeEffect effect)
    {
        quantity = CalculateQuantityRange(quantity, effect);
        familyFriendly += quantity;
        if (familyFriendly <= 0)
        {
            alive = false;
            return;
        }
    }

    public float CalculateQuantityRange(float quantity, RangeEffect effect)
    {
        float newQuantity = quantity;
        switch (effect)
        {
            case RangeEffect.Down:
                newQuantity = quantity * -1;
                break;
            case RangeEffect.RandomUpDown:
                newQuantity = UnityEngine.Random.Range(-quantity,quantity);
                break;
            case RangeEffect.None:
                newQuantity = 0;
                break;
        }
        return newQuantity;
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
