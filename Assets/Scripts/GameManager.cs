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
    public AudioSource audioSource = null;

    private void Awake()
    {
        instance = this;
        roundCounter = 0;
    }

    private void Start()
    {
        UIManager.instance.risaSlider.maxValue = maxSliderValue;
        UIManager.instance.audienciaSlider.maxValue = maxSliderValue;
        UIManager.instance.familyFriendlySlider.maxValue = maxSliderValue;
        ShowNextQuestion();
    }

    public void ApplyCardResults(Card carta)
    {
        UIManager.instance.questionsCanvas.SetActive(false);
        ChangeRisa(carta.baseAmountChange, carta.risaEffect);
        ChangeAudiencia(carta.baseAmountChange, carta.audienciaEffect);
        ChangeFamilyFriendly(carta.baseAmountChange, carta.familyFriendlyEffect);
        switch (carta.cardType) {
            case CardType.invitadoSale:
                if (currentCompanion != null)
                {
                    Debug.Log("Sale el invitado");
                }
                else
                {
                    Debug.Log("No hay ningún invitado");
                }
                break;

            case CardType.invitadoEntra:
                if (currentCompanion == null)
                {
                    Debug.Log("Entra el invitado");
                }
                else
                {
                    Debug.Log("Ya hay un invitado");
                }
                break;
            case CardType.animacionPJ:
                Debug.Log("Miki Motos está animado");
                break;
            case CardType.animacionPublico:
                Debug.Log("el público parece animado");
                break;
            case CardType.Molesto:
                MolestoManager.instance.SpawnMolesto();
                Debug.Log("El público parece molesto");
                break;
            case CardType.animacionCam:
                Debug.Log("La cámara se ha agitado");
                break;
            case CardType.PantallaFondo:
                Debug.Log("La pantalla de fondo ha cambiado");
                break;
            case CardType.CambioLuz:
                Debug.Log("La luz ha cambiado");
                break;
            case CardType.EfectoSonido:
                audioSource.clip = carta.efectoSonido;
                audioSource.Play();
                Debug.Log("*Sonido de martillo de goma*");
                break;
            default:
                break;
        }
        ShowNextQuestion();
    }

    public void ChangeRisa(float quantity, RangeEffect effect)
    {
        quantity = CalculateQuantityRange(quantity, effect);
        risa += quantity;
        UIManager.instance.risaSlider.value = risa;
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
        UIManager.instance.audienciaSlider.value = audiencia;
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
        UIManager.instance.familyFriendlySlider.value = familyFriendly;
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
            roundCounter++;
        }
        else
        {
            StartCoroutine(EndGame());
        }
    }


}
