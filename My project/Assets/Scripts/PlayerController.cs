using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyForce(Vector2 force)
    {
        rb.AddForce(force);
        print("Force: " + force);
    }

    public void Attack()
    {
        print("ATTACKEEEEEEEEE");
    }
}
