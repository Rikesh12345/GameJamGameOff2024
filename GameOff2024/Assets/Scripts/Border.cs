using System;
using UnityEngine;

public class Border : MonoBehaviour
{
    [Header("Knockback Settings")]
    [SerializeField] private float _knockbackForce = 10f;

    [Header("Dialog System")]
    [SerializeField] private DialogUI _dialogUI;

    [SerializeField] private Dialog knockbackDialog = new Dialog("Jim", "I shouldn't leave the bay in a rowboat.");

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision.CompareTag("Player"))
            {
                Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
                Unit unit = collision.GetComponent<Unit>();

                if (playerRb != null && unit != null)
                {
                    // Calculate knockback direction to the left
                    Vector2 knockbackDirection = Vector2.left * _knockbackForce;

                    // Start knockback coroutine
                    unit.StartCoroutine(unit.ApplyKnockback(knockbackDirection));

                    // Show dialog after knockback
                    if (_dialogUI != null)
                    {
                        _dialogUI.ShowDialog(knockbackDialog.Speaker, knockbackDialog.Text);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerEnter2D: " + ex);
        }
    }
}
