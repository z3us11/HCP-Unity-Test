using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WavingEnemy : Character
{
    // Start is called before the first frame update
    Vector3 startRotation;
    [SerializeField]
    Vector3 endRotation;
    [SerializeField]
    float rotateSpeed;

    Vector3 initialRotation;
    Vector3 finalRotation;


    float lerpAlpha;
    float timeCount;

    bool isReverse;

    public override void Start()
    {
        base.Start();
        startRotation = transform.rotation.eulerAngles;
        initialRotation = startRotation;
        finalRotation = endRotation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        lerpAlpha = timeCount * rotateSpeed;
        if (lerpAlpha < 1)
        {
            if (!isReverse)
            {
                transform.rotation = Quaternion.Euler(Vector3.Lerp(initialRotation,finalRotation,lerpAlpha));
            }
            else
            {
                transform.rotation = Quaternion.Euler(Vector3.Lerp(finalRotation,initialRotation,lerpAlpha));
            }
        }
        else
        {
            timeCount = 0;
            isReverse = !isReverse;
        }
        timeCount = timeCount + Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Constants.playerTag))
        {
            Debug.Log("Damage");
            collision.gameObject.GetComponent<Character>().ApplyDamage(1);
        }
    }

}
