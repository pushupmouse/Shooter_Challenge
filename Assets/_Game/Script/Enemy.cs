using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public float Health { get; set; }
    public bool IsDead { get; set; }

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

    public void Die()
    {
        IsDead = true;

        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
    }
}
