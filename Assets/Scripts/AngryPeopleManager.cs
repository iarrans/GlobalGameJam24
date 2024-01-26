using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class AngryPeopleManager : MonoBehaviour
{

    public static AngryPeopleManager instance;

    [SerializeField]
    private float waitTime = 5;

    public float molestoTime = 0;
    private float personTime = 0;
    private int chosen;

    [SerializeField]
    private int difficulty = 1;


    [SerializeField]
    private GameObject molesto;

    [SerializeField]
    private List<AngryPerson> peopleList;

    void Awake()
    {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        molesto.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        personTime += Time.deltaTime;


        if (personTime >= waitTime/difficulty) {
            if (molesto.activeSelf == false)
            {
                chosen = Random.Range(0, peopleList.Count);
                print(peopleList[chosen].personName);
                molesto.gameObject.GetComponent<SpriteRenderer>().sprite = peopleList[chosen].image;
                molesto.SetActive(true);
                molestoTime = 0;
            }
            personTime = 0;
        }

        if(molesto.activeSelf == true) { molestoTime += Time.deltaTime; }

        if (molestoTime >= 1) { ReduceMolesto(); molestoTime = 0; }

    }

    void ReduceMolesto()
    {
        if (peopleList[chosen].field == "Risa")
        {
            GameManager.instance.ChangeRisa(peopleList[chosen].strength * -1 * difficulty);
        }
        if (peopleList[chosen].field == "Audiencia")
        {
            GameManager.instance.ChangeAudiencia(peopleList[chosen].strength * -1 * difficulty);
        }
        if (peopleList[chosen].field == "Family Friendly")
        {
            GameManager.instance.ChangeFamilyFriendly(peopleList[chosen].strength * -1 * difficulty);
        }

        if (!GameManager.instance.alive)
        {
            this.gameObject.SetActive(false);
            GameManager.instance.ShowNextQuestion();
            
        }
    }

}
