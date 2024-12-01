using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _goldCoinsTxt;

    private int _goldCoins;
    public int GoldCoins { get => _goldCoins; set => _goldCoins = value; }

    private bool[] _constructionsFound;
    public bool[] ConstructionsFound { get => _constructionsFound; set => _constructionsFound = value; }

    private void Start()
    {
        try
        {
            ConstructionsFound = new bool[5];
            //ConstructionsFound[0] = true;
            //ConstructionsFound[1] = true;
            //Debug.LogWarning("for testing");
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
            _goldCoinsTxt.text = _goldCoins.ToString();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Start: " + ex);
        }
    }
}
