using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AngryPeopleManager : MonoBehaviour
{

    public static AngryPeopleManager instance;

    [SerializeField]
    private float waitTime = 5;
    private float personTime = 0;
    private int chosen;


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
        if (personTime >= waitTime) {
            if (molesto.activeSelf == false)
            {
                chosen = Random.Range(0, peopleList.Count);
                print(peopleList[chosen].name);
                molesto.gameObject.GetComponent<SpriteRenderer>().sprite = peopleList[chosen].image;
                molesto.SetActive(true);
            }
            else
            {
                molesto.SetActive(false);
            }
            personTime = 0;
        }
    }


}
