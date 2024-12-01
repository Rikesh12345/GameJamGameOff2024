using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    [SerializeField]
    private int _tentacleDamage = 1;
    [SerializeField]
    private string _enemyTag;
    [SerializeField]
    private float _knockbackForce = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            if (other.gameObject.TryGetComponent<Unit>(out Unit enemyUnit))
            {
                if (other.CompareTag(_enemyTag) && !enemyUnit.Dead)
                {
                    if (enemyUnit.TryGetComponent(out Rigidbody2D rb))
                    {
                        Unit unit = other.GetComponent<Unit>();

                        if (unit != null)
                        {
                            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

                            unit.StartCoroutine(unit.ApplyKnockback(knockbackDirection * _knockbackForce));
                        }
                    }

                    enemyUnit.TakeDamage(_tentacleDamage);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerEnter2D: " + ex);
        }
    }
}
