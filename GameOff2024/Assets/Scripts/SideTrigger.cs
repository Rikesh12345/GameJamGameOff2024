using System;
using UnityEngine;

public class SideTrigger : MonoBehaviour
{
    [SerializeField]
    private bool _isPortSide; // True for port, false for starboard
    private Ship _parentShip;

    [SerializeField]
    private string _enemyTag;

    private void Start()
    {
        try
        {
            _parentShip = GetComponentInParent<Ship>();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Start: " + ex);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        try
        {
            if (other.gameObject.TryGetComponent<Unit>(out Unit enemyUnit))
            {
                if (other.CompareTag(_enemyTag) && _parentShip != null && !_parentShip.Dead && !enemyUnit.Dead)
                {
                    _parentShip.OnEnemyStay(other.transform, _isPortSide);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerStay2D: " + ex);
        }
    }
}
