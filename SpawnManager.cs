using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] doors;
    public GameObject enemyPrefab;
    public Transform enemies;
    private int spawnOffset;
    private int doorDec;
    
    void Start()
    {
        InvokeRepeating("SpawnEnemies", 2, 0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies()
    {
        doorDec = (Random.Range(0, doors.Length));

        if (doorDec > 2)
        {
            spawnOffset = 2;
        }
        else
        {
            spawnOffset = -2;
        }

        Vector3 spawnLocation = new (doors[doorDec].transform.position.x + spawnOffset, 1.26f, doors[doorDec].transform.position.z) ;

        Instantiate(enemyPrefab, spawnLocation, Quaternion.identity,  enemies);
    }
}
