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
    public int dir = 1;

    [SerializeField]
    private int difficulty = 1;

    [SerializeField]
    private int speed = 3;


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
        molesto.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 molestoPos = molesto.GetComponent<Transform>().position;
        Vector3 newPos = new Vector3(molestoPos.x + Time.deltaTime * difficulty * dir * speed, molestoPos.y, molestoPos.z);
        molesto.GetComponent<Transform>().position = newPos;


        personTime += Time.deltaTime;


        if (personTime >= waitTime/difficulty) {
            if (molesto.GetComponent<SpriteRenderer>().enabled == false)
            {
                chosen = Random.Range(0, peopleList.Count);
                print(peopleList[chosen].personName);
                molesto.gameObject.GetComponent<SpriteRenderer>().sprite = peopleList[chosen].image;
                molesto.GetComponent<SpriteRenderer>().enabled = true;
                molestoTime = 0;
            }
            personTime = 0;
        }

        if(molesto.GetComponent<SpriteRenderer>().enabled == true) { molestoTime += Time.deltaTime; }

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
