using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rowboat : Unit
{
    [SerializeField]
    private AudioClip[] _waterPaddleSound;

    protected override void Start()
    {
        try
        {
            base.Start();

        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    protected override void Update()
    {
        try
        {
            base.Update();

            MoveAnimation();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    private void MoveAnimation()
    {
        try
        {
            if (_rigidbody2D.velocity.sqrMagnitude > 1f)
            {
                _animator.SetBool("IsRowing", true);
            }
            else
            {
                _animator.SetBool("IsRowing", false);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".MoveAnimation: " + ex);
        }
    }

    protected override void PlayTakeDamageSoundFX()
    {
        try
        {
            //SoundFXManager.Instance.PlayRandomSoundFXClip(_shipTakeDamageSounds, transform, 0.15f);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".PlayTakeDamageSoundFX: " + ex);
        }
    }

    protected override void AttackAnimation(bool isPortSide)
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".AttackAnimation: " + ex);
        }
    }

    protected override void DieAnimation()
    {
        try
        {
            // Ship-specific death animation logic
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".DieAnimation: " + ex);
        }
    }

    public void IntroOver()
    {
        try
        {
            _animator.SetBool("IsFishing", true);
            GameObjectManager.Instance.ActivateIntroSalvageZone();
            GameObjectManager.Instance.SetFishingText(true);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".IntroOver: " + ex);
        }
    }

    public void StopFishing()
    {
        try
        {
            _animator.SetBool("IsFishing", false);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".StopFishing: " + ex);
        }
    }

    public void CollectIntroTreasure()
    {
        try
        {
            _animator.SetTrigger("CollectTreasure");
            GameObjectManager.Instance.SetFishingText(false);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".StopFishing: " + ex);
        }
    }

    public void TreasureCollected()
    {
        try
        {
            GameManager.Instance.ShowLootWindow();
            GameObjectManager.Instance.SpawnRowboat(true);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".TreasureCollected: " + ex);
        }
    }

    public void PlayRowingSounds()
    {
        try
        {
            SoundFXManager.Instance.PlayRandomSoundFXClip(_waterPaddleSound, transform, 0.25f);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".PlayRowingSounds: " + ex);
        }
    }
}
