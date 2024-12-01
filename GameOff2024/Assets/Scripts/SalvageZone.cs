using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SalvageZone : MonoBehaviour
{
    [SerializeField]
    private int _goldCoins;
    public int GoldCoins { get => _goldCoins; set => _goldCoins = value; }

    [SerializeField]
    private Sprite _constructionSprite;
    public Sprite ConstructionSprite { get => _constructionSprite; set => _constructionSprite = value; }
    [SerializeField]
    private string _constructionDescriptionText;
    public string ConnstructionDescriptionText { get => _constructionDescriptionText; set => _constructionDescriptionText = value; }

    [SerializeField]
    private Sprite _upgradeSprite;
    public Sprite UpgradeSprite { get => _upgradeSprite; set => _upgradeSprite = value; }
    [SerializeField]
    private string _upgradeDescriptionText;
    public string UpgradeDescriptionText { get => _upgradeDescriptionText; set => _upgradeDescriptionText = value; }

    [SerializeField]
    private GameObject _salvageIconGO;

    [Header("Salvage Mechanics")]
    [SerializeField] 
    private float _fillRate = 0.1f; // How much the bar fills per spacebar tap
    public float FillRate { get => _fillRate; set => _fillRate = value; }

    [SerializeField] 
    private float _decayRate = 0.05f; // How quickly the bar decreases
    public float DecayRate { get => _decayRate; set => _decayRate = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision.CompareTag("Player"))
            {
                _salvageIconGO.SetActive(false);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerStay2D: " + ex);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        try
        {
            if (collision.CompareTag("Player"))
            {
                _salvageIconGO.SetActive(true);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerExit2D: " + ex);
        }
    }

}
