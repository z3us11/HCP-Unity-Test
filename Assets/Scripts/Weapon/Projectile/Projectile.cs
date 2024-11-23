using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Rigidbody2D rigidbody;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float lifeTime;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.up * moveSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        Debug.Log("Destroyed");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Constants.playerTag))
        {
            Debug.Log("Damage");
            collision.gameObject.GetComponent<Character>().ApplyDamage(1);
        }
        Destroy(gameObject);
    }
}
