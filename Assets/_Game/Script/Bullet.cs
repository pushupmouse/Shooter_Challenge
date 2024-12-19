using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    //TODO: this should be part of the gun script
    [SerializeField] private float lifeTime = 4f;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float damage = 10f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // Ensure no leftover forces from previous activations
        rb.velocity = Vector2.zero;
        StartCoroutine(ReturnToPoolAfterDelay(lifeTime));
    }

    // Public method to initialize the bullet with a direction and speed
    public void OnInit(Vector2 direction)
    {
        rb.AddForce(direction.normalized * bulletSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);

        IDamagable target = collision.GetComponent<IDamagable>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
    }
}
