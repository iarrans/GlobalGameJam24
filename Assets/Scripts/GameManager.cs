using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using static UnityEngine.Rendering.DebugUI;

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

    public GameObject spectators; //Hay que arreglarlo

    public Light mainLight;

    public GameObject baston;
    public Transform bastonFinPosition;
    public Transform bastonSpawn;

    public List<AudioClip> sfx;
    public AudioSource Mickey;
    public AudioSource Publico;
    public AudioSource Invitado;
    public AudioSource othersfx;

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
                    ShowNextQuestion();
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
                    ShowNextQuestion();
                }
                break;
            case CardType.animacionPJ:
                StartCoroutine(AnimateCharacter(carta));
                break;
            case CardType.animacionPublico:
                StartCoroutine(AnimateSpectators(carta.publicoAnimation));
                break;
            case CardType.Molesto:
                StartCoroutine(SpawnMolesto());
                break;
            case CardType.animacionCam:
                StartCoroutine(CameraShake());
                break;
            case CardType.PantallaFondo:
                StartCoroutine(ChangeScreenImage(carta));
                break;
            case CardType.CambioLuz:
                StartCoroutine(LightChange());
                break;
            case CardType.EfectoSonido:
                StartCoroutine(PlaySoundEffects(carta.efectoSonido));
                break;
            default:
                break;
        }
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
        UIManager.instance.questionsCanvas.SetActive(false);

        float speed = CompanionSpeed;

        GameObject character = GameObject.Instantiate(baston, bastonSpawn);
        character.transform.position = bastonSpawn.position;

        Vector3 end = bastonFinPosition.position;
        // speed should be 1 unit per second
        while (character.transform.position != end)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        mainLight.intensity = 0;
        yield return new WaitForSeconds(2);
        //Aquí audio de micky pa casa

        end = invitadoSpawnPosition.position;
        // speed should be 1 unit per second
        while (mickey.transform.position != end)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, bastonSpawn.position, speed * Time.deltaTime);
            mickey.transform.position = Vector3.MoveTowards(mickey.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        mickey.SetActive(false);
        character.SetActive(false);
        yield return null;
        MainUtils.score = roundCounter;
        SceneManager.LoadScene("Game Over");
    }

    public void ShowNextQuestion()
    {
        UIManager.instance.contadorRondas.text = "Minutos en prime time: " + roundCounter;
        int randomIndex = UnityEngine.Random.Range(0, possibleQuestions.Count);
        Question question = possibleQuestions[randomIndex];
        if (alive) {
            if (roundCounter % 10 == 0 && roundCounter != 0)
            {
                if (currentCompanion != null)
                {
                    question = possibleQuestions[9];
                } else
                {
                    question = possibleQuestions[7];
                }
            }
            else
            {
                

                while (question.invitado && currentCompanion == false)
                {
                    randomIndex = UnityEngine.Random.Range(0, possibleQuestions.Count);
                    question = possibleQuestions[randomIndex];
                }
            }
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
        ShowNextQuestion();
    }

    public IEnumerator EnterCharacter()
    {
        float speed = CompanionSpeed;

        int ranIndex= UnityEngine.Random.Range(0, possibleCharacters.Count);

        GameObject character = GameObject.Instantiate(possibleCharacters[ranIndex], invitadoSpawnPosition);
        character.transform.eulerAngles = new Vector3(0, 90, 0);//128
        character.transform.position = invitadoSpawnPosition.position;
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
        ShowNextQuestion();
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
        ShowNextQuestion();
    }

    public IEnumerator AnimateSpectators(string publicoAnimation)
    { 
        spectators.GetComponent<Animator>().Play(publicoAnimation);
        yield return new WaitForSeconds(3);
        spectators.GetComponent<Animator>().Play("Idle");
        ShowNextQuestion();
       
    }

    public IEnumerator CameraShake()
    {
        Camera.main.DOShakePosition(2, 2, 2, 2, true);
        yield return new WaitForSeconds(2);
        ShowNextQuestion();
    }

    public IEnumerator LightChange()
    {
        float originalIntensity = mainLight.intensity;    
        while (mainLight.intensity > 0)
        {
            mainLight.intensity -= 5f;
            yield return new WaitForSeconds(0.05f);
        }
        mainLight.intensity = 0;
        yield return new WaitForSeconds(2f);
        while (mainLight.intensity < 40)
        {
            mainLight.intensity += 5f;
            yield return new WaitForSeconds(0.1f);
        }
        mainLight.intensity = originalIntensity;
        ShowNextQuestion();
    }

    public IEnumerator ChangeScreenImage(Card carta)
    {
        screen.GetComponent<Renderer>().material.mainTexture = carta.imagenEnPantalla;
        Debug.Log("La pantalla de fondo ha cambiado");
        yield return new WaitForSeconds(2f);
        ShowNextQuestion();
    }

    private IEnumerator PlaySoundEffects(AudioClip efectoSonido)
    {
        audioSource.clip = efectoSonido;
        audioSource.Play();
        Debug.Log("*Sonido de martillo de goma*");
        yield return new WaitForSeconds(3);
        ShowNextQuestion();
    }

    private IEnumerator SpawnMolesto()
    {
        MolestoManager.instance.SpawnMolesto();
        yield return new WaitForSeconds(2);
        ShowNextQuestion();
    }

}
