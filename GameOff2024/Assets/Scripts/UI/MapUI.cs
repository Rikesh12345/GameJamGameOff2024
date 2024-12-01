using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject _mapDisplay; // The UI Image displaying the Render Texture
    [SerializeField] private Camera _mapCamera; // The second camera for the map

    private bool _isMapVisible = false;

    private bool _firstTimeUse = true;

    private void Awake()
    {
        try
        {
            _mapDisplay.SetActive(false);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Awake: " + ex);
        }
    }

    private void Update()
    {
        try
        {
            if (GameManager.Instance.HasWorldMap)
            {
                // Toggle map visibility when "M" is pressed or when space/left click while map is open
                if (Keyboard.current.mKey.wasPressedThisFrame ||
                    (_isMapVisible && (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)))
                {
                    ToggleMap();

                    if (_firstTimeUse)
                    {
                        _firstTimeUse = false;
                        GameObjectManager.Instance.SetWorldMapControlsText(false);
                    }
                } 
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    // Method to toggle the map display
    public void ToggleMap()
    {
        try
        {
            if (GameManager.Instance.HasWorldMap)
            {
                SoundFXManager.Instance.PlayRandomSoundFXClip(GameManager.Instance.ButtonClickSound, GameManager.Instance.PlayerGO.transform, 0.5f);

                _isMapVisible = !_isMapVisible;
                _mapDisplay.SetActive(_isMapVisible);

                // Enable or disable the map camera based on visibility
                _mapCamera.enabled = _isMapVisible;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ToggleMap: " + ex);
        }
    }
}
