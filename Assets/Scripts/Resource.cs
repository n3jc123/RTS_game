using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private int amount = 1510;
    private int villagersGathering = 0;

    private float timer = 0.5f;

    void Start()
    {
        this.transform.GetChild(1).gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(villagersGathering < 0)
        {
            villagersGathering = 0;
        }
        if(villagersGathering > 0)
        {
            

            if (this.gameObject.tag == "Forrest")
            {
                
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    
                    timer = 0.5f / villagersGathering;
                    amount--; 
                }
            }
            else if (this.gameObject.tag == "Gold")
            {

                timer -= Time.deltaTime;
                if (timer < 0)
                {

                    timer = 0.5f / villagersGathering;
                    amount--;
                }
            }
            else if (this.gameObject.tag == "Farm")
            {

                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    timer = 0.5f / villagersGathering;
                    amount--;
                }
            }
            if (this.gameObject.tag == "Stone")
            {

                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    timer = 0.5f / villagersGathering;
                    amount--;
                }
            }

        }
        if(amount < 1500)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    
    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Villager" && collision.gameObject.GetComponent<FSM>().gathering)
        {
            villagersGathering++;
            
        }
    }
    

    void OnTriggerExit(Collider collision)
    {
       
        if (collision.gameObject.tag == "Villager")
        {
            villagersGathering--;
        }
    }
}
