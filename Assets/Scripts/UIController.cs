using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class UIController : MonoBehaviour
{
    public Canvas VillagerUI;
    public Canvas TownhallUI;
    public Canvas BarracksUI;
    public Canvas StablesUI;


    // Start is called before the first frame update
    void Start()
    {
        VillagerUI.gameObject.SetActive(false);
        BarracksUI.gameObject.SetActive(false);
        StablesUI.gameObject.SetActive(false);
        TownhallUI.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatuUI(GameObject selectedBuilding)
    {
        
        
            if (selectedBuilding.tag == "Townhall")
            {
                TownhallUI.gameObject.SetActive(true);
            }
            else if (selectedBuilding.tag == "Stables")
            {
                StablesUI.gameObject.SetActive(true);
            }
            else if (selectedBuilding.tag == "Barracks")
            {
                BarracksUI.gameObject.SetActive(true);
            }

        
    }
    public void DeactivateUI(GameObject selectedBuilding)
    {
        if (selectedBuilding.tag == "Townhall")
        {
            TownhallUI.gameObject.SetActive(false);
        }
        else if (selectedBuilding.tag == "Stables")
        {
            StablesUI.gameObject.SetActive(false);
        }
        else if (selectedBuilding.tag == "Barracks")
        {
            BarracksUI.gameObject.SetActive(false);
        }
    }
        
    
}
