using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    const string InnerWalls = "InnerWalls";

    [SerializeField] GameObject levelPlatform;
    [SerializeField] GameObject gateOpening;
    [SerializeField] GameObject gateClosed;
    [SerializeField] int platformCount;
    [SerializeField] Vector2 platformDimensions;
    [SerializeField] bool randomizeSeed;
    [SerializeField] int seed;

    System.Random random;

    Vector3[] directions => new Vector3[]
    {
        Vector3.up * platformDimensions.y,
        Vector3.down * platformDimensions.y,
        Vector3.right * platformDimensions.x,
        Vector3.left * platformDimensions.x
    };


    void Start()
    {
        if (randomizeSeed)
        {
            seed = new System.Random().Next();
        }

        random = new System.Random(seed);

        List<Transform> spawnedPlatforms = new List<Transform>();
        for (int i = 0; i < platformCount; i++)
        {
            Vector3 position = Vector3.zero;
            if (spawnedPlatforms.Count > 0)
            {
                position = GetRandomPlatformPosition(spawnedPlatforms);
            }

            Transform platform = Instantiate(levelPlatform, position, Quaternion.identity, transform).transform;
            platform.name = $"Platform{i + 1:00}";
            spawnedPlatforms.Add(platform);
        }

        List<Vector3> barrierSpawnPoints = new List<Vector3>();

        foreach (Transform platform in spawnedPlatforms)
        {
            SpawnBarriers(spawnedPlatforms, barrierSpawnPoints, platform);
            SetWallStructure(platform);
        }
    }

    private void SetWallStructure(Transform platform)
    {
        Transform innerWalls = platform.Find(InnerWalls);
        int index = random.Next(0, innerWalls.childCount);

        for (int i = 0; i < innerWalls.childCount; i++)
        {
            innerWalls.GetChild(i).gameObject.SetActive(index == i);
        }
    }

    private void SpawnBarriers(List<Transform> spawnedPlatforms, List<Vector3> barrierSpawnPoints, Transform platform)
    {
        foreach (Vector3 dir in directions)
        {
            Vector3 newBarrierPosition = platform.position + dir / 2;

            if (barrierSpawnPoints.Contains(newBarrierPosition))
            {
                Debug.Log($"Platform {platform.name} failed at {newBarrierPosition}", platform);
                continue;
            }

            Debug.Log($"Platform {platform.name} added at {newBarrierPosition}", platform);
            barrierSpawnPoints.Add(newBarrierPosition);
            GameObject typeToSpawn;

            if (FindPlatformAtPosition(spawnedPlatforms, platform.position + dir))
            {
                typeToSpawn = gateOpening;
            }
            else
            {
                typeToSpawn = gateClosed;
            }

            Instantiate(typeToSpawn, newBarrierPosition, Quaternion.LookRotation(Vector3.forward, (newBarrierPosition - platform.position).normalized), transform);
        }
    }

    Vector3 GetRandomPlatformPosition(List<Transform> spawned)
    {
        int index = random.Next(0, spawned.Count);
        int xOffset = random.Next(-1, 2);
        int yOffset = random.Next(-1, 2);

        //Only want 1 direction offset
        if (xOffset != 0 && yOffset != 0)
        {
            if (random.Next(0, 100) < 50)
            {
                yOffset = 0;
            }
            else
            {
                xOffset = 0;
            }
        }

        Vector3 position = spawned[index].position + new Vector3(platformDimensions.x * xOffset, platformDimensions.y * yOffset, 0);

        if (FindPlatformAtPosition(spawned, position) != null) return GetRandomPlatformPosition(spawned);

        return position;
    }

    Transform FindPlatformAtPosition(List<Transform> spawned, Vector3 position)
    {
        foreach (Transform t in spawned)
        {
            if (t.position == position) return t;
        }

        return null;
    }
}
