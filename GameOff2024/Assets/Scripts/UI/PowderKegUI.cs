using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowderKegUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _powderKegPrefab; // Reference to the powder keg prefab.

    private Transform _spawnPoint; // Point behind the player's ship where the keg will spawn.
    public Transform SpawnPoint { get => _spawnPoint; set => _spawnPoint = value; }

    [SerializeField]
    private Image _cooldownFillImage; // UI element to show cooldown.

    [SerializeField]
    private float _cooldownDuration = 20f; // Cooldown in seconds.

    private bool _isCooldown = false;

    private bool _firstTimeActivated = false;

    public bool FirstTimeActivated { get => _firstTimeActivated; set => _firstTimeActivated = value; }

    [SerializeField]
    private AudioClip[] _kegIntoWaterSFX;


    private void Update()
    {
        try
        {
            if (Input.GetKeyDown(KeyCode.E) && !_isCooldown && GameManager.Instance.HasPowderKeg)
            {
                ThrowPowderKeg();

                if (!FirstTimeActivated)
                {
                    FirstTimeActivated = true;
                    GameObjectManager.Instance.SetPowderKegControlsText(false);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    private void ThrowPowderKeg()
    {
        try
        {
            if (SpawnPoint == null)
            {
                SpawnPoint = GameManager.Instance.PlayerGO.transform;
            }
            GameObject powderKegIns = Instantiate(_powderKegPrefab, SpawnPoint.position, Quaternion.identity);
            SoundFXManager.Instance.PlayRandomSoundFXClip(_kegIntoWaterSFX, SpawnPoint, 0.35f);
            Destroy(powderKegIns, 20f);
            StartCoroutine(StartCooldown());
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ThrowPowderKeg: " + ex);
        }
    }

    private IEnumerator StartCooldown()
    {
        _isCooldown = true;
        float elapsedTime = 0f;

        _cooldownFillImage.fillAmount = 0; // Start with an empty fill.

        while (elapsedTime < _cooldownDuration)
        {
            elapsedTime += Time.deltaTime;
            _cooldownFillImage.fillAmount = elapsedTime / _cooldownDuration; // Update the fill amount.
            yield return null;
        }

        _cooldownFillImage.fillAmount = 1; // Fully filled after cooldown.
        _isCooldown = false;
    }
}
