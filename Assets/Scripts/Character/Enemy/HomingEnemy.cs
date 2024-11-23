using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HomingEnemy : Character
{
    [SerializeField]
    PlayerCharacter player;
    [SerializeField]
    float followDistance = 5;

    Rigidbody2D rb;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();   
        //player = FindAnyObjectByType<PlayerCharacter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector2.Distance(transform.position, player.transform.GetChild(0).position) < followDistance)
        {
            //transform.position = Vector2.MoveTowards(transform.position,player.transform.position,moveSpeed*Time.deltaTime);

            // Calculate the direction to the target
            Vector3 direction = (player.transform.GetChild(0).position - transform.position).normalized;

            // Calculate the new position
            Vector3 newPosition = transform.position + direction * moveSpeed * Time.fixedDeltaTime;

            rb.MovePosition(newPosition);


            //rb.AddForce(direction * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
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
