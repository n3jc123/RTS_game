using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGeneration : MonoBehaviour
{
    public Texture2D bitMap;
    public GameObject denseForrest;
    public GameObject water;


    
    private Color[] map;

    // Start is called before the first frame update
    void Start()
    {

        
        map = bitMap.GetPixels();
        Debug.Log(map.Length);
        for (int x = 0; x < bitMap.width; x++)
        {
            for(int y = 0; y < bitMap.height; y++)
            {
               

                if (map[x + y * bitMap.width].g > 0.90f)
                {
                    GetComponent<BuildingManager>().grid.SetValue(x, y, "Forrest");
                    Instantiate(denseForrest, GetComponent<BuildingManager>().grid.GetWorldCenterPosition(x, y), Quaternion.identity);
                }
                else if(map[x + y * bitMap.width].b > 0.90f)
                {
                    Instantiate(water, GetComponent<BuildingManager>().grid.GetWorldCenterPosition(x, y), Quaternion.identity);
                }

            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
