using System;
using System.Collections;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [Header("Unit Settings")]
    [SerializeField]
    private int _health;
    public int Health { get => _health; private set => _health = value; }
    [SerializeField]
    private int _maxHealth;
    public int MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
    [SerializeField]
    private int _maxMaxHealth = 20;
    public int MaxMaxHealth { get => _maxMaxHealth; set => _maxMaxHealth = value; }
    [SerializeField]
    private float _movementSpeed;
    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }
    [SerializeField]
    private int _maxMovementSpeed = 8;
    public int MaxMovementSpeed { get => _maxMovementSpeed; set => _maxMovementSpeed = value; }
    [SerializeField]
    private float _rotationSpeed;
    public float RotationSpeed { get => _rotationSpeed; private set => _rotationSpeed = value; }
    [SerializeField]
    private float attackRange;
    public float AttackRange { get => attackRange; private set => attackRange = value; }
    private bool _dead;
    public bool Dead { get => _dead; private set => _dead = value; }

    protected Transform _target;

    protected Animator _animator;

    protected Rigidbody2D _rigidbody2D;

    [SerializeField]
    private SpriteRenderer _unitVisuals;
    public SpriteRenderer UnitVisuals { get => _unitVisuals; set => _unitVisuals = value; }


    private bool _hasTreasure;
    public bool HasTreasure { get => _hasTreasure; set => _hasTreasure = value; }

    [SerializeField]
    private GameObject _treasureGO;

    [Header("MapIcon")]
    [SerializeField]
    private GameObject _mapIcon;

    [Header("Hit Effect")]
    [SerializeField]
    private GameObject _hitEfectPrefab;

    [Header("EnemyHealthBar")]
    [SerializeField]
    private EnemyHealthUI _enemyHealthUI;

    [Header("NPC")]
    [SerializeField]
    protected bool _isNPC;

    [Header("Knockback Settings")]
    [SerializeField]
    private float _knockbackDuration = 0.2f; // Duration of the knockback effect
    private bool _isKnockedBack = false;
    public bool IsKnockedBack { get => _isKnockedBack; set => _isKnockedBack = value; }

    [Header("Loot")]
    [SerializeField]
    private SalvageZone _salvageZoneLootPrefab;
    [SerializeField]
    protected bool _isBlackbeared;

    private void Awake()
    {
        try
        {
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            Health = MaxHealth;

            if (GameManager.Instance != null && CompareTag("Player") && !_isNPC)
            {
                GameManager.Instance.PlayerGO = gameObject;
                GameManager.Instance.PlayerController = GetComponent<PlayerController>();
                GameManager.Instance.Vcam.Follow = GameManager.Instance.PlayerGO.transform;                
                GameManager.Instance.HealthUI.SetHealthUI(this);
            }
            if (CompareTag("Enemy"))
            {
                _enemyHealthUI.GetComponent<EnemyHealthUI>();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Awake: " + ex);
        }
    }

    protected virtual void Start()
    {
        try
        {
            InitializeUnit();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Start: " + ex);
        }
    }

    public void InitializeUnit()
    {
        try
        {
            if (CompareTag("Enemy"))
            {
                GameObjectManager.Instance.RegisterEnemy(this);                
            }

            if (_mapIcon != null)
            {
                _mapIcon.SetActive(true);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Start: " + ex);
        }

        SetTreasure(HasTreasure);
    }

    protected virtual void Update()
    {
        try
        {
            //UpdateTarget();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    //protected void UpdateTarget()
    //{
    //    try
    //    {
    //        _target = null;  // Reset the target each frame

    //        // Use GameObjectManager to find the nearest enemy within the attack range
    //        Unit nearestEnemy = GameObjectManager.Instance.FindNearestEnemy(transform.position, AttackRange);

    //        if (nearestEnemy != null)
    //        {
    //            _target = nearestEnemy.transform;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("ERROR in " + this + ".UpdateTarget: " + ex);
    //    }
    //}

    //public void Attack()
    //{
    //    try
    //    {
    //        if (_target != null && Vector3.Distance(transform.position, _target.position) <= AttackRange)
    //        {
    //            AttackAnimation(true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("ERROR in " + this + ".Attack: " + ex);
    //    }
    //}

    protected abstract void AttackAnimation(bool isPortSide);

    public void TakeDamage(int damage, Transform hitPoint = null)
    {
        try
        {
            if (Dead)
            {
                return;
            }

            Health -= damage;

            SetHealthUI();

            if (hitPoint != null)
            {
                GameObject hitEffect = Instantiate(_hitEfectPrefab, hitPoint.position, Quaternion.identity);
                Destroy(hitEffect, 2f);
            }

            PlayTakeDamageSoundFX();

            if (Health <= 0)
            {
                Dead = true;
                Die();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".TakeDamage: " + ex);
        }
    }

    protected void SetHealthUI()
    {
        try
        {
            if (CompareTag("Player"))
            {
                GameManager.Instance.HealthUI.SetHealthUI(this);
            }
            if (CompareTag("Enemy"))
            {
                _enemyHealthUI.SetHealthUI();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetHealthUI: " + ex);
        }
    }

    public void IncreaseMaxHealth(int value)
    {
        try
        {
            MaxHealth += value;
            Health = MaxHealth;
            GameManager.Instance.HealthUI.SetHealthUI(this);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".IncreaseMaxHealth: " + ex);
        }
    }

    public void SetHealthToMax()
    {
        try
        {
            Health = MaxHealth;
            GameManager.Instance.HealthUI.SetHealthUI(this);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetHealthToMax: " + ex);
        }
    }

    protected abstract void PlayTakeDamageSoundFX();

    private void Die()
    {
        try
        {
            if (_isBlackbeared)
            {
                GameManager.Instance.GameOver();
            }

            DieAnimation();
            DropLoot(_salvageZoneLootPrefab, transform.position);
            if (CompareTag("Player"))
            {
                GameManager.Instance.HealthUI.SetHealth(false);
            }
            // Unregister the enemy from GameObjectManager
            if (CompareTag("Enemy"))
            {
                _enemyHealthUI.SetHealth(false);
                GameObjectManager.Instance.UnregisterEnemy(this);
            }
            Destroy(gameObject, 3f); // or any other cleanup logic
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Die: " + ex);
        }
    }

    protected abstract void DieAnimation();

    protected virtual void DropLoot(SalvageZone salvageZonePrefab, Vector2 positon)
    {
        try
        {

        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".DropLoot: " + ex);
        }
    }

    public void SetTreasure(bool active)
    {
        try
        {
            if (_treasureGO != null)
            {
                if (active)
                {
                    _treasureGO.SetActive(true);
                }
                else
                {
                    _treasureGO.SetActive(false);
                } 
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".SetTreasure: " + ex);
        }
    }

    public void CollectTreasure()
    {
        try
        {
            //_animator.SetTrigger("CollectTreasure");
            GameManager.Instance.ShowLootWindow();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".CollectTreasure: " + ex);
        }
    }

    // Knockback coroutine to disable movement temporarily
    public IEnumerator ApplyKnockback(Vector2 force)
    {
        try
        {
            IsKnockedBack = true;

            // Apply the knockback force
            _rigidbody2D.velocity = Vector2.zero; // Stop current movement
            _rigidbody2D.AddForce(force, ForceMode2D.Impulse);

            Debug.Log($"Knockback applied: {force}");
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in ApplyKnockback: " + ex);
        }

        // Wait for the knockback duration
        yield return new WaitForSeconds(_knockbackDuration);

        IsKnockedBack = false;
        Debug.Log("Knockback ended");
    }

    private void OnDestroy()
    {
        try
        {
                   
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnDestroy: " + ex);
        }
    }

    protected virtual void OnDisable()
    {
        try
        {
            if (GameManager.Instance.IsQuitting)
            {
                return;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnDisable: " + ex);
        }
    }
}
