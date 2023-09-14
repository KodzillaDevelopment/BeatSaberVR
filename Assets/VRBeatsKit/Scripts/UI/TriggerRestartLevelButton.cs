using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRBeats.ScriptableEvents;

namespace VRBeats.UI
{
    [RequireComponent(typeof(Button))]
    public class TriggerRestartLevelButton : MonoBehaviour
    {
        [SerializeField] private GameEvent onRestart = null;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener( TriggerRestartEvent ); ;
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void TriggerRestartEvent()
        {
            ScoreManager.correctNumber = 0;

            SceneManager.LoadScene(0);
            //onRestart.Invoke();
        }
    }
}

