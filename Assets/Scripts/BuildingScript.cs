using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    private Transform[] buildingParts;

    public bool canBePlaced;

    public bool isBuilt = false;

    private int health;

    public bool isClicked = false;

    public GameObject player;




    // Start is called before the first frame update
    void Start()
    {
        buildingParts = GetComponentsInChildren<Transform>();
        canBePlaced = true;
        health = 100;
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if(isClicked)
        {
            if (this.tag == "Townhall")
            {
                player.GetComponent<UIController>().TownhallUI.enabled = true;
                
            }
            else if (this.tag == "Barracks")
            {
                player.GetComponent<UIController>().BarracksUI.enabled = true;
                
            }
            else if (this.tag == "Stables")
            {
                player.GetComponent<UIController>().StablesUI.enabled = true;
                

            }
            
        }
        else
        {
            if (this.tag == "Townhall")
            {
                player.GetComponent<UIController>().TownhallUI.enabled = false;
                
                
            }
            else if (this.tag == "Barracks")
            {
                player.GetComponent<UIController>().BarracksUI.enabled = false;
                
            }
            else if (this.tag == "Stables")
            {
                player.GetComponent<UIController>().StablesUI.enabled = false;
                

            }
        }
        
    }

    void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.tag != "Untagged" && !isBuilt) {

            for (int i = 0; i < buildingParts.Length; i++)
            {
                buildingParts[i].GetComponent<MeshRenderer>().material.color = Color.red;
            }
            canBePlaced = false;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if(!isBuilt)
        {
            for (int i = 0; i < buildingParts.Length; i++)
            {
                buildingParts[i].GetComponent<MeshRenderer>().material.color = Color.green;
            }
            canBePlaced = true;
        }
        
    }
}
