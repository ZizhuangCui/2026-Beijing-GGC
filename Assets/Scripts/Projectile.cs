using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 3f;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float Deceleration = 0.5f;    
    [SerializeField] private float lifetime = 3f;       

    private Rigidbody2D rb;
    private float timer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float angle = Random.Range(0f, 360f);
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        speed =Random.Range(minSpeed, maxSpeed);
        rb.velocity = direction * speed;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        timer += Time.deltaTime;
        Vector2 newVelocity = rb.velocity;
        if(rb.velocity.x<0)
        {
            newVelocity.x += Deceleration * Time.deltaTime;
        }
        else
        {
            newVelocity.x -= Deceleration * Time.deltaTime;
        }

        if(rb.velocity.y<0)
        {
            newVelocity.y += Deceleration * Time.deltaTime;
        }
        else
        {
            newVelocity.y -= Deceleration * Time.deltaTime;
        }

        rb.velocity = newVelocity;

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
