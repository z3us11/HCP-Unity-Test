using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PatrolingEnemy : Character
{
    [SerializeField]
    GameObject[] partolLocations;
    [SerializeField]
    GameObject sprite;


    Vector3 initialPosition;
    Vector3 finalPosition;


    float lerpAlpha;
    float timeCount;


    int patrolIndex;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        if(partolLocations.Length > 1)
        {
            finalPosition = partolLocations[0].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLocation();
    }

    private void UpdateLocation()
    {
        lerpAlpha = timeCount * moveSpeed;
        if (lerpAlpha < 1)
        {
            sprite.transform.position = Vector3.Lerp(initialPosition, finalPosition, lerpAlpha);
        }
        else
        {
            timeCount = 0;
            patrolIndex++;
            if(patrolIndex >= partolLocations.Length)
            {
                patrolIndex = 0;
            }
            initialPosition = sprite.transform.position;
            finalPosition = partolLocations[patrolIndex].transform.position;
        }
        timeCount = timeCount + Time.deltaTime;
    }

    
}
