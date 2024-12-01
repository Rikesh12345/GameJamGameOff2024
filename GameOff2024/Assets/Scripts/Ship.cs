using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Unit
{
    [SerializeField]
    private float _attackRate = 1.0f; // Time between shots, in seconds
    [SerializeField]
    private GameObject _projectilePrefab;
    [SerializeField]
    private GameObject _cannonSmokeEffectPrefab;
    [SerializeField]
    private List<Transform> _portCannonPoints;       // List of spawn points on the port side
    [SerializeField]
    private List<Transform> _starboardCannonPoints;  // List of spawn points on the starboard side

    [SerializeField]
    private int _numberOfCannons;
    public int NumberOfCannons { get => _numberOfCannons; set => _numberOfCannons = value; }

    [SerializeField]
    private int _maxNumberOfCannons = 6;
    public int MaxNumberOfCannons { get => _maxNumberOfCannons; set => _maxNumberOfCannons = value; }

    private float _lastAttackTime = -Mathf.Infinity; // Last time a shot was fired

    [SerializeField]
    private AudioClip[] _cannonSounds;
    [SerializeField]
    private AudioClip[] _shipTakeDamageSounds;

    [Header("WaterEffects")]
    [SerializeField]
    private GameObject _waterSplashEffect;

    [Header("PowderKeg")]
    [SerializeField]
    private Transform _powderKegSpawnPoint;


    protected override void Start()
    {
        try
        {
            base.Start();

            if (CompareTag("Player"))
            {
                GameManager.Instance.EnableAmbienteSail();
                GameManager.Instance.gameObject.GetComponent<PowderKegUI>().SpawnPoint = _powderKegSpawnPoint;
            }

        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Start: " + ex);
        }
    }

    protected override void Update()
    {
        try
        {
            base.Update();           
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    // Called by child trigger colliders when an enemy stays in the trigger
    public void OnEnemyStay(Transform enemyTransform, bool isPortSide)
    {
        try
        {
            // Only fire if enough time has passed since the last attack
            if (Time.time - _lastAttackTime >= _attackRate)
            {
                _lastAttackTime = Time.time; // Update the last attack time
                Vector3 directionToEnemy = (enemyTransform.position - transform.position).normalized;
                List<Transform> cannonPoints = isPortSide ? _portCannonPoints : _starboardCannonPoints;

                FireCannons(cannonPoints, directionToEnemy, isPortSide);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnEnemyStay: " + ex);
        }
    }

    private void FireCannons(List<Transform> cannonPoints, Vector3 direction, bool isPortSide)
    {
        try
        {

            foreach (Transform cannonPoint in cannonPoints)
            {
                GameObject projectile = Instantiate(_projectilePrefab, cannonPoint.position, Quaternion.identity);
                Projectile projectileScript = projectile.GetComponent<Projectile>();

                // Initialize the projectile with direction and range
                projectileScript.Initialize(direction, AttackRange);

                GameObject cannonSmokeEffect = Instantiate(_cannonSmokeEffectPrefab, new Vector3(cannonPoint.position.x, cannonPoint.position.y, -1f), Quaternion.identity);
                Destroy(cannonSmokeEffect, 2f);
            }
            if (cannonPoints.Count > 0)
            {
                SoundFXManager.Instance.PlayRandomSoundFXClip(_cannonSounds, transform, 0.25f);
                AttackAnimation(isPortSide);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".FireCannons: " + ex);
        }
    }

    protected override void PlayTakeDamageSoundFX()
    {
        try
        {
            SoundFXManager.Instance.PlayRandomSoundFXClip(_shipTakeDamageSounds, transform, 0.15f);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".PlayTakeDamageSoundFX: " + ex);
        }
    }

    protected override void AttackAnimation(bool isPortSide)
    {
        try
        {
            // Trigger the correct animation based on the side
            if (isPortSide)
            {
                _animator.SetTrigger("PortAttack");
            }
            else
            {
                _animator.SetTrigger("StarboardAttack");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".AttackAnimation: " + ex);
        }
    }


    protected override void DieAnimation()
    {
        try
        {
            // Ship-specific death animation logic
            _waterSplashEffect.SetActive(false);
            _animator.SetTrigger("Sink");
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".DieAnimation: " + ex);
        }
    }

    // called through sink animation
    public void RespawnPlayerShip()
    {
        try
        {
            //if (CompareTag("Player") && !_isNPC)
            //{
            //    GameObjectManager.Instance.RespawnPlayerShip(NumberOfCannons);
            //    GameObjectManager.Instance.SetUiElements(true);
            //}
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".RespawnPlayerShip: " + ex);
        }
    }

    protected override void DropLoot(SalvageZone salvageZonePrefab, Vector2 position)
    {
        try
        {
            base.DropLoot(salvageZonePrefab, position);

            if (salvageZonePrefab != null && CompareTag("Enemy"))
            {
                if (_isBlackbeared)
                {

                    GameObjectManager.Instance.SpawnLoot(salvageZonePrefab, transform.position, false, true);
                }
                else
                {
                    GameObjectManager.Instance.SpawnLoot(salvageZonePrefab, transform.position);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".DropLoot: " + ex);
        }
    }

    protected override void OnDisable()
    {
        try
        {
            base.OnDisable();

            if (GameManager.Instance.IsQuitting)
            {
                return;
            }

            if (CompareTag("Player") && Dead)
            {
                GameManager.Instance.DisableAmbienteSail();

                if (CompareTag("Player") && !_isNPC)
                {
                    GameObjectManager.Instance.RespawnPlayerShip(NumberOfCannons);
                    GameObjectManager.Instance.SetUiElements(true);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnDisable: " + ex);
        }
    }
}
