using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float acceleration = 75f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackSpeed = 2.5f;
    [SerializeField] private Bullet bulletPF;

    private Rigidbody2D rb;
    private PlayerController target;

    public float Acceleration => acceleration;
    public float MaxSpeed => maxSpeed;
    public float AttackRange => attackRange;
    public float AttackSpeed => attackSpeed;
    public Bullet BulletPF => bulletPF;
    public Rigidbody2D Rb => rb;
    public PlayerController Target => target;
    public float Health { get; set; }
    public bool IsDead { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        OnInit();
    }

    private void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        IsDead = false;
        Health = 100f;
    }

    public void SetTarget(PlayerController player)
    {
        target = player;
    }

    public void Die()
    {
        IsDead = true;

        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
    }
}
