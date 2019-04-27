﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{

    public float buildTimer;

    public bool canBuild = true;

    public bool isBuilt = false;

    public LayerMask objectsToRemove;

    public Material canBuildMat;

    public Material nobuildMat;

    public float areaToRemoveObjects;

    private BoxCollider col;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, areaToRemoveObjects, objectsToRemove);
    }

    public void PlaceBuilding()
    {
        col.isTrigger = true;
        isBuilt = true;
        RemoveObjectInArea(transform.position, areaToRemoveObjects);
    }

    private void Fade(Material material, float value)
    {
        Color color = material.color;
        color.a = value;
        material.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BuildingController>())
        {
            canBuild = false;
            meshRenderer.material = nobuildMat;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BuildingController>())
        {
            canBuild = true;
            meshRenderer.material = canBuildMat;
        }
    }


    private void RemoveObjectInArea(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, objectsToRemove);

        int i = 0;
        while (i < hitColliders.Length)
        {
            Destroy(hitColliders[i].transform.gameObject);
            i++;
        }
    }
}
