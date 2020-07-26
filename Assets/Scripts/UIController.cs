using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Canvas VillagerUI;
    public Canvas TownhallUI;
    public Canvas BarracksUI;
    public Canvas StablesUI;

    // Start is called before the first frame update
    void Start()
    {
        VillagerUI.enabled = false;
        BarracksUI.enabled = false;
        StablesUI.enabled = false;
        TownhallUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
