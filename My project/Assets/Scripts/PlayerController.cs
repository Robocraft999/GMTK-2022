using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Rigidbody2D ProjectilePrefab;
    private float shootPower = 40f;

    public TMP_Text scoreText;
    private int score = 0;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            if (scoreText) scoreText.text = "" + value;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyForce(Vector2 force)
    {
        rb.AddForce(force);
    }

    public void Attack(GameObject other)
    {
        Vector3 dir = other.transform.position - transform.position;
        Vector3 extend = new Vector3(GetComponent<Collider2D>().bounds.extents.x, 0, 0) * dir.normalized.x;
        Rigidbody2D projectile = Instantiate(ProjectilePrefab, transform.position + extend, transform.rotation);
        projectile.AddForce(dir * shootPower);
        StartCoroutine(ProjectileTravelHandler(projectile, other.GetComponent<Collider2D>()));
    }

    public IEnumerator ProjectileTravelHandler(Rigidbody2D projectile, Collider2D other)
    {
        yield return new WaitUntil(() => projectile.IsTouching(other));
        other.GetComponent<Rigidbody2D>().AddForce(projectile.velocity);
        Destroy(projectile);
        GameManager.Instance.scores[name] += 250;
    }
}
