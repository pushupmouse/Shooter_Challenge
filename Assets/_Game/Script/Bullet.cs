using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    //TODO: this should be part of the gun script
    [SerializeField] private float bulletSpeed = 20f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // Ensure no leftover forces from previous activations
        rb.velocity = Vector2.zero;
    }

    // Public method to initialize the bullet with a direction and speed
    public void OnInit(Vector2 direction)
    {
        rb.AddForce(direction.normalized * bulletSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
    }
}
