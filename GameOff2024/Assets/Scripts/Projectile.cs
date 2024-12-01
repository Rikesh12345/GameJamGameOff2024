using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 20f; // Speed of the projectile
    [SerializeField]
    private int _projectileDamage = 1; // Damage of the projectile
    [SerializeField]
    private string _enemyTag;   // Tag to specify enemy units
    [SerializeField]
    private float _knockbackForce = 10f; // Force applied for knockback
    [SerializeField]
    private float _aoeKnockbackRadius = 10f; // Radius for the AoE knockback on destruction

    private Vector3 _direction;
    private float _maxDistance;
    private Vector3 _startPosition;
    //private int _projectileDamage;
    private bool _knockback;

    [SerializeField]
    private GameObject _projectileIntoWaterEffectPrefab;
    [SerializeField]
    private GameObject _projectileIntoWaterShaderEffect;
    [SerializeField]
    private AudioClip[] _projectileIntoWaterSounds;

    public void Initialize(Vector3 direction, float maxDistance, bool knockback = false)
    {
        try
        {
            _direction = direction.normalized;
            _maxDistance = maxDistance;
            _startPosition = transform.position;
            //_projectileDamage = projectileDamage;
            _knockback = knockback;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Initialize: " + ex);
        }
    }

    private void Update()
    {
        try
        {
            // Move the projectile
            transform.position += _direction * _speed * Time.deltaTime;

            // Destroy the projectile if it exceeds the max range
            if (Vector3.Distance(_startPosition, transform.position) >= _maxDistance)
            {
                HandleDestruction(false);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            // Only deal damage if the other object has the enemy tag
            if (other.CompareTag(_enemyTag))
            {
                Unit unit = other.GetComponent<Unit>();
                if (unit != null)
                {
                    // Inflict damage on the unit
                    unit.TakeDamage(_projectileDamage, transform);

                    // Apply knockback if enabled
                    if (_knockback && unit.TryGetComponent(out Rigidbody2D rb))
                    {
                        Vector2 knockbackDirection = (unit.transform.position - transform.position).normalized;
                        if (other.CompareTag("Player"))
                        {
                            if (other.TryGetComponent(out PlayerController playerController))
                            {
                                if (playerController.IsCutsceneActive)
                                {
                                    playerController.EnableMovement();
                                }
                            }
                        }
                        else
                        {
                            //rb.AddForce(knockbackDirection * _knockbackForce, ForceMode2D.Impulse);
                        }

                        unit.StartCoroutine(unit.ApplyKnockback(knockbackDirection * _knockbackForce));
                    }

                    // Destroy the projectile on impact
                    HandleDestruction(true);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerEnter2D: " + ex);
        }
    }

    private void HandleDestruction(bool hit)
    {
        try
        {
            if (!hit)
            {
                // Play destruction effects
                //SoundFXManager.Instance.PlayRandomSoundFXClip(_projectileIntoWaterSounds, transform, 0.35f);
                SoundFXManager.Instance.PlayRandomSoundFXClip(_projectileIntoWaterSounds, transform, 0.35f, cooldown: 0.3f, soundKey: "ProjectileImpact");

                GameObject projectileIntoWaterEffectIns = Instantiate(_projectileIntoWaterEffectPrefab, transform.position, Quaternion.identity);
                GameObject projectileIntoWaterShaderEffect = Instantiate(_projectileIntoWaterShaderEffect, transform.position, Quaternion.identity);
                Destroy(projectileIntoWaterEffectIns, 2f);
                Destroy(projectileIntoWaterShaderEffect, 1f);
            }

            if (_knockback)
            {
                // Perform AoE knockback
                PerformAoEKnockback();
            }


            // Destroy the projectile object
            Destroy(gameObject);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".HandleDestruction: " + ex);
        }
    }

    private void PerformAoEKnockback()
    {
        try
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _aoeKnockbackRadius);
            foreach (Collider2D collider in hitColliders)
            {
                if (collider.CompareTag(_enemyTag))
                {
                    if (collider.TryGetComponent(out Rigidbody2D rb))
                    {
                        Unit unit = collider.GetComponent<Unit>();

                        if (unit != null)
                        {
                            Vector2 knockbackDirection = (collider.transform.position - transform.position).normalized;

                            unit.StartCoroutine(unit.ApplyKnockback(knockbackDirection * _knockbackForce));
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".PerformAoEKnockback: " + ex);
        }
    }

    private void OnDrawGizmosSelected()
    {
        try
        {
            // Visualize the AoE knockback radius in the editor
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _aoeKnockbackRadius);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnDrawGizmosSelected: " + ex);
        }
    }
}
