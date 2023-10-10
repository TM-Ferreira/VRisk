using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnSetter : MonoBehaviour
{
    public GameObject playerRig;
    
    public List<GameObject> spawnPoints;

    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        
        spawnPoints.Clear();
        foreach (Transform point in transform)
        {
            spawnPoints.Add(point.gameObject);
        }
        
        Invoke("SelectPoint", 0.01f);
    }

    private void SelectPoint()
    {
        int selector = Random.Range(0, spawnPoints.Count);
        Vector3 newPosition = spawnPoints[selector].transform.position;
        playerRig.transform.position = newPosition;
    }
}
