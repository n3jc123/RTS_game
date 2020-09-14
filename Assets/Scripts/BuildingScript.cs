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
    public bool isClicked = false;
    public bool villagerArrived;

    public int health;
    public int team = 0;

    private float timer = 0.2f;
    
    public GameObject villager;
    private GameObject player1;
    private GameObject player2;

    public Image healthBar;


    void Start()
    {
        this.transform.GetChild(1).gameObject.SetActive(false);
        buildingParts = this.transform.GetChild(0).gameObject.GetComponentsInChildren<Transform>();
        canBePlaced = true;
        
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
    }

    void Update()
    {
        UpdateHealthBar();
        Build();
        CheckIfDestroyed();

    }

    private void CheckIfDestroyed()
    {
        if (health < 0)
        {

            if (team == 0)
            {
                player1.GetComponent<Player>().RemoveBuilding(this.gameObject);
            }
            else
            {
                player2.GetComponent<Player>().RemoveBuilding(this.gameObject);
            }

            DestroyImmediate(gameObject);
        }
    }
    private void UpdateHealthBar()
    {
        if (health == 100)
        {
            healthBar.enabled = false;
        }
        else
        {
            healthBar.enabled = true;

        }

        healthBar.fillAmount = health / 100f;
    }
    private void Build()
    {
        if (isBeingBuilt)
        {

            this.transform.GetChild(1).gameObject.SetActive(true);
            this.transform.GetChild(0).gameObject.SetActive(false);
            if (villagerArrived)
            {

                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    timer = 0.03f;
                    health++;
                    if (health == 100 && isBeingBuilt)
                    {
                        isBeingBuilt = false;
                        isBuilt = true;

                        if (this.gameObject.tag == "House")
                        {
                            player1.GetComponent<Player>().maxPopulation += 5;
                        }

                    }
                }
            }

        }
        if (isBuilt)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        //ce se zgodi collision spremeni barvo na rdeco
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
        //ce zgradba neha collidat spremeni barvo na zeleno
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
