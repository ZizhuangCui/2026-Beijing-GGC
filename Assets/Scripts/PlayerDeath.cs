using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float respawnDelay = 1f;
    [SerializeField] bool isDead;
    private Rigidbody2D rigidBody2D;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource respawnSound;

    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isDead = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead && collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDead && other.CompareTag("Projectile"))
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        deathSound.Play();
        animator.SetBool("isDead", isDead);
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.simulated = false;
        GameManager.Instance.OnPlayerDeath();
        Invoke("Respawn", respawnDelay);
    }

    void Respawn()
    {
        isDead = false;
        animator.SetBool("isDead", isDead);
        transform.position = CheckPointManager.Instance.getRespawnPoint().position;
        respawnSound.Play();
        rigidBody2D.simulated = true;
        GameManager.Instance.OnPlayerRespawned();
    }

}
