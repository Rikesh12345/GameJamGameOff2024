using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldIncomeUI : MonoBehaviour
{
    private Vector3 _offset;

    [SerializeField]
    private AudioClip[] _coinIncomeSound;

    // Start is called before the first frame update
    void Start()
    {
        _offset = new Vector2(0, 5);
        transform.position += _offset;

        SoundFXManager.Instance.PlayRandomSoundFXClip(_coinIncomeSound, GameManager.Instance.PlayerGO.transform, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
