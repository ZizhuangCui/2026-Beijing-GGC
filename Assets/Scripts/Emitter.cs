using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    private bool isActive = false;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectileAmount = 8;   
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private AudioSource sound;

    private float fireTimer;

    void Update()
    {
        if (!isActive)
        {
            return;
        }
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        sound.enabled = isActive;
    }

    void Fire()
    {
        for (int i = 0; i < projectileAmount; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            isActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            isActive = false;
        }
    }
}
