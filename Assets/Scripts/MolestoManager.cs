using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class MolestoManager : MonoBehaviour
{

    public static MolestoManager instance;

    [SerializeField]
    private float waitTime;

    public float molestoTime = 0;
    private float personTime = 0;
    private int chosen;

    [SerializeField]
    private int difficulty = 1;

    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private float damage = 1;


    [SerializeField]
    private GameObject molesto;
    private GameObject m;

    [SerializeField]
    private GameObject spawnerTerr;
    [SerializeField]
    private GameObject spawnerFly;
    [SerializeField]
    private List<GameObject> exitPoints;
    public int exit;


    [SerializeField]
    private List<Molesto> peopleList;

    [SerializeField]
    private List<GameObject> posMarkers;

    [SerializeField]
    private List<GameObject> flyMarkers;
    private Vector3 markerpos;

    public int currentMarker = 0;

    public bool MolestoHit = false;

    void Awake()
    {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        molesto.SetActive(false);
        waitTime = Random.Range(3, 10);
        m = molesto.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (MolestoHit)
        {
            MolestoLeave();
        }
        else {
            MoveMolesto();
        }
        

        personTime += Time.deltaTime;


        if (personTime >= waitTime/difficulty) {
            if (!molesto.activeSelf)
            {
                SpawnMolesto();
            }
            personTime = 0;
            waitTime = Random.Range(3, 10);
        }

        if(molesto.activeSelf && !MolestoHit) { molestoTime += Time.deltaTime; }

        if (molestoTime >= 1) { ReduceMolesto(); molestoTime = 0; }

    }

    public void SpawnMolesto()
    {
        if (molesto.activeSelf)
        {
            return;
        }
        chosen = Random.Range(0, peopleList.Count);
        print(peopleList[chosen].personName);
        molesto = Instantiate(peopleList[chosen].sprite);
        molesto.SetActive(true);
        molestoTime = 0;
        
        if (chosen == 0)
        {
            currentMarker = 3;
            molesto.transform.position = spawnerFly.transform.position;
        }
        else
        {
            currentMarker = 0;
            molesto.transform.position = spawnerTerr.transform.position;
        }
        
    }

    void MoveMolesto()
    {
        speed = Mathf.Min(Mathf.Max(GameManager.instance.roundCounter, 1) * difficulty / 3.0f, 10);
        if(chosen == 0)
        {
            markerpos = flyMarkers[currentMarker].GetComponent<Transform>().position;
        }
        else
        {
            markerpos = posMarkers[currentMarker].GetComponent<Transform>().position;
        }
        
        molesto.transform.position = Vector3.MoveTowards(molesto.transform.position, markerpos, Time.deltaTime * speed);
    }

    void MolestoLeave()
    {
        speed = 5;
        GameObject exitP = exitPoints[exit];

        molesto.transform.position = Vector3.MoveTowards(molesto.transform.position, exitP.transform.position, Time.deltaTime * speed);
        if (molesto.transform.position == exitP.transform.position)
        {
            molesto.SetActive(false);
            MolestoHit = false;
            Destroy(molesto);
            molesto = m.gameObject;
        }
    }

    void ReduceMolesto()
    {
        damage = Mathf.Min(Mathf.Max(GameManager.instance.roundCounter, 1) * difficulty / 5.0f, 10);
        switch (peopleList[chosen].field)
        {
            case "Risa":
                GameManager.instance.ChangeRisa(peopleList[chosen].strength * damage, RangeEffect.Down);
                break;
            case "Audiencia":
                GameManager.instance.ChangeAudiencia(peopleList[chosen].strength * damage, RangeEffect.Down);
                break;
            case "Family Friendly":
                GameManager.instance.ChangeFamilyFriendly(peopleList[chosen].strength * damage, RangeEffect.Down);
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
