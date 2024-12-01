using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    public static GameObjectManager Instance { get; private set; }

    [Header("PlayerShips")]
    [SerializeField]
    private GameObject _playerRowboatPrefab;
    [SerializeField]
    private GameObject _playerRowboatWithCranePrefab;
    [SerializeField]
    private GameObject _playerShipSloop2CannonsPrefab;
    [SerializeField]
    private GameObject _playerShipSloop4CannonsPrefab;
    [SerializeField]
    private GameObject _playerShipSloop6CannonsPrefab;

    [Header("EnemyShips")]
    [SerializeField]
    private GameObject _enemyShipSloop2CannonsPrefab;
    [SerializeField]
    private GameObject _enemyShipSloop4CannonsPrefab;
    [SerializeField]
    private GameObject _enemyShipSloop6CannonsPrefab;
    [SerializeField]
    private GameObject _blackbeardShipPrefab;
    private List<Unit> _enemyUnits = new List<Unit>();
    public List<Unit> EnemyUnits { get => _enemyUnits; private set => _enemyUnits = value; }
    private Transform _playerTransform;

    [SerializeField]
    private List<GameObject> _salvageZones = new List<GameObject>();

    [SerializeField]
    private GameObject _introSalvageZone;

    [SerializeField]
    private GameObject _salvageZonesCrane;
    public GameObject SalvageZonesCrane { get => _salvageZonesCrane; set => _salvageZonesCrane = value; }

    [Header("MapGameObjects")]
    [SerializeField]
    private GameObject[] _mapGameObjects;

    [Header("Borders")]
    [SerializeField]
    private GameObject _bayBorder;
    [SerializeField]
    private GameObject _blackbeardIntroBorder;

    [Header("PlayerSpawnPoints")]
    [SerializeField]
    private Transform[] _playerSpawnPoints;
    public Transform[] PlayerSpawnPoints { get => _playerSpawnPoints; set => _playerSpawnPoints = value; }

    [Header("EnemySpawnPoints")]
    [SerializeField]
    private Transform[] _enemySpawnPoints;
    public Transform[] EnemySpawnPoints { get => _enemySpawnPoints; set => _enemySpawnPoints = value; }

    [Header("UI Elements")]
    [SerializeField]
    private GameObject _uiElements;
    [SerializeField]
    private GameObject _fishingTextGO;
    [SerializeField]
    private GameObject _rowingTextGO;
    [SerializeField]
    private GameObject _controlsTutorialGO;
    [SerializeField]
    private GameObject _guideArrowGo;
    [SerializeField]
    private GameObject _worldMapControlslGO;
    [SerializeField]
    private GameObject _powderKegControlslGO;
    [SerializeField]
    private GameObject _sailingModeControlslGO;

    [Header("Outro")]
    [SerializeField]
    private GameObject _outroTimelineGO;
    public GameObject OutroGO { get => _outroTimelineGO; set => _outroTimelineGO = value; }

    private SemaphoreSlim _lockMultiStarts = new SemaphoreSlim(1);

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
            foreach (GameObject GO in _mapGameObjects)
            {
                GO.SetActive(true);
            }

            //// Assume player has a tag "Player" for finding their transform
            //GameObject player = GameObject.FindGameObjectWithTag("Player");
            //if (player != null)
            //{
            //    _playerTransform = player.transform;
            //}

            //// Check for enemies and spawn one if needed
            //CheckAndSpawnEnemies();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Start: " + ex);
        }
    }

    public void RegisterEnemy(Unit enemy)
    {
        try
        {
            if (!EnemyUnits.Contains(enemy))
            {
                EnemyUnits.Add(enemy);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".RegisterEnemy: " + ex);
        }
    }

    public void UnregisterEnemy(Unit enemy)
    {
        try
        {
            if (EnemyUnits.Contains(enemy))
            {
                EnemyUnits.Remove(enemy);

                // If there are no enemies left, spawn a new one
                CheckAndSpawnEnemies();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".UnregisterEnemy: " + ex);
        }
    }

    

    public void CheckAndSpawnEnemies()
    {
        if (_lockMultiStarts.Wait(0))
        {
            try
            {
                if (GameManager.Instance.IsPlayerAtShipyard || GameManager.Instance.GameEnd)
                {
                    return;
                }

                StartCoroutine(WaitBeforeSpawning());
            }
            catch (Exception ex)
            {
                Debug.LogError("ERROR in " + this + ".CheckAndSpawnEnemies: " + ex);
            }
            finally
            {
                _lockMultiStarts.Release(1);
            }
        }
    }

    private IEnumerator WaitBeforeSpawning()
    {
        int randomValue = UnityEngine.Random.Range(5, 10);
        yield return new WaitForSeconds(randomValue);
        Debug.Log(randomValue);

        if (EnemyUnits.Count <= 3)
        {
            int j = 1;

            if (Probability(20))
            {
                j = 3;
            }
            else if (Probability(40))
            {
                j = 2;
            }
            for (int i = 0; i < j; i++)
            {
                SpawnRandomEnemies();
            }

            //SpawnEnemyShip(_enemyShipSloop2CannonsPrefab);
            //SpawnBlackbeardShip();
        }
        
    }

    private void SpawnRandomEnemies()
    {
        try
        {
            if (Probability(20))
            {
                SpawnEnemyShip(_enemyShipSloop6CannonsPrefab);
            }
            else if (Probability(40))
            {
                SpawnEnemyShip(_enemyShipSloop4CannonsPrefab);
            }
            else
            {
                SpawnEnemyShip(_enemyShipSloop2CannonsPrefab);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SpawnRandomEnemies: " + ex);
        }
    }

    public bool Probability(float percentage)
    {
        try
        {
            // Clamp the percentage to valid range (0-100) to avoid unexpected results
            percentage = Mathf.Clamp(percentage, 0f, 100f);

            // Generate a random value between 0 and 100
            float randomValue = UnityEngine.Random.Range(0f, 100f);

            // Return true if the random value is less than the percentage
            return randomValue < percentage;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Probability: " + ex);
            return false;
        }

    }

    private void SpawnEnemyShip(GameObject enemyShipGO)
    {
        try
        {
            // Get all enemy spawn points
            Transform[] spawnPoints = EnemySpawnPoints;

            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                Debug.LogError("No enemy spawn points available!");
                return;
            }

            // Get the player's position
            Transform playerTransform = GameManager.Instance.PlayerGO.transform;

            // Sort spawn points by distance to the player
            Transform nearestSpawnPoint = null;
            Transform secondNearestSpawnPoint = null;

            float shortestDistance = Mathf.Infinity;
            float secondShortestDistance = Mathf.Infinity;

            foreach (var spawnPoint in spawnPoints)
            {
                float distance = Vector3.Distance(playerTransform.position, spawnPoint.position);

                if (distance < shortestDistance)
                {
                    secondShortestDistance = shortestDistance;
                    secondNearestSpawnPoint = nearestSpawnPoint;

                    shortestDistance = distance;
                    nearestSpawnPoint = spawnPoint;
                }
                else if (distance < secondShortestDistance)
                {
                    secondShortestDistance = distance;
                    secondNearestSpawnPoint = spawnPoint;
                }
            }

            if (nearestSpawnPoint == null)
            {
                Debug.LogError("No valid spawn point found!");
                return;
            }

            // Check if the nearest spawn point is in the player's screen range
            Vector3 screenPosition = Camera.main.WorldToViewportPoint(nearestSpawnPoint.position);
            bool isOnScreen = screenPosition.x > 0 && screenPosition.x < 1 && screenPosition.y > 0 && screenPosition.y < 1;

            // Use the second nearest point if the nearest is on screen
            Transform chosenSpawnPoint = isOnScreen && secondNearestSpawnPoint != null
                ? secondNearestSpawnPoint
                : nearestSpawnPoint;

            // Spawn the enemy ship at the chosen spawn point
            GameObject enemyShipPrefab = enemyShipGO; // Reference to the enemy ship prefab
            Instantiate(enemyShipPrefab, chosenSpawnPoint.position, chosenSpawnPoint.rotation);

            Debug.Log($"Enemy spawned at {chosenSpawnPoint.position} (OnScreen: {isOnScreen})");
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SpawnEnemyShip: " + ex);
        }
    }


    public void SpawnRowboat(bool hasTreasure)
    {
        try
        {
            Transform playerTransform = GameManager.Instance.PlayerGO.transform;
            //GameManager.Instance.PlayerGO.SetActive(false);
            Destroy(GameManager.Instance.PlayerGO);
            GameObject playerRowboat = Instantiate(_playerRowboatPrefab, playerTransform.position, playerTransform.rotation);
            GameManager.Instance.PlayerController.CanMove = true;
            if (hasTreasure)
            {
                playerRowboat.GetComponent<Rowboat>().HasTreasure = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SpawnRowboat: " + ex);
        }
    }

    public void SpawnRowboatWithCrane(bool hasTreasure)
    {
        try
        {
            Transform playerTransform = GameManager.Instance.PlayerGO.transform;
            Destroy(GameManager.Instance.PlayerGO);
            GameObject playerRowboat = Instantiate(_playerRowboatWithCranePrefab, playerTransform.position, playerTransform.rotation);
            GameManager.Instance.PlayerController.CanMove = true;
            if (hasTreasure)
            {
                playerRowboat.GetComponent<Rowboat>().HasTreasure = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SpawnRowboat: " + ex);
        }
    }

    public void SpawnShipSloop(int numberOfCannons)
    {
        try
        {
            if (numberOfCannons % 2 > 0 && numberOfCannons > 0 && numberOfCannons <= 6)
            {
                Debug.LogError("Only 2, 4, 6 cannons allowed.");
            }

            Transform playerTransform = GameManager.Instance.PlayerGO.transform;
            Destroy(GameManager.Instance.PlayerGO);

            switch (numberOfCannons)
            {
                case 2:
                    GameObject Sloop2CannonsIns = Instantiate(_playerShipSloop2CannonsPrefab, playerTransform.position, playerTransform.rotation);
                    Sloop2CannonsIns.GetComponent<Ship>().NumberOfCannons = 2;
                    break;
                case 4:
                    GameObject Sloop4CannonsIns = Instantiate(_playerShipSloop4CannonsPrefab, playerTransform.position, playerTransform.rotation);
                    Sloop4CannonsIns.GetComponent<Ship>().NumberOfCannons = 4;
                    break;
                case 6:
                    GameObject Sloop6CannonsIns = Instantiate(_playerShipSloop6CannonsPrefab, playerTransform.position, playerTransform.rotation);
                    Sloop6CannonsIns.GetComponent<Ship>().NumberOfCannons = 6;
                    break;
                default:
                    Debug.LogError("Only 2, 4, 6 cannons allowed.");
                    return;
            }

            GameManager.Instance.PlayerController.CanMove = true;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SpawnShipSloop: " + ex);
        }
    }

    private void SpawnBlackbeardShip()
    {
        try
        {
            _playerTransform = GameManager.Instance.PlayerGO.transform;

            if (_playerTransform == null)
            {
                Debug.LogWarning("Player transform is not assigned. Cannot spawn enemy ship near player.");
                return;
            }

            // Calculate spawn position around player
            float spawnDistance = 2 * _playerTransform.GetComponent<Unit>().AttackRange;
            Vector3 spawnPosition = _playerTransform.position + (Vector3.right * spawnDistance);

            // Instantiate enemy ship
            GameObject newEnemy = Instantiate(_blackbeardShipPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Spawned new enemy ship at " + spawnPosition);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SpawnEnemyShip: " + ex);
        }
    }

    public void RespawnPlayerShip(int numberOfCannons)
    {
        try
        {
            // Get all respawn points from the GameObjectManager
            Transform[] spawnPoints = PlayerSpawnPoints;

            // Find the nearest spawn point
            Transform nearestSpawnPoint = null;
            float shortestDistance = Mathf.Infinity;

            foreach (Transform spawnPoint in spawnPoints)
            {
                float distance = Vector3.Distance(GameManager.Instance.PlayerGO.transform.position, spawnPoint.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestSpawnPoint = spawnPoint;
                }
            }

            if (nearestSpawnPoint == null)
            {
                Debug.LogError("No valid respawn points found!");
                return;
            }

            // Spawn the new player ship with the same number of cannons
            SpawnShipSloop(numberOfCannons);

            // Set the new ship's position to the nearest spawn point
            GameManager.Instance.PlayerGO.transform.position = nearestSpawnPoint.position;
            GameManager.Instance.PlayerGO.transform.rotation = nearestSpawnPoint.rotation;

            Debug.Log($"Player respawned at {nearestSpawnPoint.position} with {numberOfCannons} cannons.");
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".RespawnPlayerShip: " + ex);
        }
    }

    public void ActivateIntroSalvageZone()
    {
        try
        {
            _introSalvageZone.SetActive(true);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ActivateIntroSalvageZone: " + ex);
        }
    }

    public void SetFishingText(bool active)
    {
        try
        {
            _fishingTextGO.SetActive(active);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetFishingText: " + ex);
        }
    }

    public void SetRowingText(bool active)
    {
        try
        {
            if (_controlsTutorialGO != null)
            {
                _controlsTutorialGO.SetActive(active);
                _rowingTextGO.SetActive(active);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetRowingText: " + ex);
        }
    }

    public void SetSailingModeText(bool active)
    {
        try
        {
            _sailingModeControlslGO.SetActive(active);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetSailingModeText: " + ex);
        }
    }

    public void SetWorldMapControlsText(bool active)
    {
        try
        {
            _worldMapControlslGO.SetActive(active);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetWorldMapControlsText: " + ex);
        }
    }

    public void SetPowderKegControlsText(bool active)
    {
        try
        {
            _powderKegControlslGO.SetActive(active);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetPowderKegControlsText: " + ex);
        }
    }

    public void SetGuideArrow(bool active)
    {
        try
        {
            _guideArrowGo.SetActive(active);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetGuideArrow: " + ex);
        }
    }

    public void DestroySalvageZone(GameObject salvageZone)
    {
        try
        {
            if (salvageZone != null)
            {
                _salvageZones.Remove(salvageZone);
                Destroy(salvageZone);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".DestroySalvageZone: " + ex);
        }
    }

    public void DisableBayBorder()
    {
        try
        {
            _bayBorder.SetActive(false);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".DisableBayBorder: " + ex);
        }
    }

    public void EnableBlackbeardIntroBorder()
    {
        try
        {
            _blackbeardIntroBorder.SetActive(true);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".DisableBayBorder: " + ex);
        }
    }

    public void SetUiElements(bool active)
    {
        try
        {
            if (active)
            {
                _uiElements.SetActive(true);
            }
            else
            {
                _uiElements.SetActive(false);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetUiElements: " + ex);
        }
    }

    public void SpawnLoot(SalvageZone salvageZoneLoot, Vector2 spawnPosition, bool isStrongEnemy = false, bool isBlackbeard = false)
    {
        try
        {
            if (isBlackbeard)
            {
                SalvageZone salvageZone = Instantiate(salvageZoneLoot, spawnPosition, salvageZoneLoot.transform.rotation);
            }
            else if (isStrongEnemy)
            {
                SalvageZone salvageZone = Instantiate(salvageZoneLoot, spawnPosition, salvageZoneLoot.transform.rotation);
                salvageZone.GoldCoins = 25;
            }
            else if (Probability(33.3f))
            {
                SalvageZone salvageZone = Instantiate(salvageZoneLoot, spawnPosition, salvageZoneLoot.transform.rotation);
                salvageZone.GoldCoins = UnityEngine.Random.Range(2, 5);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SpawnLoot: " + ex);
        }
    }
}
