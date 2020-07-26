using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid
{
    private int width;
    private int height;
    private int cellSize;
    private int halfCellSize;
    private Vector3 originPosition;
    private String[,] gridArray;
    private TextMesh[,] debugTextArray;


    public Grid(int width, int height, int cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.halfCellSize = cellSize / 2;

        gridArray = new String[width, height];
        debugTextArray = new TextMesh[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                
                gridArray[x, y] = "Empty";
                //debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y], null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter);
                checkColliders(x, y);
                
                
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                
            }
        }
       
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + originPosition;
    }

    public Vector3 GetWorldCenterPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + originPosition + new Vector3(halfCellSize, 0, halfCellSize);
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    public void SetValue(int x, int y, String value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            //debugTextArray[x, y].text = value;
        }
    }

    public void SetValue(Vector3 worldPosition, String value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        //Debug.Log(x + ", " + y);
        SetValue(x, y, value);
    }

    public String GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return "";
        }
    }

    public String GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public Vector3 GetTileCenterPosition()
    {
        int x, y;
        GetXY(GetMouseWorldPosition(), out x, out y);
        return GetWorldPosition(x, y) + new Vector3(halfCellSize, 0, halfCellSize);
    }

    private void checkColliders(int x, int y)
    {
        Collider[] cols = Physics.OverlapSphere(GetWorldPosition(x, y) + new Vector3(halfCellSize, 0, halfCellSize), halfCellSize);
        foreach (Collider col in cols)
        {
            if (col.tag != "Untagged")
            {
                SetValue(x, y, col.tag);
                
            }


        }
        
    }



    //---------------------------------//
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = -Vector3.one;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            vec = hit.point;
        }
        
        return vec;

        
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        return worldCamera.ScreenToWorldPoint(screenPosition);
        
    }

    public void setNewValuesForBuildings(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);

        checkColliders(x, y);
        checkColliders(x + 1, y);
        checkColliders(x, y + 1);
        checkColliders(x - 1, y);
        checkColliders(x, y - 1);
    }


}
