using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/Card", order = 1)]
public class Card : ScriptableObject
{
    public string descripcion;

    //Tipo y efecto de escoger esta carta
    public RangeEffect risaEffect;

    public RangeEffect audienciaEffect;

    public RangeEffect familyFriendlyEffect;

    public CardType cardType;

    public float baseAmountChange;

    public string mickeyAnimation;
    public string invitadoAnimation;
    public string publicoAnimation;

    public Texture imagenEnPantalla;

    public AudioClip efectoSonido;

}

public enum RangeEffect
{
    None, Up, Down, RandomUpDown
}

public enum CardType
{
    invitadoSale, invitadoEntra, animacionPJ, animacionPublico, Molesto, animacionCam, PantallaFondo, CambioLuz, EfectoSonido
}

