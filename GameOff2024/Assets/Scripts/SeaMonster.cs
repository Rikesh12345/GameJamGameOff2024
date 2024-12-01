using System;
using UnityEngine;

public class SeaMonster : Unit
{
    [SerializeField]
    private float _attackRate = 1.0f; // Time between shots, in seconds
    [SerializeField]
    private GameObject _tentacleSlamPrefab;
    [SerializeField]
    private GameObject _cannonSmokeEffectPrefab;

    private float _lastAttackTime = -Mathf.Infinity; // Last time a shot was fired

    
    private Vector3 _tentacleSpawnPos;

    [SerializeField]
    private AudioClip[] _cannonSounds;
    [SerializeField]
    private AudioClip[] _shipTakeDamageSounds;

    [Header("WaterEffects")]
    [SerializeField]
    private GameObject _waterSplashEffect;
    [SerializeField]
    private GameObject _waterSinkEffect;

    // Called by child trigger collider
    public void OnEnemyStay(Transform enemyTransform)
    {
        try
        {
            // Only fire if enough time has passed since the last attack
            if (Time.time - _lastAttackTime >= _attackRate)
            {
                _lastAttackTime = Time.time; // Update the last attack time
                Vector3 directionToEnemy = (enemyTransform.position - transform.position).normalized;
                Vector3 directionToKrake = (transform.position - enemyTransform.position).normalized;
                _tentacleSpawnPos = enemyTransform.position + directionToKrake * 3f;

                SlamAttack(_tentacleSpawnPos, directionToEnemy);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnEnemyStay: " + ex);
        }
    }

    private void SlamAttack(Vector3 tentacleSpawnPos, Vector3 directionToEnemy)
    {
        try
        {

            //GameObject projectile = Instantiate(_projectilePrefab, tentacleSpawnTransform.position, Quaternion.identity);
            //Projectile projectileScript = projectile.GetComponent<Projectile>();

            //// Calculate the angle in degrees
            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //// Apply the rotation on the z-axis
            //projectile.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            //// Initialize the projectile with direction and range
            //projectileScript.Initialize(direction, AttackRange);

            //AttackAnimation(false);

            //GameObject cannonSmokeEffect = Instantiate(_cannonSmokeEffectPrefab, new Vector3(enemyTransform.position.x, enemyTransform.position.y, -1f), Quaternion.identity);
            //Destroy(cannonSmokeEffect, 2f);

            //SoundFXManager.Instance.PlayRandomSoundFXClip(_cannonSounds, transform, 0.25f);
            //AttackAnimation(isPortSide);

            GameObject tentacleIns = Instantiate(_tentacleSlamPrefab, tentacleSpawnPos, Quaternion.identity);
            Destroy(tentacleIns, 2.5f);

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;

            // Apply the rotation on the z-axis
            tentacleIns.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SlamAttack: " + ex);
        }
    }

    protected override void PlayTakeDamageSoundFX()
    {
        try
        {

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
            // SeaMonster-specific attack animation logic, e.g., tentacle slam
            _animator.SetTrigger("TentacleSlam");
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
            _waterSplashEffect.SetActive(false);
            _waterSinkEffect.SetActive(true);
            _animator.SetTrigger("Sink");
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".DieAnimation: " + ex);
        }
    }

    public void GrabUnit(Unit target)
    {
        try
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= AttackRange)
            {
                // Logic for grabbing a unit, e.g., immobilizing it
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".GrabUnit: " + ex);
        }
    }

    protected override void DropLoot(SalvageZone salvageZonePrefab, Vector2 position)
    {
        try
        {
            base.DropLoot(salvageZonePrefab, position);

            if (salvageZonePrefab != null && CompareTag("Enemy"))
            {
                GameObjectManager.Instance.SpawnLoot(salvageZonePrefab, transform.position, true);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".DropLoot: " + ex);
        }
    }
}
