using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private Transform[] buildingParts;

    public bool canBePlaced;

    public bool isBuilt;

    private int _health;

    public bool isClicked = false;

    private string _unit;

    private string _name;

    public Building()
    {
        
        _health = 100;
    }
    // Start is called before the first frame update
    void Start()
    {
        buildingParts = GetComponentsInChildren<Transform>();
        isBuilt = false;
        canBePlaced = true;
        
       

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag != "Untagged" && !isBuilt)
        {

            for (int i = 0; i < buildingParts.Length; i++)
            {
                buildingParts[i].GetComponent<MeshRenderer>().material.color = Color.red;
            }
            canBePlaced = false;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (!isBuilt)
        {
            for (int i = 0; i < buildingParts.Length; i++)
            {
                buildingParts[i].GetComponent<MeshRenderer>().material.color = Color.green;
            }
            canBePlaced = true;
        }

    }
}
