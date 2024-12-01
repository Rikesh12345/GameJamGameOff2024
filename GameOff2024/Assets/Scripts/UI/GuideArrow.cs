using System;
using UnityEngine;

public class GuideArrow : MonoBehaviour
{
    [SerializeField]
    private Transform _target; // Target object to point to
    [SerializeField]
    private GameObject _arrowImageGO; // GameObject of the arrow image
    [SerializeField]
    private float _hideDistance = 25f; // Distance threshold to hide the arrow

    private void LateUpdate()
    {
        try
        {
            if (_target == null) return;

            // Calculate the direction to the target
            Vector3 directionToTarget = _target.position - GameManager.Instance.PlayerGO.transform.position;

            // Check the distance to the target
            float distanceToTarget = directionToTarget.magnitude;

            // Show or hide the arrow based on distance
            if (distanceToTarget <= _hideDistance)
            {
                _arrowImageGO.SetActive(false);
                return; // No need to rotate if the arrow is hidden
            }
            else
            {
                _arrowImageGO.SetActive(true);
            }

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            // Apply the rotation on the z-axis
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".LateUpdate: " + ex);
        }
    }
}
