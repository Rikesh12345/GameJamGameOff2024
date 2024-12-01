using UnityEngine;
using UnityEngine.Playables;

public class TimelineHandler : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector _playableDirector;

    private void OnEnable()
    {
        if (_playableDirector != null)
        {
            _playableDirector.stopped += OnTimelineStopped;
        }
    }

    private void OnDisable()
    {
        if (_playableDirector != null)
        {
            _playableDirector.stopped -= OnTimelineStopped;
        }
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        if (director == _playableDirector)
        {
            Debug.Log("Timeline ended");

            if (_playableDirector.transform.parent != null)
            {
                GameManager.Instance.PlayerController.CanMove = true;
                // Option 2: Destroy the Parent of PlayableDirector
                Destroy(_playableDirector.transform.parent.gameObject);
                GameObjectManager.Instance.SetUiElements(true);
            }
            else
            {
                // Option 2: Destroy the PlayableDirector
                Destroy(_playableDirector.gameObject);
                GameObjectManager.Instance.SetUiElements(true);
            }


        }
    }
}
