using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGeneration : MonoBehaviour
{
    public Texture2D bitMap;
    public GameObject forrest;
    public GameObject water;
    public GameObject gold;
    public GameObject stone;
    public GameObject farm;



    private Color[] map;

    // Start is called before the first frame update
    void Start()
    {
        int randomRotation;
        
        map = bitMap.GetPixels();
        //Debug.Log(map.Length);
        for (int x = 0; x < bitMap.width; x++)
        {
            for(int y = 0; y < bitMap.height; y++)
            {

                randomRotation = Random.Range(0, 4);

                if (map[x + y * bitMap.width].r > 0.90f && map[x + y * bitMap.width].g > 0.90f && map[x + y * bitMap.width].b > 0.90f)
                {
                    GetComponent<BuildingManager>().grid.SetValue(x, y, "Stone");
                    Instantiate(stone, GetComponent<BuildingManager>().grid.GetWorldCenterPosition(x, y), Quaternion.Euler(0, 90 * randomRotation, 0));
                }
                else if (map[x + y * bitMap.width].g > 0.90f && map[x + y * bitMap.width].r > 0.90f)
                {
                    GetComponent<BuildingManager>().grid.SetValue(x, y, "Gold");
                    Instantiate(gold, GetComponent<BuildingManager>().grid.GetWorldCenterPosition(x, y), Quaternion.Euler(0, 90 * randomRotation, 0));
                }
                else if (map[x + y * bitMap.width].g > 0.90f)
                {
                    GetComponent<BuildingManager>().grid.SetValue(x, y, "Forrest");
                    Instantiate(forrest, GetComponent<BuildingManager>().grid.GetWorldCenterPosition(x, y), Quaternion.Euler(0, 90 * randomRotation, 0));
                }
                
                else if (map[x + y * bitMap.width].r > 0.90f)
                {
                    GetComponent<BuildingManager>().grid.SetValue(x, y, "Farm");
                    Instantiate(farm, GetComponent<BuildingManager>().grid.GetWorldCenterPosition(x, y), Quaternion.identity);
                }
                else if (map[x + y * bitMap.width].b > 0.90f)
                {
                    GetComponent<BuildingManager>().grid.SetValue(x, y, "Water");
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
