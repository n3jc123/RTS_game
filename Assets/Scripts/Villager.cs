using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : Unit
{
    

    

    public int gold, wood, stone, food = 0;

    private float timer = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()

    {
        if(isSelected)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            
        }
        else
        {
            this.transform.GetChild(0).gameObject.SetActive(false);

        }
    }

    private void gatherResource()
    {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {



                if (hit.collider.transform.tag == "DenseForrest")
                {
                    timer -= Time.deltaTime;
                    if (timer < 0)
                    {
                        timer = 0.5f;
                        wood++;
                    }
                }
            }
    }

    
}
