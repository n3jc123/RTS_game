using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class BuildingScript : MonoBehaviour
{
    private Transform[] buildingParts;

    public bool canBePlaced;

    public bool isBuilt = false;

    public bool isBeingBuilt = false;

    private int health;

    public bool isClicked = false;

    public bool isWaitingForVillager;

    public GameObject villager;

    private float timer = 0.2f;

    private GameObject player;

    public Image buildingBar;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.GetChild(1).gameObject.SetActive(false);
        buildingParts = this.transform.GetChild(0).gameObject.GetComponentsInChildren<Transform>();
        canBePlaced = true;
        health = 1;
        player = GameObject.FindGameObjectWithTag("Player");
        isWaitingForVillager = true;
        //buildingBar = transform.GetChild(0).GetComponent<Image>();


    }

    // Update is called once per frame
    void Update()
    {
        if(health == 100)
        {
            buildingBar.enabled = false;
        }
        else
        {
            buildingBar.enabled = true;

        }

        buildingBar.fillAmount = health / 100f;
        if (isBeingBuilt)
        {
            
            this.transform.GetChild(1).gameObject.SetActive(true);
            this.transform.GetChild(0).gameObject.SetActive(false);
            if(villager != null && villager.GetComponent<Villager>().there)
            {
                Debug.Log("lalala");
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    timer = 0.03f;
                    health++;
                    //Debug.Log(health);
                    if (health == 100 && isBeingBuilt)
                    {
                        isBeingBuilt = false;
                        isBuilt = true;
                        //villager.GetComponent<Villager>().there = false;
                        if (this.gameObject.tag == "House")
                        {
                            player.GetComponent<Player>().maxPopulation += 5;
                        }

                    }
                }
            }
            
        }
        if(isBuilt)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.transform.GetChild(1).gameObject.SetActive(false);
        }

        

    }

    void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.tag != "Untagged" && !isBuilt && !isBeingBuilt) {

            for (int i = 0; i < buildingParts.Length; i++)
            {
                buildingParts[i].GetComponent<MeshRenderer>().material.color = Color.red;
            }
            canBePlaced = false;
        }
        
    }

    void OnTriggerExit(Collider collision)
    {
        if(!isBuilt && !isBeingBuilt)
        {
            for (int i = 0; i < buildingParts.Length; i++)
            {
                buildingParts[i].GetComponent<MeshRenderer>().material.color = Color.green;
            }
            canBePlaced = true;
        }
        
    }
}
