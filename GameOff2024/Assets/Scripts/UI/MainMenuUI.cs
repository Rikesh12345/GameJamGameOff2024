using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private Button _startButton;
    [SerializeField]
    private Button _exitButton;
    [SerializeField]
    private Button _creditsButton;

    [SerializeField]
    private GameObject _creditWindow;

    [Header("LoadingScene")]
    [SerializeField]
    private GameObject _loadingScreenGO;
    [SerializeField]
    private Slider _loadingSceneBar;
    [SerializeField]
    private TMP_Text _loadingText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        try
        {
            StartCoroutine(LoadSceneAsynchronously("SampleScene"));
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".StartGame: " + ex);
        }        
    }

    //private IEnumerator LoadSceneAsynchronously()
    //{
    //    _startButton.interactable = false;
    //    _loadingSceneProgressBar.fillAmount = 0;

    //    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("SampleScene");

    //    while (!asyncOperation.isDone)
    //    {
    //        //float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
    //        //_loadingSceneProgressBar.fillAmount = progress;
    //        //_loadingSceneSlider.value = progress;
    //        _loadingSceneSlider.value = asyncOperation.progress;
    //        yield return null;
    //    }
    //    _startButton.interactable = true;
    //}

    private IEnumerator LoadSceneAsynchronously(string sceneName)
    {
        // Show loading screen
        _loadingScreenGO.SetActive(true);

        // Begin loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Prevent the scene from activating until loading is complete
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Update the loading bar and text
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            _loadingSceneBar.value = progress;

            if (_loadingText != null)
            {
                if (progress != 0)
                {
                    _loadingText.text = $"{((progress * 100) - 1):0}%";
                }
                else
                {
                    _loadingText.text = $"{(progress * 100):0}%";
                }                
            }

            // Activate the scene when fully loaded
            if (operation.progress >= 0.9f)
            {
                //// Wait for a short moment (optional)
                //yield return new WaitForSeconds(0.5f);

                // Activate the scene
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void ExitGame()
    {
        try
        {
            Application.Quit();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ExitGame: " + ex);
        }
    }

    public void Credits()
    {
        try
        {
            _creditWindow.SetActive(true);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Credits: " + ex);
        }
    }

    public void CreditsOkButton()
    {
        try
        {
            _creditWindow.SetActive(false);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".CreditsOkButton: " + ex);
        }
    }
}
