using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private int amount = 3000;
    private float timer = 0.5f;
    public bool isGathered;
    // Start is called before the first frame update
    void Start()
    {
        isGathered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isGathered)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0.5f;
                amount--;
                Debug.Log("Imam se " + amount + " surovin");
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Villager")
        {
            isGathered = true;
            Debug.Log("evooo meeenee");
        }
    }

    void OnTriggerexit(Collider collision)
    {

        if (collision.gameObject.tag == "Villager")
        {
            isGathered = false;


        }
    }
}
