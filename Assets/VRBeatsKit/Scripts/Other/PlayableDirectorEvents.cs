using UnityEngine;
using UnityEngine.Playables;
using VRBeats.ScriptableEvents;

namespace VRBeats
{
    public class PlayableDirectorEvents : MonoBehaviour
    {
        [SerializeField] private GameEvent onLevelComplete = null;
        [SerializeField] GameObject closedCanvas;
        [SerializeField] GameObject closeObject;
        [SerializeField] GameObject openObject;

        private PlayableDirector director = null;
        private bool alreadyStarted = false;
        private bool eventTrigered = false;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();           
        }

        private void Update()
        {
            if (!alreadyStarted)
            {
                alreadyStarted = director.state == PlayState.Playing;
                return;
            }

            if (!eventTrigered && director.time >= director.playableAsset.duration - 0.5f)
            {
                closeObject.SetActive(false);
                openObject.SetActive(true);
                closedCanvas.SetActive(false);
                eventTrigered = true;
                onLevelComplete.Invoke();
            }

        }

        public void OnRestart()
        {           
            alreadyStarted = false;
            eventTrigered = false;
        }

    }

}
