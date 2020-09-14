using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    private int numberOfEnemyUnits { get; set; }
    public int resourceSum { get; set; }
    public float enemyUnitsDistance { get; set; }
    public float unitsRatio { get; set; }

    public float ratioWeight;
    public float distanceWeight;
    public float resourceWeight;

    public int fCost;
    public int hCost;
    public int gCost;

    public State parentState;
    public State(int _numberOfEnemyUnits, float _enemyUnitsDistance, int _resourceSum, int _allUnitsCount, int _gCost)
    {
        resourceSum = _resourceSum;
        enemyUnitsDistance = _enemyUnitsDistance;
        unitsRatio = _numberOfEnemyUnits / _allUnitsCount;
        gCost = _gCost;
        parentState = null;

        ratioWeight = 0.9f;
        distanceWeight = 0.4f;
        resourceWeight = 0.6f;

        hCost = Mathf.RoundToInt(enemyUnitsDistance * distanceWeight + resourceSum * resourceWeight + unitsRatio * ratioWeight);
    }

    





}
