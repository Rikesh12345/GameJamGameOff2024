using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackbeardIntroTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _blackbeardIntroTimelineGO;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision.CompareTag("Player"))
            {
                GameObjectManager.Instance.SetUiElements(false);
                _blackbeardIntroTimelineGO.SetActive(true);
                GameManager.Instance.PlayerController.DisableMovement();
                GameManager.Instance.PlayerGO.transform.position = new Vector3(85f, 456f, 0f);
                GameManager.Instance.PlayerGO.transform.rotation = Quaternion.Euler(0, 0, -90);
                GameManager.Instance.gameObject.GetComponent<MusicManager>().EnemyProximityRange = 100;
                Destroy(gameObject);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerEnter2D: " + ex);
        }
    }
}
