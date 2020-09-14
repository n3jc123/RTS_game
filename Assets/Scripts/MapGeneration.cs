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
    public GameObject mountain;

    private GameObject forrestParent;
    private GameObject goldParent;
    private GameObject stoneParent;
    private GameObject farmParent;
    private GameObject waterParent;
    private GameObject mountainParent;



    private Color[] map;

    public bool smallMap;

    // Start is called before the first frame update
    void Awake()
    {
        forrestParent = GameObject.Find("Forrests");
        goldParent = GameObject.Find("Golds");
        stoneParent = GameObject.Find("Stones");
        farmParent = GameObject.Find("Farms");
        waterParent = GameObject.Find("Waters");
        mountainParent = GameObject.Find("Mountains");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap()
    {
        int randomRotation;

        map = bitMap.GetPixels();

        GameObject spawnedObject;
        for (int x = 0; x < bitMap.width; x++)
        {
            for (int y = 0; y < bitMap.height; y++)
            {

                randomRotation = Random.Range(0, 4);

                if (map[x + y * bitMap.width].r > 0.90f && map[x + y * bitMap.width].g > 0.90f && map[x + y * bitMap.width].b > 0.90f)
                {
                    GetComponent<BuildingManager>().grid.SetValue(x, y, "Stone");
                    spawnedObject = Instantiate(stone, GetComponent<BuildingManager>().grid.GetWorldCenterPosition(x, y), Quaternion.Euler(0, 90 * randomRotation, 0));
                    if (!smallMap)
                        spawnedObject.transform.parent = stoneParent.transform;
                }
                
                else if (map[x + y * bitMap.width].g > 0.45f && map[x + y * bitMap.width].g < 0.55f && map[x + y * bitMap.width].r > 0.45f && map[x + y * bitMap.width].r < 0.55f)
                {
                    spawnedObject = Instantiate(mountain, GetComponent<BuildingManager>().grid.GetWorldCenterPosition(x, y), Quaternion.Euler(0, 90 * randomRotation, 0));
                    if (!smallMap)
                        spawnedObject.transform.parent = mountainParent.transform;
                    for (int i = -5; i < 6; i++)
                    {
                        for (int j = -5; j < 6; j++)
                        {
                            GetComponent<BuildingManager>().grid.SetValue(x + i, y + j, "Mountain");

                        }

                    }
                    

                }
                else if (map[x + y * bitMap.width].g > 0.90f && map[x + y * bitMap.width].r > 0.90f)
                {
                    GetComponent<BuildingManager>().grid.SetValue(x, y, "Gold");
                    spawnedObject = Instantiate(gold, GetComponent<BuildingManager>().grid.GetWorldCenterPosition(x, y), Quaternion.Euler(0, 90 * randomRotation, 0));
                    if (!smallMap)
                        spawnedObject.transform.parent = goldParent.transform;
                }
                else if (map[x + y * bitMap.width].g > 0.90f)
                {
                    GetComponent<BuildingManager>().grid.SetValue(x, y, "Forrest");
                    spawnedObject = Instantiate(forrest, GetComponent<BuildingManager>().grid.GetWorldCenterPosition(x, y), Quaternion.Euler(0, 90 * randomRotation, 0));
                    if(!smallMap)
                        spawnedObject.transform.parent = forrestParent.transform;
                }

                else if (map[x + y * bitMap.width].r > 0.90f)
                {
                    GetComponent<BuildingManager>().grid.SetValue(x, y, "Farm");
                    spawnedObject = Instantiate(farm, GetComponent<BuildingManager>().grid.GetWorldCenterPosition(x, y), Quaternion.identity);
                    if (!smallMap)
                        spawnedObject.transform.parent = farmParent.transform;

                }
                else if (map[x + y * bitMap.width].b > 0.90f)
                {
                    GetComponent<BuildingManager>().grid.SetValue(x, y, "Water");
                    spawnedObject = Instantiate(water, GetComponent<BuildingManager>().grid.GetWorldCenterPosition(x, y), Quaternion.Euler(0, 90 * randomRotation, 0));
                    if (!smallMap)
                        spawnedObject.transform.parent = waterParent.transform;
                }


            }

        }
    }
}
