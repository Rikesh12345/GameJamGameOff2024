using Pathfinding;
using System;
using UnityEngine;

public class EnemyShipMovement : MonoBehaviour
{
    private Transform _target;
    [SerializeField]
    private float _offset = 2;
    private float _randomVariance;
    [SerializeField]
    private float _minRandomVarianceRange = 1f;
    [SerializeField]
    private float _maxRandomVarianceRange = 2f;

    private IAstarAI _ai;
    private Unit _unit;
    private Rigidbody2D _rigidbody2D;


    private Vector3 _spawnPosition;

    [Header("Krake")]
    [SerializeField]
    private bool _isKrake;

    [Header("Boss")]
    [SerializeField]
    private bool _isBoss;

    private void Start()
    {
        try
        {
            _ai = GetComponent<IAstarAI>();
            _target = GameManager.Instance.PlayerGO.transform;
            _unit = GetComponent<Unit>();

            _rigidbody2D = GetComponent<Rigidbody2D>();

            // Apply initial randomization varied enemy positioning
            _randomVariance = UnityEngine.Random.Range(_minRandomVarianceRange, _maxRandomVarianceRange);

            _spawnPosition = transform.position;
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
            if (GameManager.Instance.PlayerGO == null)
            {
                return;
            }
            if (_unit.Dead)
            {
                _rigidbody2D.velocity = Vector2.zero;
                _ai.SetPath(null);
                _ai.isStopped = true;
                return;
            }
            if (_unit.IsKnockedBack)
            {
                _ai.SetPath(null);
                return;
            }

            if (GameManager.Instance.PlayerUnit.Dead)
            {
                _ai.maxSpeed = 3;
                _ai.destination = _spawnPosition;
                return;
            }

            if (Vector3.Distance(transform.position, GameManager.Instance.PlayerGO.transform.position) >= _unit.AttackRange * 1.5f)
            {
                _ai.maxSpeed = 8;
            }
            else if (Vector3.Distance(transform.position, GameManager.Instance.PlayerGO.transform.position) >= _unit.AttackRange)
            {
                _ai.maxSpeed = 4;
            }
            else
            {
                _ai.maxSpeed = 3;
            }

            if (_target == null)
            {
                UpdateTarget();
            }
            else
            {
                if (_isKrake || _isBoss)
                {
                    if (Vector3.Distance(transform.position, GameManager.Instance.PlayerGO.transform.position) <= _unit.AttackRange * 2.5f)
                    {
                        ChaseAndCircleAroundTarget();
                    }
                    else
                    {
                        _ai.destination = _spawnPosition;
                    }
                }
                else
                {
                    ChaseAndCircleAroundTarget();
                }
            }
            
            
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    private void UpdateTarget()
    {
        try
        {
            if (GameManager.Instance.PlayerGO != null)
            {
                _target = GameManager.Instance.PlayerGO.transform;
            }
            else
            {
                Debug.LogError("No player target found");
            }                            
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".UpdateTarget: " + ex);
        }
    }

    private void ChaseAndCircleAroundTarget()
    {
        try
        {
            Vector3 normal = (_ai.position - _target.position).normalized;
            Vector3 tangent = Vector3.Cross(normal, Vector3.forward); // Vector3.forward is the up direction for 2D

            // Set orbit destination
            _ai.destination = _target.position + normal * (_unit.AttackRange / _randomVariance) + tangent * _offset;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ChaseAndCircleAroundTarget: " + ex);
        }
    }

    private void OnDrawGizmos()
    {
        try
        {
            if (_unit != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(transform.position, _unit.AttackRange);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnDrawGizmos: " + ex);
        }
    }
}
