using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField]
    private AudioClip _musicClip;
    private AudioSource _currentMusicSource;

    [SerializeField]
    private float _volumeAdjustSpeed = 1f; // Speed of volume change
    private float _originalMusicVolume;

    private Coroutine _volumeCoroutine;

    [SerializeField]
    private float enemyProximityRange = 50f; // Distance to consider an enemy "near"
    public float EnemyProximityRange { get => enemyProximityRange; set => enemyProximityRange = value; }
    [SerializeField]
    private LayerMask enemyLayer; // LayerMask for enemy objects


    private void Start()
    {
        try
        {
            //_musicSource = gameObject.AddComponent<AudioSource>();
            //_musicSource.clip = _musicClip;
            //_originalMusicVolume = _musicSource.volume;
            //SoundFXManager.Instance.PlayLoopingTrimmedSoundFXClip(_musicSource.clip, GameManager.Instance.PlayerGO.transform, 0.25f, 0f, -1f);

            _currentMusicSource = gameObject.AddComponent<AudioSource>();
            _currentMusicSource.clip = _musicClip;
            _currentMusicSource.loop = true;
            _currentMusicSource.volume = 0.25f; // Default volume
            _currentMusicSource.playOnAwake = false;
            _originalMusicVolume = _currentMusicSource.volume;

            _currentMusicSource.Play();

            // Start periodic enemy check
            InvokeRepeating(nameof(CheckEnemyProximity), 0f, 1f);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Start: " + ex);
        }
    }

    private void CheckEnemyProximity()
    {
        bool enemiesNearby = AreEnemiesNearby();

        // Set target volume based on enemy proximity
        float targetVolume = enemiesNearby ? _originalMusicVolume * 0.15f : _originalMusicVolume;

        // Adjust volume gradually
        AdjustMusicVolume(targetVolume);
    }

    private void Update()
    {
        try
        {
            //// Test toggle with keys (replace with actual enemy detection later)
            //if (Input.GetKeyDown(KeyCode.Alpha1)) // Press "1" to simulate enemies nearby
            //{
            //    AdjustMusicVolume(_originalMusicVolume * 0.5f);
            //    Debug.Log("Lowering music volume.");
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha2)) // Press "2" to simulate no enemies nearby
            //{
            //    AdjustMusicVolume(_originalMusicVolume);
            //    Debug.Log("Restoring music volume.");
            //}

            //bool enemiesNear = AreEnemiesNearby();

            //if (enemiesNear)
            //{
            //    AdjustMusicVolume(_originalMusicVolume * 0.5f); // Reduce volume to 50%
            //}
            //else
            //{
            //    AdjustMusicVolume(_originalMusicVolume); // Restore original volume
            //}
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    private void AdjustMusicVolume(float targetVolume)
    {
        // Stop any existing volume adjustment coroutine
        if (_volumeCoroutine != null)
            StopCoroutine(_volumeCoroutine);

        // Start a new volume adjustment coroutine
        _volumeCoroutine = StartCoroutine(AdjustVolumeRoutine(targetVolume));
    }

    private IEnumerator AdjustVolumeRoutine(float targetVolume)
    {
        while (!Mathf.Approximately(_currentMusicSource.volume, targetVolume))
        {
            // Gradually move toward the target volume
            _currentMusicSource.volume = Mathf.MoveTowards(_currentMusicSource.volume, targetVolume, _volumeAdjustSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame
        }
    }

    private bool AreEnemiesNearby()
    {
        Vector3 playerPosition = GameManager.Instance.PlayerGO.transform.position;

        foreach (var enemy in GameObjectManager.Instance.EnemyUnits)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance <= EnemyProximityRange)
            {
                //Debug.Log($"Enemy {enemy.name} is within range: {distance}");
                return true; // Enemy is close enough
            }
        }

        //Debug.Log("No enemies nearby.");
        return false;
    }
}
