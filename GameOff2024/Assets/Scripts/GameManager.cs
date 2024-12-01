using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private AudioClip _ambienteOcean;
    [SerializeField]
    private AudioClip _ambienteSail;

    [SerializeField]
    private GameObject _waterShader;
    [SerializeField]
    private GameObject _globalVolume;

    private bool _isSailing;

    [SerializeField]
    private GameObject _playerGO;
    public GameObject PlayerGO
    { 
        get => _playerGO;

        set
        {
            try
            {
                _playerGO = value;
                PlayerUnit = _playerGO.GetComponent<Unit>();
            }
            catch (Exception ex)
            {
                Debug.LogError("ERROR in " + this + ".PlayerGO: " + ex);
            }
        } 
    }
    private Unit _playerUnit;
    public Unit PlayerUnit { get => _playerUnit; set => _playerUnit = value; }
    private PlayerController _playerController;
    public PlayerController PlayerController { get => _playerController; set => _playerController = value; }
    [SerializeField]
    private CinemachineVirtualCamera _Vcam;
    public CinemachineVirtualCamera Vcam { get => _Vcam; set => _Vcam = value; }
    [SerializeField]
    private CinemachineVirtualCamera _outroVcam;
    public CinemachineVirtualCamera OutroVcam { get => _outroVcam; set => _outroVcam = value; }

    // UI Elements

    [SerializeField]
    private GameObject _salvageGO;
    public GameObject SalvageUIGO { get => _salvageGO; set => _salvageGO = value; }
    [SerializeField]
    private SalvageUI _salvageUI;
    public SalvageUI SalvageUI { get => _salvageUI; set => _salvageUI = value; }

    [SerializeField]
    private LootWindow _lootWindowUI;

    [Header("Skip Tutorial")]
    [SerializeField]
    private bool _skipTutorial;
    public bool SkipTutorial { get => _skipTutorial; set => _skipTutorial = value; }
    [SerializeField]
    private GameObject _introTimeline;

    [Header("PlayerCurrency")]
    [SerializeField]
    private PlayerCurrency _playerCurrency;
    public PlayerCurrency PlayerCurrency { get => _playerCurrency; set => _playerCurrency = value; }

    [Header("Items")]
    [SerializeField]
    private bool _hasWorldMap;
    public bool HasWorldMap { get => _hasWorldMap; private set => _hasWorldMap = value; }
    [SerializeField]
    private bool _hasPowderKeg;
    public bool HasPowderKeg { get => _hasPowderKeg; private set => _hasPowderKeg = value; }

    [Header("SailingMode")]
    private bool _hasSailingMode;
    public bool HasSailingMode { get => _hasSailingMode; set => _hasSailingMode = value; }

    [Header("WorldMap")]
    [SerializeField]
    private GameObject _worldMapButtonGO;
    [SerializeField]
    private GameObject _worldMapParticleEffectGO;

    [Header("PowderKeg")]
    [SerializeField]
    private GameObject _powderKegButtonGO;
    [SerializeField]
    private GameObject _powderKegParticleEffectGO;

    [Header("PlayerHealthUI")]
    [SerializeField]
    private PlayerHealthUI _healthUI;
    public PlayerHealthUI HealthUI { get => _healthUI; private set => _healthUI = value; }

    [Header("UI Sound")]
    [SerializeField]
    private AudioClip[] _buttonClickSound;
    public AudioClip[] ButtonClickSound { get => _buttonClickSound; set => _buttonClickSound = value; }

    [Header("ApplicationQuit")]
    [SerializeField]
    private bool _isQuitting = false;
    public bool IsQuitting { get => _isQuitting; private set => _isQuitting = value; }

    [Header("PlayerAtShipyard")]
    [SerializeField]
    private bool _isPlayerAtShipyard;
    public bool IsPlayerAtShipyard { get => _isPlayerAtShipyard; set => _isPlayerAtShipyard = value; }

    [Header("Game Over")]
    private bool _gameEnd;
    public bool GameEnd { get => _gameEnd; set => _gameEnd = value; }

    private void Awake()
    {
        try
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Awake: " + ex);
        }
    }

    private void Start()
    {
        try
        {
            //_playerController = _playerGO.GetComponent<PlayerController>();

            StartGame();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Start: " + ex);
        }
    }

    private void StartGame()
    {
        try
        {
            if (!SkipTutorial)
            {
                _introTimeline.SetActive(true);
            }
            else
            {
                PlayerCurrency.GoldCoins = 100;
                EnableWorldMap();
                GameObjectManager.Instance.SetWorldMapControlsText(false);
                EnablePowderKeg();
                GameObjectManager.Instance.SetPowderKegControlsText(false);
                PlayerGO.GetComponent<Unit>().InitializeUnit();
                GameObjectManager.Instance.DisableBayBorder();
                GameObjectManager.Instance.SpawnShipSloop(6);
                //GameObjectManager.Instance.CheckAndSpawnEnemies();
            }

            _waterShader.SetActive(true);
            _globalVolume.SetActive(true);

            SoundFXManager.Instance.PlayLoopingTrimmedSoundFXClip(_ambienteOcean, transform, 0.5f, 0f);         
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".StartGame: " + ex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableAmbienteSail()
    {
        try
        {
            if (!_isSailing)
            {
                SoundFXManager.Instance.PlayLoopingTrimmedSoundFXClip(_ambienteSail, transform, 0.2f, 0f);
                _isSailing = true;
            }            
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".StartAmbienteSail: " + ex);
        }
    }

    public void DisableAmbienteSail()
    {
        try
        {
            if (_isSailing)
            {
                // Disable Sound!            
                _isSailing = false;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".StartAmbienteSail: " + ex);
        }
    }

    public void SetupLootWindow(int goldCoins, Sprite constructionSprite, string connstructionDescriptionText, Sprite upgradeSprite, string upgradeDescriptionText)
    {
        try
        {
            _lootWindowUI.SetupLootWindow(goldCoins, constructionSprite, connstructionDescriptionText, upgradeSprite, upgradeDescriptionText, GameEnd);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetupLootWindow: " + ex);
        }
    }

    public void ShowLootWindow()
    {
        try
        {            
            _lootWindowUI.ShowLootWindow();
            //PauseGame();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ShowLootWindow: " + ex);
        }
    }

    private void PauseGame()
    {
        try
        {
            Time.timeScale = 0;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".PauseGame: " + ex);
        }
    }

    public void ResumeGame()
    {
        try
        {
            Time.timeScale = 1;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".PauseGame: " + ex);
        }
    }

    public void EnableWorldMap()
    {
        try
        {
            HasWorldMap = true;
            _worldMapButtonGO.SetActive(true);
            _worldMapButtonGO.GetComponent<Animator>().SetTrigger("EnableMap");
            StartCoroutine(ShowUiParticleEffect(_worldMapParticleEffectGO));
            GameObjectManager.Instance.SetWorldMapControlsText(true);
            
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".EnableWorldMap: " + ex);
        }
    }

    public void EnablePowderKeg()
    {
        try
        {
            HasPowderKeg = true;
            _powderKegButtonGO.SetActive(true);
            _powderKegButtonGO.GetComponent<Animator>().SetTrigger("EnableMap");
            StartCoroutine(ShowUiParticleEffect(_powderKegParticleEffectGO));
            GameObjectManager.Instance.SetPowderKegControlsText(true);

        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".EnableWorldMap: " + ex);
        }
    }

    IEnumerator ShowUiParticleEffect(GameObject GO)
    {
        GO.SetActive(true);

        yield return new WaitForSeconds(3);

        GO.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        try
        {
            IsQuitting = true;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnApplicationQuit: " + ex);
        }
    }

    public void GameOver()
    {
        try
        {
            GameEnd = true;

            int numberOfEnemies = GameObjectManager.Instance.EnemyUnits.Count;
            List<Unit> enemyUnits = GameObjectManager.Instance.EnemyUnits;

            for (int i = numberOfEnemies - 1; i >= 0; i--)
            {
                GameObjectManager.Instance.EnemyUnits[i].TakeDamage(100);
            }



        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".GameOver: " + ex);
        }
    }

    public void SetupOutro()
    {
        try
        {
            PlayerController.DisableMovement();
            PlayerGO.transform.position = new Vector3(95f, 470f, 0f);
            PlayerGO.transform.rotation = Quaternion.Euler(0, 0, 0);
            OutroVcam.Follow = PlayerGO.transform;

            GameObjectManager.Instance.OutroGO.SetActive(true);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".GameOver: " + ex);
        }
    }
}
