using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private int amount = 1510;
    private float timer = 0.5f;
    
    private GameObject player;
    private int villagersGathering = 0;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("Player");
        Debug.Log(this.transform.GetChild(0).name);
        Debug.Log(this.transform.GetChild(1).name);

        this.transform.GetChild(1).gameObject.SetActive(false);
        //Debug.Log("sparse: " + sparseForrest.transform.position);
        //Debug.Log(gameObject.transform.position);
        //sparseForrest.SetActive(false);



    }

    // Update is called once per frame
    void Update()
    {
        
        if(villagersGathering > 0)
        {
            

            if (this.gameObject.tag == "Forrest")
            {
                
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    
                    timer = 0.5f / villagersGathering;
                    amount--;
                    player.GetComponent<Player>().wood++;
                    
                }
            }
            else if (this.gameObject.tag == "Gold")
            {

                timer -= Time.deltaTime;
                if (timer < 0)
                {

                    timer = 0.5f / villagersGathering;
                    amount--;
                    player.GetComponent<Player>().gold++;

                }
            }
            else if (this.gameObject.tag == "Farm")
            {

                timer -= Time.deltaTime;
                if (timer < 0)
                {

                    timer = 0.5f / villagersGathering;
                    amount--;
                    player.GetComponent<Player>().food++;

                }
            }
            if (this.gameObject.tag == "Stone")
            {

                timer -= Time.deltaTime;
                if (timer < 0)
                {

                    timer = 0.5f / villagersGathering;
                    amount--;
                    player.GetComponent<Player>().stone++;

                }
            }

        }
        if(amount < 1500)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
            //sparseForrest.SetActive(true);
            //Instantiate(resourceModel);
            
            
        }
    }

    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Villager")
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
