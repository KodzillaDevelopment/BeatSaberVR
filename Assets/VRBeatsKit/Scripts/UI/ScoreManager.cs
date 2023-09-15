using UnityEngine;
using UnityEngine.UI;
using Platinio.TweenEngine;
using VRBeats.ScriptableEvents;
using System.Collections;

namespace VRBeats
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private Text multiplierLabel = null;
        public Text scoreLabel = null;
        [SerializeField] private Image multiplierLoader = null;
        [SerializeField] private float scoreFollowTime = 1.0f;
        [SerializeField] private CanvasGroup canvasGroup = null;
        [SerializeField] private GameEvent onGameOver = null;
        [SerializeField] Text correctNumbersText;
        [SerializeField] Text errorNumbersText;

        private int maxMultiplier = 0;
        private int scorePerHit = 0;
        private int currentScore = 0;
        private int currentMultiplier = 0;
        private int toNextMultiplierIncrease = 2;
        public int acumulateCorrectSlices = 0;
        public int acumulateErrors = 0;
        private int errorLimit = 0;
        private float visualScore = 0.0f;
        private int scoreTweenID = -1;
        private int loaderTweenID = -1;

        public static int correctNumber = 0;
        public static bool canErrorIncrease = true;

        private bool destroyed = false;

        public int CurrentScore
        {
            get
            {
                return currentScore;
            }
        }

        private void Awake()
        {
            maxMultiplier = VR_BeatManager.instance.GameSettings.MaxMultiplier;
            scorePerHit = VR_BeatManager.instance.GameSettings.ScorePerHit;
            errorLimit = VR_BeatManager.instance.GameSettings.ErrorLimit;
            multiplierLoader.fillAmount = 0.0f;
        }
        private void OnEnable()
        {
            correctNumber = 0;
        }
        public void OnGameOver()
        {
            gameObject.CancelAllTweens();
            canvasGroup.Fade(0.0f, 0.5f).SetEase(Ease.EaseOutExpo).SetOwner(gameObject);
        }

        public void OnGameRestart()
        {
            ResetThisComponent();
            gameObject.CancelAllTweens();
            canvasGroup.Fade(1.0f, 0.5f).SetEase(Ease.EaseOutExpo).SetOwner(gameObject);
        }

        public void ResetThisComponent()
        {
            currentMultiplier = 0;
            currentScore = 0;
            acumulateCorrectSlices = 0;
            visualScore = 0;
            acumulateErrors = 0;
            toNextMultiplierIncrease = 2;
        }



        private void Update()
        {
            //Debug.Log("Error limit is : " + errorLimit);
            //Debug.Log("Current score is : " + currentScore);
            //Debug.Log("Accumulate errors is : " + acumulateErrors);
            //Debug.Log("Accumulate correct is : " + acumulateCorrectSlices);

            UpdateUI();
        }

        public void OnCorrectSlice()
        {
            if (destroyed)
                return;

            correctNumber++;

            if (correctNumber > 45 && correctNumber % 20 == 0)
            {
                canErrorIncrease = false;
                FindObjectOfType<GameEventManager>().OpenShield();
            }
            if (correctNumber == 5 || correctNumber == 15)
            {
                StartCoroutine(FindObjectOfType<GameEventManager>().SpawnBubbleWall());
                StartCoroutine(ShowBubbles());
                //foreach (var item in FindObjectsOfType<VR_BeatCube>())
                //{
                //    if (item.gameObject.activeInHierarchy)
                //    {
                //        //item.canMove = false;
                //        //item.particles.SetActive(true);
                //        //FindObjectOfType<GameEventManager>().notificationText.text = "HAPSEDER";
                //    }
                //}
            }
            //acumulateErrors = 0;
            acumulateCorrectSlices++;
            currentScore += scorePerHit + (scorePerHit * currentMultiplier);

            CancelTweenById(scoreTweenID);
            scoreTweenID = PlatinioTween.instance.ValueTween(visualScore, currentScore, scoreFollowTime).SetEase(Ease.EaseOutExpo).SetOnUpdateFloat(delegate (float value)
             {
                 visualScore = value;
             }).ID;

            UpdateMultiplierLoaderValue();

            if (acumulateCorrectSlices >= toNextMultiplierIncrease)
            {
                IncreaseMultiplier();
            }


        }
        private float timer = 4f;
        IEnumerator ShowBubbles()
        {
            while (timer >= 0)
            {
                timer -= Time.deltaTime;
                foreach (var item in FindObjectsOfType<VR_BeatCube>())
                {
                    if (item.gameObject.activeInHierarchy)
                    {
                        item.canMove = false;
                        item.particles.SetActive(true);
                        //yield return new WaitForSeconds(3f);
                        //item.onCorrectSlice.Invoke();
                        //yield return new WaitForFixedUpdate();
                        item.Kill();
                        //item.canMove = false;
                        //item.particles.SetActive(true);
                        //StartCoroutine(ShowBubbles(item));
                        //FindObjectOfType<GameEventManager>().notificationText.text = "HAPSEDER";
                    }
                }
                if (timer < 0)
                {
                    GameEventManager.instance.canHold = false;
                    timer = 3f;
                    yield break;
                }
                yield return null;
            }

            //FindObjectOfType<GameEventManager>().notificationText.text = "";
        }

        private void CancelTweenById(int id)
        {
            if (id != -1)
                PlatinioTween.instance.CancelTween(id);
        }

        private void UpdateMultiplierLoaderValue()
        {
            if (destroyed)
                return;

            float multiplierLoaderValue = (float)acumulateCorrectSlices / (float)toNextMultiplierIncrease;


            CancelTweenById(loaderTweenID);
            loaderTweenID = PlatinioTween.instance.ValueTween(multiplierLoader.fillAmount, multiplierLoaderValue, 1.0f).SetEase(Ease.EaseOutExpo).SetOnUpdateFloat(delegate (float value)
            {
                if (multiplierLoader != null)
                    multiplierLoader.fillAmount = value;
            }).SetOwner(multiplierLoader.gameObject).ID;
        }

        public void OnIncorrectSlice()
        {
            if (destroyed)
                return;

            if (canErrorIncrease)
            {
                acumulateErrors++;
                acumulateCorrectSlices = 0;
                currentMultiplier = 0;
                toNextMultiplierIncrease = 2;

                UpdateMultiplierLoaderValue();

                if (acumulateErrors > errorLimit)
                {
                    //onGameOver.Invoke();
                }
            }

        }

        private void UpdateUI()
        {
            if (destroyed)
                return;

            multiplierLabel.text = currentMultiplier.ToString();
            scoreLabel.text = Mathf.CeilToInt(visualScore).ToString();
            correctNumbersText.text = correctNumber.ToString();
            errorNumbersText.text = acumulateErrors.ToString();
        }

        private void IncreaseMultiplier()
        {
            if (destroyed)
                return;

            acumulateCorrectSlices = 0;
            currentMultiplier = Mathf.Min(currentMultiplier + 1, maxMultiplier);
            toNextMultiplierIncrease = (currentMultiplier + 1) * 2;

            PlatinioTween.instance.CancelTween(multiplierLoader.gameObject);

            PlatinioTween.instance.ValueTween(multiplierLoader.fillAmount, 1.0f, 1.0f).SetEase(Ease.EaseOutExpo).SetOnUpdateFloat(delegate (float value)
            {
                if (multiplierLoader != null)
                    multiplierLoader.fillAmount = value;
            }).SetOwner(multiplierLoader.gameObject).SetOnComplete(delegate
           {
               if (multiplierLabel != null)
               {
                   PlatinioTween.instance.ValueTween(multiplierLoader.fillAmount, 0.0f, 0.5f).SetEase(Ease.EaseOutExpo).SetOnUpdateFloat(delegate (float value)
                   {
                       if (multiplierLoader != null)
                           multiplierLoader.fillAmount = value;
                   }).SetOwner(multiplierLoader.gameObject);
               }

           });

        }

    }

}

