using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private void Awake()
    {
        instance = this;
    }

    //TODO -> Función que, dada pregunta, prepara toda la info en la UI y asigna a los botones las cartas de las opciones
    public void PrepareCardsUI(Question question)
    {
        QuestionText.text = question.enunciado;

        option1Button.buttonCard = question.Option1;
        option1Text.text = question.Option1.descripcion;

        option2Button.buttonCard = question.Option2;
        option2Text.text = question.Option2.descripcion;
    }

}
