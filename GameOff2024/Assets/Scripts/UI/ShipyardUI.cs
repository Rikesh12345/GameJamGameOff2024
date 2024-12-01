using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipyardUI : MonoBehaviour
{
    [SerializeField]
    private Image _unitVisuals;

    [SerializeField]
    private GameObject[] _constructionsGO;

    [SerializeField]
    private GameObject _craneButton;
    private int _craneCost = 5;

    [SerializeField]
    private GameObject _sloopButton;
    private int _sloopCost = 10;

    [SerializeField]
    private TMP_Text _goldCoinsTxt;

    [Header("UpgradeUI")]
    [SerializeField]
    private GameObject[] _upgradeUiGOs;

    [SerializeField]
    private TMP_Text _numberOfCannonsText;
    [SerializeField]
    private Button _cannonsButton;
    [SerializeField]
    private GameObject _cannonsButtonTextGO;
    [SerializeField]
    private GameObject _cannonsMaxTextGO;

    [SerializeField]
    private TMP_Text _sailingSpeedText;
    [SerializeField]
    private Button _sailingSpeedButton;
    [SerializeField]
    private GameObject _sailingSpeedButtonTextGO;
    [SerializeField]
    private GameObject _sailingSpeedMaxTextGO;

    [SerializeField]
    private TMP_Text _armorText;
    [SerializeField]
    private Button _armorButton;
    [SerializeField]
    private GameObject _armorButtonTextGO;
    [SerializeField]
    private GameObject _armorMaxTextGO;

    private int _cannonsUpgradeCost = 15;
    private int _sailingSpeedUpgradeCost = 10;
    private int _armorUpgradeCost = 5;

    [Header("Repair")]
    [SerializeField]
    private Button _repairButton;
    [SerializeField]
    private Button _doneButton;
    [SerializeField]
    private Image _repairProgressBar;
    [SerializeField]
    private float _repairDuration = 3f; // Duration of repair in seconds.
    private bool _isRepairing = false;
    [SerializeField]
    private AudioClip[] _repairSFX;

    //private void Start()
    //{
    //    try
    //    {

    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("ERROR in " + this + ".Start: " + ex);
    //    }
    //}

    private void Update()
    {
        try
        {
            _goldCoinsTxt.text = GameManager.Instance.PlayerCurrency.GoldCoins.ToString();
            UpdateButtons();
            UpdateShipStats();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Start: " + ex);
        }
    }

    private void OnEnable()
    {
        try
        {
            ResetButtons();

            for (int i = 0; i < GameManager.Instance.PlayerCurrency.ConstructionsFound.Length; i++)
            {
                if (GameManager.Instance.PlayerCurrency.ConstructionsFound[i])
                {
                    _constructionsGO[i].SetActive(true);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnEnable: " + ex);
        }
    }

    public void SetUnitVisuals(Sprite sprite)
    {
        try
        {
            _unitVisuals.sprite = sprite;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetUnitVisuals: " + ex);
        }
    }

    public void CraneConstruction()
    {
        try
        {
            if (GameManager.Instance.PlayerCurrency.GoldCoins >= _craneCost)
            {
                SoundFXManager.Instance.PlayRandomSoundFXClip(GameManager.Instance.ButtonClickSound, GameManager.Instance.PlayerGO.transform, 0.5f);

                GameManager.Instance.PlayerCurrency.GoldCoins -= _craneCost;
                _craneButton.SetActive(false);
                GameObjectManager.Instance.SpawnRowboatWithCrane(false);
                Unit unit = GameManager.Instance.PlayerGO.GetComponent<Unit>();
                SetUnitVisuals(unit.UnitVisuals.sprite);
                GameObjectManager.Instance.SalvageZonesCrane.SetActive(true);
                GameObjectManager.Instance.SetGuideArrow(false);
            }            
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".CraneConstruction: " + ex);
        }
    }

    public void SloopConstruction()
    {
        try
        {
            if (GameManager.Instance.PlayerCurrency.GoldCoins >= _sloopCost)
            {
                SoundFXManager.Instance.PlayRandomSoundFXClip(GameManager.Instance.ButtonClickSound, GameManager.Instance.PlayerGO.transform, 0.5f);

                GameManager.Instance.PlayerCurrency.GoldCoins -= _sloopCost;
                _sloopButton.SetActive(false);
                GameObjectManager.Instance.SpawnShipSloop(2);
                Unit unit = GameManager.Instance.PlayerGO.GetComponent<Unit>();
                SetUnitVisuals(unit.UnitVisuals.sprite);
                GameObjectManager.Instance.DisableBayBorder();
                GameObjectManager.Instance.EnableBlackbeardIntroBorder();

                foreach (GameObject GO in _upgradeUiGOs)
                {
                    GO.SetActive(true);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SloopConstruction: " + ex);
        }
    }

    private void ResetButtons()
    {
        try
        {
            _doneButton.interactable = true;
            _repairProgressBar.fillAmount = 0;
            _isRepairing = false;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ResetButtons: " + ex);
        }
    }

    private void UpdateButtons()
    {
        try
        {
            if (GameManager.Instance.PlayerGO != null)
            {
                if (GameManager.Instance.PlayerGO.TryGetComponent<Ship>(out Ship ship))
                {
                    if (ship.NumberOfCannons >= ship.MaxNumberOfCannons)
                    {
                        _cannonsButton.interactable = false;
                        _cannonsButtonTextGO.SetActive(false);                        
                        _cannonsMaxTextGO.SetActive(true);
                    }
                    else
                    {
                        _cannonsButton.interactable = true;
                        _cannonsButtonTextGO.SetActive(true);
                        _cannonsMaxTextGO.SetActive(false);
                    }

                    if (ship.MovementSpeed >= ship.MaxMovementSpeed)
                    {
                        _sailingSpeedButton.interactable = false;
                        _sailingSpeedButtonTextGO.SetActive(false);
                        _sailingSpeedMaxTextGO.SetActive(true);
                    }
                    else
                    {
                        _sailingSpeedButton.interactable = true;
                        _sailingSpeedButtonTextGO.SetActive(true);
                        _sailingSpeedMaxTextGO.SetActive(false);
                    }

                    if (ship.Health >= ship.MaxMaxHealth)
                    {
                        _armorButton.interactable = false;
                        _armorButtonTextGO.SetActive(false);
                        _armorMaxTextGO.SetActive(true);
                    }
                    else
                    {
                        _armorButton.interactable = true;
                        _armorButtonTextGO.SetActive(true);
                        _armorMaxTextGO.SetActive(false);
                    }

                    if (ship.Health == ship.MaxHealth)
                    {
                        _repairButton.interactable = false;
                    }
                    else
                    {
                        _repairButton.interactable = true;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".UpdateButtons: " + ex);
        }
    }

    private void UpdateShipStats()
    {
        try
        {
            if (GameManager.Instance.PlayerGO != null)
            {
                if (GameManager.Instance.PlayerGO.TryGetComponent<Ship>(out Ship ship))
                {
                    _numberOfCannonsText.text = ship.NumberOfCannons.ToString() + "\n" + "Cannons";
                    _sailingSpeedText.text = ship.MovementSpeed.ToString() + "\n" + "Knots";
                    _armorText.text = ship.MaxHealth.ToString() + "\n" + "Armor";
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".UpdateShipStats: " + ex);
        }
    }

    public void CannonsButton()
    {
        try
        {
            if (GameManager.Instance.PlayerGO != null)
            {
                if (GameManager.Instance.PlayerCurrency.GoldCoins >= _cannonsUpgradeCost)
                {
                    SoundFXManager.Instance.PlayRandomSoundFXClip(GameManager.Instance.ButtonClickSound, GameManager.Instance.PlayerGO.transform, 0.5f);

                    GameManager.Instance.PlayerCurrency.GoldCoins -= _cannonsUpgradeCost;

                    Ship ship = GameManager.Instance.PlayerGO.GetComponent<Ship>();

                    if (ship.NumberOfCannons == 2)
                    {
                        GameObjectManager.Instance.SpawnShipSloop(4);
                    }
                    else if (ship.NumberOfCannons == 4)
                    {
                        GameObjectManager.Instance.SpawnShipSloop(6);
                    }
                        
                    SetUnitVisuals(ship.UnitVisuals.sprite);
                }
                
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".CannonsButton: " + ex);
        }
    }

    public void SailingSpeedButton()
    {
        try
        {
            if (GameManager.Instance.PlayerGO != null)
            {
                if (GameManager.Instance.PlayerCurrency.GoldCoins >= _sailingSpeedUpgradeCost)
                {
                    SoundFXManager.Instance.PlayRandomSoundFXClip(GameManager.Instance.ButtonClickSound, GameManager.Instance.PlayerGO.transform, 0.5f);

                    GameManager.Instance.PlayerCurrency.GoldCoins -= _sailingSpeedUpgradeCost;

                    Ship ship = GameManager.Instance.PlayerGO.GetComponent<Ship>();

                    ship.MovementSpeed += 0.5f;

                    SetUnitVisuals(ship.UnitVisuals.sprite);
                }

            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SailingSpeedButton: " + ex);
        }
    }

    public void ArmorButton()
    {
        try
        {
            if (GameManager.Instance.PlayerGO != null)
            {
                if (GameManager.Instance.PlayerCurrency.GoldCoins >= _armorUpgradeCost)
                {
                    SoundFXManager.Instance.PlayRandomSoundFXClip(GameManager.Instance.ButtonClickSound, GameManager.Instance.PlayerGO.transform, 0.5f);

                    GameManager.Instance.PlayerCurrency.GoldCoins -= _armorUpgradeCost;

                    Ship ship = GameManager.Instance.PlayerGO.GetComponent<Ship>();

                    ship.IncreaseMaxHealth(1);

                    SetUnitVisuals(ship.UnitVisuals.sprite);
                }

            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".CannonsButton: " + ex);
        }
    }

    public void RepairButton()
    {
        try
        {
            if (_isRepairing) return; // Prevent multiple repair calls.

            if (GameManager.Instance.PlayerGO != null)
            {
                Unit unit = GameManager.Instance.PlayerGO.GetComponent<Unit>();

                if (unit.Health < unit.MaxHealth)
                {
                    SoundFXManager.Instance.PlayRandomSoundFXClip(GameManager.Instance.ButtonClickSound, GameManager.Instance.PlayerGO.transform, 0.5f);
                    SoundFXManager.Instance.PlayRandomSoundFXClip(_repairSFX, GameManager.Instance.PlayerGO.transform, 0.5f);
                    StartCoroutine(RepairShip(unit));
                }
                else
                {
                    Debug.Log("Ship is already at max health.");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".RepairButton: " + ex);
        }
    }

    private IEnumerator RepairShip(Unit unit)
    {
        _isRepairing = true;
        _doneButton.interactable = false; // Disable button during repair.
        _repairProgressBar.fillAmount = 0;

        float elapsedTime = 0f;

        while (elapsedTime < _repairDuration)
        {
            elapsedTime += Time.deltaTime;
            _repairProgressBar.fillAmount = Mathf.Clamp01(elapsedTime / _repairDuration);
            yield return null;
        }

        // Complete the repair.
        unit.SetHealthToMax();
        _repairProgressBar.fillAmount = 0;

        _isRepairing = false;
        _doneButton.interactable = true; // Re-enable the button.
    }

    public void DoneButton()
    {
        try
        {
            SoundFXManager.Instance.PlayRandomSoundFXClip(GameManager.Instance.ButtonClickSound, GameManager.Instance.PlayerGO.transform, 0.5f);

            gameObject.SetActive(false);
            GameObjectManager.Instance.SetUiElements(true);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OkButton: " + ex);
        }
    }

    public void OnDisable()
    {
        try
        {
            GameObjectManager.Instance.SetUiElements(true);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnDisable: " + ex);
        }
    }
}
