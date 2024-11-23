using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.playerTag))
        {
            Debug.Log("Damage");
            collision.gameObject.GetComponent<Character>().ApplyDamage(1);
        }
    }

}
