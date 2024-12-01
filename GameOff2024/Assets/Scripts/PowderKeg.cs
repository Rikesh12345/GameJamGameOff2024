using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderKeg : MonoBehaviour
{
    [SerializeField]
    private string _enemyTag;
    [SerializeField]
    private float _knockbackForce = 10f;
    [SerializeField]
    private int _kegDamage = 5;
    [SerializeField]
    private GameObject _kegExplosionEffect;
    [SerializeField]
    private AudioClip[] _kegExplosionSFX;

    private Animator _animator;

    private void Start()
    {
        try
        {
            _animator = GetComponent<Animator>();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Start: " + ex);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision.CompareTag(_enemyTag))
            {
                Unit unit = collision.GetComponent<Unit>();
                if (unit != null)
                {
                    Vector2 knockbackDirection = (unit.transform.position - transform.position).normalized;

                    unit.TakeDamage(_kegDamage);
                    unit.StartCoroutine(unit.ApplyKnockback(knockbackDirection * _knockbackForce));
                    SoundFXManager.Instance.PlayRandomSoundFXClip(_kegExplosionSFX, GameManager.Instance.PlayerGO.transform, 0.5f);
                    Instantiate(_kegExplosionEffect, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerEnter2D: " + ex);
        }
    }
}
