using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCannon : MonoBehaviour
{
    [Header("Settings")]
    //private Transform _target; // Target object to point to
    [SerializeField]
    private GameObject _cannonGO;
    [SerializeField]
    private Transform _cannonPoint;
    [SerializeField]
    private GameObject _projectilePrefab;
    private Animator _animator;
    [SerializeField]
    private GameObject _cannonSmokeEffectPrefab;
    [SerializeField]
    private AudioClip[] _cannonSounds;

    [Header("Stats")]
    [SerializeField]
    private float _attackRate = 5.0f; // Time between shots, in seconds
    [SerializeField]
    private float _attackRange = 25f;

    private float _lastAttackTime = -Mathf.Infinity; // Last time a shot was fired

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

    private void Update()
    {
        try
        {
            if (GameManager.Instance.PlayerGO == null)
            {
                return;
            }

            // Calculate the direction to the target
            Vector3 directionToTarget = GameManager.Instance.PlayerGO.transform.position - _cannonGO.transform.position;

            // Check the distance to the target
            float distanceToTarget = directionToTarget.magnitude;

            if (distanceToTarget <= _attackRange)
            {
                if (Time.time - _lastAttackTime >= _attackRate)
                {
                    _lastAttackTime = Time.time; // Update the last attack time

                    Vector3 directionToEnemy = (GameManager.Instance.PlayerGO.transform.position - transform.position).normalized;

                    GameObject projectile = Instantiate(_projectilePrefab, _cannonPoint.transform.position, Quaternion.identity);
                    Projectile projectileScript = projectile.GetComponent<Projectile>();

                    // Initialize the projectile with direction and range
                    projectileScript.Initialize(directionToEnemy, _attackRange, true);

                    GameObject cannonSmokeEffect = Instantiate(_cannonSmokeEffectPrefab, new Vector3(_cannonPoint.position.x, _cannonPoint.position.y, -1f), Quaternion.identity);
                    Destroy(cannonSmokeEffect, 2f);

                    SoundFXManager.Instance.PlayRandomSoundFXClip(_cannonSounds, transform, 0.5f);

                    _animator.SetTrigger("RedCannonAttack");
                }
            }


        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".LateUpdate: " + ex);
        }
    }

    private void LateUpdate()
    {
        try
        {
            if (GameManager.Instance.PlayerGO == null) return;

            // Calculate the direction to the target
            Vector3 directionToTarget = GameManager.Instance.PlayerGO.transform.position - _cannonGO.transform.position;

            // Check the distance to the target
            float distanceToTarget = directionToTarget.magnitude;

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            // Apply the rotation on the z-axis
            _cannonGO.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".LateUpdate: " + ex);
        }
    }
}
