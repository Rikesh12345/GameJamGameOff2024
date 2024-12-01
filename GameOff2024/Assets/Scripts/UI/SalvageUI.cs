using System;
using UnityEngine;
using UnityEngine.UI;

public class SalvageUI : MonoBehaviour
{
    [SerializeField]
    private Image _salvageFillImage;
    [SerializeField]
    private float _decayRate = 0.05f;
    private float _currentFill = 0f;
    public bool IsSalvaging { get; set; } = false;

    private GameObject _salvageZoneGO;
    public GameObject SalvageZoneGO { get => _salvageZoneGO; set => _salvageZoneGO = value; }

    private SalvageZone _salvageZone;
    public SalvageZone SalvageZone { get => _salvageZone; set => _salvageZone = value; }

    private bool _firstTimeSalvaging = true;

    private void Start()
    {
        try
        {
            if (_salvageFillImage == null)
            {
                Debug.LogError("SalvageUI: Salvage fill image is not assigned!");
            }
            ResetUI();
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
            if (!IsSalvaging && _currentFill > 0f)
            {
                // Use the decay rate of the current salvage zone
                float decayRate = SalvageZone != null ? SalvageZone.DecayRate : _decayRate;

                // Decay the bar
                _currentFill -= decayRate * Time.deltaTime;
                _salvageFillImage.fillAmount = Mathf.Clamp01(_currentFill);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    public void UpdateFill(float amount)
    {
        try
        {
            _currentFill += amount;
            _currentFill = Mathf.Clamp01(_currentFill);
            _salvageFillImage.fillAmount = _currentFill;

            if (_currentFill >= 1f)
            {
                if (_firstTimeSalvaging && !GameManager.Instance.SkipTutorial)
                {
                    CollectIntroTreasure();
                    _firstTimeSalvaging = false;
                }
                else
                {
                    CollectTreasure();
                }

                ResetUI();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".UpdateFill: " + ex);
        }
    }

    private void CollectIntroTreasure()
    {
        try
        {
            Debug.Log("Treasure collected!");

            Rowboat rowboat = GameManager.Instance.PlayerGO.GetComponent<Rowboat>();

            rowboat.StopFishing();
            rowboat.CollectIntroTreasure();
            GameObjectManager.Instance.DestroySalvageZone(SalvageZoneGO);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".CollectTreasure: " + ex);
        }
    }

    private void CollectTreasure()
    {
        try
        {
            Debug.Log("Treasure collected!");

            Unit unit = GameManager.Instance.PlayerGO.GetComponent<Unit>();
            unit.CollectTreasure();

            GameObjectManager.Instance.DestroySalvageZone(SalvageZoneGO);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".CollectTreasure: " + ex);
        }
    }

    public void ResetUI()
    {
        try
        {
            _currentFill = 0f;
            _salvageFillImage.fillAmount = 0f;
            GameManager.Instance.SalvageUIGO.SetActive(false);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ResetUI: " + ex);
        }
    }
}
