using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _outroWindowGO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowOutroWindow()
    {
        try
        {
            _outroWindowGO.SetActive(true);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ShowOutroWindow: " + ex);
        }
    }

    public void OkOutroButton()
    {
        try
        {
            SceneManager.LoadScene("MainMenu");
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OkOutroButton: " + ex);
        }
    }
}
