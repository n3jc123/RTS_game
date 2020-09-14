using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour
{
    
    public void MoveLeft()
    {
        transform.position += Vector3.left * 10;
    }
    public void MoveRight()
    {
        transform.position += Vector3.right * 10;
    }
    public void MoveUp()
    {
        transform.position += Vector3.forward * 10;
    }
    public void MoveDown()
    {
        transform.position += Vector3.back * 10;
    }

    public void Update()
    {
        if(Input.GetKeyDown("up"))
        {
            MoveUp();
        }
        if (Input.GetKeyDown("down"))
        {
            MoveDown();
        }
        if (Input.GetKeyDown("left"))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown("right"))
        {
            MoveRight();
        }
    }
}
