using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float maxSliderValue;
    public float risa = 100;
    public float audiencia = 100;
    public float familyFriendly = 100;

    public int roundCounter = 0;
    public bool alive = true;

    public GameObject mickey;
    public GameObject currentCompanion = null; 

    public List<Question> possibleQuestions;
    public AudioSource audioSource = null;
    public GameObject screen = null;

    public Transform invitadoSpawnPosition;
    public Transform invitadoPosition;
    public Transform MickeyPosition;
    public float CompanionSpeed;

    public List<GameObject> possibleCharacters;

    public List<GameObject> spectators;


    private void Awake()
    {
        instance = this;
        roundCounter = 0;
        DOTween.Init();
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
                    StartCoroutine(ExitCharacter(currentCompanion));
                }
                else
                {
                    Debug.Log("No hay ningún invitado");
                }
                break;

            case CardType.invitadoEntra:
                if (currentCompanion == null)
                {
                    StartCoroutine(EnterCharacter());
                }
                else
                {
                    Debug.Log("Ya hay un invitado");
                }
                break;
            case CardType.animacionPJ:
                StartCoroutine(AnimateCharacter(carta));
                break;
            case CardType.animacionPublico:
                StartCoroutine(AnimateSpectators(carta.publicoAnimation));
                break;
            case CardType.Molesto:
                MolestoManager.instance.SpawnMolesto();
                Debug.Log("El público parece molesto");
                break;
            case CardType.animacionCam:
                StartCoroutine(CameraShake());
                break;
            case CardType.PantallaFondo:
                screen.GetComponent<Renderer>().material.mainTexture = carta.imagenEnPantalla;
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


     public IEnumerator ExitCharacter(GameObject character)
     {
        character.GetComponent<Animator>().Play("Walking");
        character.transform.eulerAngles = new Vector3(0, -90, 0);

        float speed = CompanionSpeed;
        Vector3 end = invitadoSpawnPosition.position;
        // speed should be 1 unit per second
        while (character.transform.position != end)
        {
           character.transform.position = Vector3.MoveTowards(character.transform.position, end, speed * Time.deltaTime);
           yield return new WaitForEndOfFrame();
        }
        currentCompanion = null;
        character.SetActive(false);
     }

    public IEnumerator EnterCharacter()
    {
        float speed = CompanionSpeed;

        int ranIndex= UnityEngine.Random.Range(0, possibleCharacters.Count);

        GameObject character = GameObject.Instantiate(possibleCharacters[ranIndex], invitadoSpawnPosition);
        character.transform.eulerAngles = new Vector3(0, 128, 0);
        currentCompanion = character;
        character.GetComponent<Animator>().Play("Walking");

        Vector3 end = invitadoPosition.position;
        // speed should be 1 unit per second
        while (character.transform.position != end)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        character.transform.LookAt(invitadoSpawnPosition);
        character.GetComponent<Animator>().Play("IddleNeutral");
        character.transform.eulerAngles = new Vector3(0,0,0);
    }

    public IEnumerator AnimateCharacter(Card carta)
    {
        if (carta.mickeyAnimation != null && carta.mickeyAnimation != "")
        {
            mickey.GetComponent<Animator>().Play(carta.mickeyAnimation);
        }
        if (currentCompanion != null && carta.invitadoAnimation != null && carta.invitadoAnimation != "")
        {
            currentCompanion.GetComponent<Animator>().Play(carta.invitadoAnimation);
        }

        yield return new WaitForSeconds(3);

        if (currentCompanion != null) currentCompanion.GetComponent<Animator>().Play("IddleNeutral");
        mickey.GetComponent<Animator>().Play("IddleNeutral");
    }

    public IEnumerator AnimateSpectators(string publicoAnimation)
    {
        foreach (GameObject spectator in spectators)
        {
            spectator.GetComponent<Animator>().Play(publicoAnimation);
        }
        yield return new WaitForSeconds(2);
        foreach (GameObject spectator in spectators)
        {
            spectator.GetComponent<Animator>().Play("SpectatorsIdle");
        }
    }

    public IEnumerator CameraShake()
    {
        Camera.main.DOShakePosition(2, 2, 2, 2, true);
        yield return new WaitForSeconds(2);
    }

}
