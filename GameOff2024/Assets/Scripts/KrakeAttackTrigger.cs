using System;
using UnityEngine;

public class KrakeAttackTrigger : MonoBehaviour
{
    private SeaMonster _parentSeaMonster;
    [SerializeField]
    private string _enemyTag;

    private void Start()
    {
        try
        {
            _parentSeaMonster = GetComponentInParent<SeaMonster>();
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
                if (other.CompareTag(_enemyTag) && !enemyUnit.Dead)
                {
                    _parentSeaMonster.OnEnemyStay(other.transform);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerStay2D: " + ex);
        }
    }
}
