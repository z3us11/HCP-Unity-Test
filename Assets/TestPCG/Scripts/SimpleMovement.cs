using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] float speed;
    void Update()
    {
        Vector3 input = new Vector3
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical"),
            z = 0
        };

        transform.position += input * speed * Time.deltaTime;
    }
}
