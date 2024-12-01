using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneDialog : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private DialogUI _dialogUI;
    [SerializeField]
    private PlayableDirector _playableDirector;
    [SerializeField]
    private GameObject _aliceGO;
    [SerializeField]
    private GameObject _blackbeardShipGO;

    private Dialog _blackbeardDialog1 = new Dialog("Blackbeard", "Arrr! Attack!");
    private Dialog _blackbeardDialog2 = new Dialog("Blackbeard", "Alice, come with me.");
    private Dialog _aliceDialog1 = new Dialog("Alice", "Jim! Help me!");
    private Dialog _blackbeardDialog3 = new Dialog("Blackbeard", "Jim can't help you!");

    private void Start()
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Start: " + ex);
        }
    }

    public void PauseCutscene()
    {
        try
        {
            // Set the timeline's playback speed to 0 (pause)
            _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".PauseCutscene: " + ex);
        }
    }

    public void ResumeCutscene()
    {
        try
        {
            // Set the timeline's playback speed back to 1 (resume)
            _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ResumeCutscene: " + ex);
        }
    }

    public void ShowBlackbeardDialog1()
    {
        try
        {
            _dialogUI.ShowDialog(_blackbeardDialog1.Speaker, _blackbeardDialog1.Text);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ShowBlackbeardDialog1: " + ex);
        }
    }

    public void ShowBlackbeardDialog2()
    {
        try
        {
            _dialogUI.ShowDialog(_blackbeardDialog2.Speaker, _blackbeardDialog2.Text);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ShowBlackbeardDialog2: " + ex);
        }
    }

    public void AlicePositionChange()
    {
        try
        {
            _aliceGO.SetActive(false);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".AlicePositionChange: " + ex);
        }
    }

    public void ShowAliceDialog1()
    {
        try
        {
            _dialogUI.ShowDialog(_aliceDialog1.Speaker, _aliceDialog1.Text);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ShowAliceDialog1: " + ex);
        }
    }

    public void ShowBlackbeardDialog3()
    {
        try
        {
            _dialogUI.ShowDialog(_blackbeardDialog3.Speaker, _blackbeardDialog3.Text);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ShowBlackbeardDialog3: " + ex);
        }
    }

    public void BlackbearRedCannonAttack()
    {
        try
        {
            //GameManager.Instance.PlayerController.EnableMovement();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".BlackbearRedCannonAttack: " + ex);
        }

    }

    public void BlackbeardIntroOver()
    {
        try
        {
            //Destroy(_blackbeardShipGO);
            _blackbeardShipGO.SetActive(false);
            GameManager.Instance.gameObject.GetComponent<MusicManager>().EnemyProximityRange = 20;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".BlackbeardIntroOver: " + ex);
        }
    }


}
