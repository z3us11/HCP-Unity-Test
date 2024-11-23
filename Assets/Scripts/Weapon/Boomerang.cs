using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Weapon
{
    Vector3 startPosition;
    [SerializeField]
    Transform endPosition;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float rotateSpeed;

    Vector3 initialPosition;
    Vector3 finalPosition;


    float lerpAlpha;
    float timeCount;

    bool isReverse;
    bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        initialPosition = startPosition;
        finalPosition = endPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            RotateWeapon();
            UpdateLocation();
        }
    }

    void RotateWeapon()
    {
        transform.Rotate(0, 0, 2);
    }

    public override void Fire(Transform shootpoint, float range, bool isPlayer)
    {
        //TODO Boomerang audio
        base.Fire(shootpoint, range, isPlayer);
        isMoving = true;
        canFire = false;

    }
    void UpdateLocation()
    {
        lerpAlpha = timeCount * moveSpeed;
        if (lerpAlpha < 1)
        {
            if (!isReverse)
            {
                transform.position = Vector3.Lerp(initialPosition, finalPosition, lerpAlpha);
            }
            else
            {
                transform.position = Vector3.Lerp(finalPosition, initialPosition, lerpAlpha);
            }
        }
        else
        {
            timeCount = 0;
            if (isReverse)
            {
                isMoving = false;
                isReverse = false;
                StartCoroutine(ApplyCoolDown());
            }
            else
            {
                isReverse = true;
            }

        }
        timeCount = timeCount + Time.deltaTime;
    }
    IEnumerator ApplyCoolDown()
    {
        yield return new WaitForSeconds(coolDown);
        canFire = true;
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
