using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    
    public GameObject hostileNPC;
    public GameObject neutralNPC;
    public GameObject passiveNPC;
    public FloatVariable mobCount;
    public FloatVariable mobCap;
    public Vector3 spawnPoint;
    
    List<GameObject> NPCList;

    public float safeZoneRadius;
    public float spawnDistance;

    bool hasSpawnPoint;
    Vector3 distanceVector;
    Transform player;

    void Start()
    {
        mobCount.value = 0;
        NPCList = new List<GameObject>();
        NPCList.Add(hostileNPC);
        NPCList.Add(neutralNPC);
        NPCList.Add(passiveNPC);
        player = GameObject.Find("PlayerParent").GetComponent<Transform>();
    }

    void Update()
    {
        distanceVector = spawnPoint-player.position;
        SpawnRandom();
    }

    private void SpawnRandom()
    {
        if (mobCount.value < mobCap.value)
        {
            if (!hasSpawnPoint)
            {
                SearchSpawnPoint();
            }

            int randomIndex = Random.Range(0, NPCList.Count);
            Instantiate(NPCList[randomIndex], spawnPoint, Quaternion.identity);
            mobCount.value += 1f;
            hasSpawnPoint = false;
        }
    }

    private void SearchSpawnPoint()
    {
        float randomX = Random.Range(player.position.x - spawnDistance, player.position.x + spawnDistance);
        float randomZ = Random.Range(player.position.z - spawnDistance, player.position.z + spawnDistance);
            
        Vector3 randomPos = new Vector3(randomX, 0, randomZ);
   
        randomPos.y = Terrain.activeTerrain.SampleHeight(randomPos);
        spawnPoint = randomPos;

        if (distanceVector.magnitude < safeZoneRadius)
        {
            hasSpawnPoint = false;
        }
        else
        {
            hasSpawnPoint = true;
        }
    }
}
