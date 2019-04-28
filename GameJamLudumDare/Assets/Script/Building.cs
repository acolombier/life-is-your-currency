﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    public enum BuildingType { House };

    public BuildingType buildingType;

    public float childDeathRate = 0.5f;

    public float adultDeathRate = 0.1f;

    public float mortalityRate = 0.1f;

    public float foodProductionRate = 10f;

    public float foodProductionModifier = 1.1f;

    public int occupanyLimit = 10;

    public float manCount = 1f;

    public float womenCoun = 1f;

    public float childCount = 1f;

    [Tooltip("This is the time to build in Seconds")]
    public float buildTime = 10f;

    public float timeToCompleted;


}
