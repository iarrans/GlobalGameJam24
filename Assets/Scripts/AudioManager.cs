using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;


    public static AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    public static void audioPlay(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
