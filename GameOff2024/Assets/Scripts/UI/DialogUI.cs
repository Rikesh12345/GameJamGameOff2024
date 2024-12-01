using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _speakerNameText;
    [SerializeField] private TextMeshProUGUI _dialogText;
    [SerializeField] private GameObject _dialogBox;

    [Header("Settings")]
    [SerializeField] private float _textSpeed = 0.05f;

    private Coroutine _typingCoroutine;
    private bool _isTyping = false;
    private bool _dialogVisible = false;
    private string _fullText; // Store the full text

    [Header("CutsceneDialog")]
    [SerializeField]
    private CutsceneDialog _cutsceneDialog;

    private void Awake()
    {
        try
        {
            _dialogBox.SetActive(false);
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
            // Check for input to close dialog (spacebar or left mouse button)
            if (_dialogVisible && (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame))
            {
                if (_isTyping)
                {
                    CompleteTyping();
                }
                else
                {
                    HideDialog();
                    if (_cutsceneDialog.isActiveAndEnabled)
                    {
                        _cutsceneDialog.ResumeCutscene();
                    }                    
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    // Method to show dialog
    public void ShowDialog(string speaker, string text)
    {
        try
        {
            if (!_dialogBox.activeSelf)
            {
                _dialogBox.SetActive(true);
                _speakerNameText.text = speaker;
                _dialogVisible = true;

                _fullText = text; // Store the full text

                if (_typingCoroutine != null)
                {
                    StopCoroutine(_typingCoroutine);
                }

                _typingCoroutine = StartCoroutine(TypeText(_fullText));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ShowDialog: " + ex);
        }
    }

    // Method to hide dialog
    public void HideDialog()
    {
        try
        {
            _dialogBox.SetActive(false);
            _dialogVisible = false;

            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".HideDialog: " + ex);
        }
    }

    // Coroutine for typewriter effect
    private IEnumerator TypeText(string text)
    {
        _isTyping = true;
        _dialogText.text = "";

        foreach (char c in text)
        {
            _dialogText.text += c;
            yield return new WaitForSeconds(_textSpeed);
        }

        _isTyping = false;        
    }

    // Method to instantly complete typing
    private void CompleteTyping()
    {
        try
        {
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
            }

            // Display the full stored text
            _dialogText.text = _fullText;
            _isTyping = false;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".CompleteTyping: " + ex);
        }
    }
}
