using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MolestoManager : MonoBehaviour
{

    public static MolestoManager instance;

    [SerializeField]
    private float waitTime;

    public float molestoTime = 0;
    private float personTime = 0;
    private int chosen;
    public int dir = 1;

    [SerializeField]
    private int difficulty = 1;

    [SerializeField]
    private float speed = 1;


    [SerializeField]
    private GameObject molesto;

    [SerializeField]
    private List<Molesto> peopleList;

    void Awake()
    {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        molesto.GetComponent<SpriteRenderer>().enabled = false;
        waitTime = Random.Range(3, 10);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 molestoPos = molesto.GetComponent<Transform>().position;
        speed = Mathf.Min(Mathf.Max(GameManager.instance.roundCounter, 1)  * difficulty/3.0f, 10);
        Vector3 newPos = new Vector3(molestoPos.x + Time.deltaTime * dir * speed, molestoPos.y, molestoPos.z);
        molesto.GetComponent<Transform>().position = newPos;


        personTime += Time.deltaTime;


        if (personTime >= waitTime/difficulty) {
            if (molesto.GetComponent<SpriteRenderer>().enabled == false)
            {
                SpawnMolesto();
            }
            personTime = 0;
            waitTime = Random.Range(3, 10);
        }

        if(molesto.GetComponent<SpriteRenderer>().enabled == true) { molestoTime += Time.deltaTime; }

        if (molestoTime >= 1) { ReduceMolesto(); molestoTime = 0; }

    }

    public void SpawnMolesto()
    {
        chosen = Random.Range(0, peopleList.Count);
        print(peopleList[chosen].personName);
        molesto.gameObject.GetComponent<SpriteRenderer>().sprite = peopleList[chosen].image;
        molesto.GetComponent<SpriteRenderer>().enabled = true;
        molestoTime = 0;
    }

    void ReduceMolesto()
    {
        switch (peopleList[chosen].field)
        {
            case "Risa":
                GameManager.instance.ChangeRisa(peopleList[chosen].strength * difficulty, RangeEffect.Down);
                break;
            case "Audiencia":
                GameManager.instance.ChangeAudiencia(peopleList[chosen].strength * difficulty, RangeEffect.Down);
                break;
            case "Family Friendly":
                GameManager.instance.ChangeFamilyFriendly(peopleList[chosen].strength * difficulty, RangeEffect.Down);
                break;
            default:
                break;
        }

        if (!GameManager.instance.alive)
        {
            molesto.SetActive(false);
            this.gameObject.SetActive(false);
            GameManager.instance.ShowNextQuestion();
            
        }
    }

}
