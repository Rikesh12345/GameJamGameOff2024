using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _lootWindowText;

    private int _goldCoins;
    [SerializeField]
    private GameObject _goldCoinsGO;
    [SerializeField]
    private TMP_Text _goldCoinsText;
    [SerializeField]
    private GameObject _treasureGO;

    [SerializeField]
    private GameObject _constructionGO;
    [SerializeField]
    private Image _constructionImage;
    [SerializeField]
    private TMP_Text _connstructionDescriptionText;

    [SerializeField]
    private GameObject upgradeGO;
    [SerializeField]
    private Image _upgradeImage;
    [SerializeField]
    private TMP_Text _upgradeDescriptionText;
    [SerializeField]
    private TMP_Text _upgradeText;

    [SerializeField]
    private GameObject _goldCoinIncomeUIPrefab;

    [SerializeField]
    private Button _okButton;

    [SerializeField]
    private AudioClip[] _chestOpeningSound;

    private void Start()
    {
        try
        {
            _okButton.interactable = false;
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
            if (GameManager.Instance.PlayerUnit.Dead)
            {
                gameObject.SetActive(false);
            }
            if (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame)
            {
                if (_okButton.interactable)
                {
                    CloseWindow();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    public void EnableButton()
    {
        try
        {
            _okButton.interactable = true;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".EnableButton: " + ex);
        }
    }


    public void SetupLootWindow(int goldCoins, Sprite constructionSprite, string connstructionDescriptionText, Sprite upgradeSprite, string upgradeDescriptionText, bool finalLoot)
    {
        try
        {
            ResetLootWindow();

            if (finalLoot)
            {
                _upgradeImage.sprite = upgradeSprite;
                upgradeGO.SetActive(true);
                _lootWindowText.text = "Alice saved";
                _upgradeText.text = "";
                _upgradeDescriptionText.text = "Alice";
                return;
            }

            _goldCoins = goldCoins;

            if (goldCoins > 0)
            {
                _goldCoinsText.text = _goldCoins + "x Gold coins: ";
                _lootWindowText.text = "Gold found";
                _goldCoinsGO.SetActive(true);
            }

            if (constructionSprite != null)
            {
                _constructionImage.sprite = constructionSprite;
                _constructionGO.SetActive(true);
                _treasureGO.SetActive(true);
                _lootWindowText.text = "Treasure found";
                _connstructionDescriptionText.text = connstructionDescriptionText;
            }

            if (upgradeSprite != null)
            {
                _upgradeImage.sprite = upgradeSprite;
                upgradeGO.SetActive(true);
                _lootWindowText.text = "Item found";
                _upgradeDescriptionText.text = upgradeDescriptionText;
                //_treasureGO.SetActive(true);
                //_lootWindowText.text = "Treasure found";
            }

            //if (constructionSprite == null && upgradeSprite == null)
            //{
            //    _treasureGO.SetActive(false);
            //}
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetupLootWindow: " + ex);
        }
    }

    private void ResetLootWindow()
    {
        try
        {
            _goldCoinsGO.SetActive(false);
            _constructionGO.SetActive(false);
            _treasureGO.SetActive(false);
            upgradeGO.SetActive(false);
            _constructionImage.sprite = null;
            _upgradeImage.sprite = null;
            _connstructionDescriptionText.text = "";
            _upgradeDescriptionText.text = "";
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".CloseWindow: " + ex);
        }
    }

    public void ShowLootWindow()
    {
        try
        {
            if (_constructionImage.sprite != null || _upgradeImage.sprite != null)
            {
                GameObjectManager.Instance.SetUiElements(false);
                gameObject.SetActive(true);
            }
            else
            {
                AddGoldCoins();
                _okButton.interactable = false;
            }
            
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ShowLootWindow: " + ex);
        }
    }

    private void AddGoldCoins()
    {
        try
        {
            if (_goldCoins > 0)
            {
                GameObject goldCoinIncomeUIIns = Instantiate(_goldCoinIncomeUIPrefab, GameManager.Instance.PlayerGO.transform.position, _goldCoinIncomeUIPrefab.transform.rotation);
                goldCoinIncomeUIIns.transform.Find("GoldIncomeTxt").GetComponent<TMP_Text>().text = "+ " + _goldCoins.ToString();
                Destroy(goldCoinIncomeUIIns, 3f);

                GameManager.Instance.PlayerCurrency.GoldCoins += _goldCoins;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".AddGoldCoins: " + ex);
        }
    }

    public void CloseWindow()
    {
        try
        {
            SoundFXManager.Instance.PlayRandomSoundFXClip(GameManager.Instance.ButtonClickSound, GameManager.Instance.PlayerGO.transform, 0.5f);

            GameObjectManager.Instance.SetUiElements(true);
            gameObject.SetActive(false);
            //GameManager.Instance.ResumeGame();

            AddGoldCoins();
            _okButton.interactable = false;

            if (_constructionImage.sprite != null)
            {
                for (int i = 0; i < GameManager.Instance.PlayerCurrency.ConstructionsFound.Length; i++)
                {
                    if (!GameManager.Instance.PlayerCurrency.ConstructionsFound[i])
                    {
                        GameManager.Instance.PlayerCurrency.ConstructionsFound[i] = true;
                        Debug.Log("Construction added: " + i + " pos");
                        break;
                    }
                }             
            }

            if (_treasureGO.activeSelf)
            {
                Unit unit = GameManager.Instance.PlayerGO.GetComponent<Unit>();
                unit.SetTreasure(true);
            }

            if (_upgradeDescriptionText.text.Contains("Map"))
            {
                GameManager.Instance.EnableWorldMap();
            }
            if (_upgradeDescriptionText.text.Contains("Powder Keg"))
            {
                GameManager.Instance.EnablePowderKeg();
            }

            if (GameManager.Instance.GameEnd)
            {
                GameManager.Instance.SetupOutro();
            }

            GameObjectManager.Instance.SetRowingText(true);
            //Debug.LogWarning("Rowing outcommented!");

        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".CloseWindow: " + ex);
        }
    }

    public void PlayChestOpeningSoundFX()
    {
        try
        {
            if (_treasureGO.activeSelf)
            {
                SoundFXManager.Instance.PlayRandomSoundFXClip(_chestOpeningSound, GameManager.Instance.PlayerGO.transform, 0.5f);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".PlayChestOpeningSoundFX: " + ex);
        }
    }
}
