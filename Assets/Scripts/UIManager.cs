using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    // Start is called before the first frame update
    public GameObject questionsCanvas;
    public UIButton option1Button;
    public UIButton option2Button;

    public TextMeshProUGUI QuestionText;
    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;

    public Sprite upperVariation;
    public Sprite lowerVariation;
    public Sprite noneVariation;
    public Sprite randomRangeVariation;

    public Slider risaSlider;
    public Slider audienciaSlider;
    public Slider familyFriendlySlider;

    private void Awake()
    {
        instance = this;
    }

    //TODO -> Fmodificar iconos de la UI de los botones
    public void PrepareCardsUI(Question question)
    {
        QuestionText.text = question.enunciado;

        option1Button.buttonCard = question.Option1;
        option1Text.text = question.Option1.descripcion;
        DefineStats(option1Button);

        option2Button.buttonCard = question.Option2;
        option2Text.text = question.Option2.descripcion;
        DefineStats(option2Button);
    }

    public void DefineStats(UIButton boton)
    {
        Card card = boton.buttonCard;

        switch (card.risaEffect)
        {
            case RangeEffect.Up:
                boton.risaStat.sprite = upperVariation;
                break;
            case RangeEffect.Down:
                boton.risaStat.sprite = lowerVariation;
                break;
            case RangeEffect.RandomUpDown:
                boton.risaStat.sprite = randomRangeVariation;
                break;
            case RangeEffect.None:
                boton.risaStat.sprite = noneVariation;
                break;
        }

        switch (card.audienciaEffect)
        {
            case RangeEffect.Up:
                boton.audienciaStat.sprite = upperVariation;
                break;
            case RangeEffect.Down:
                boton.audienciaStat.sprite = lowerVariation;
                break;
            case RangeEffect.RandomUpDown:
                boton.audienciaStat.sprite = randomRangeVariation;
                break;
            case RangeEffect.None:
                boton.audienciaStat.sprite = noneVariation;
                break;
        }

        switch (card.familyFriendlyEffect)
        {
            case RangeEffect.Up:
                boton.familyFriendlyStat.sprite = upperVariation;
                break;
            case RangeEffect.Down:
                boton.familyFriendlyStat.sprite = lowerVariation;
                break;
            case RangeEffect.RandomUpDown:
                boton.familyFriendlyStat.sprite = randomRangeVariation;
                break;
            case RangeEffect.None:
                boton.familyFriendlyStat.sprite = noneVariation;
                break;
        }

    }

}
