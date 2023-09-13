using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRBeats;
using UnityEngine.UI;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance;

    public SpawnEventInfo wallInfo;
    public Spawneable wallObject;

    [SerializeField] GameObject[] startParticles = new GameObject[4];
    [SerializeField] GameObject[] blueParticles = new GameObject[4];
    [SerializeField] GameObject[] redParticles = new GameObject[4];
    [SerializeField] GameObject shieldObject;

    public Text notificationText;
    public Text etkisizHaleGetirir;
    private float timer = 5.5f;
    private int second;
    public bool canSave;
    public bool canHold;

    private void Awake()
    {
        instance = this;


        //lastTime = wallInfo.time;

    }

    private IEnumerator Start()
    {
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            second = (int)timer;
            notificationText.text = second.ToString();
            yield return null;
        }
        foreach (var item in startParticles)
        {
            item.SetActive(true);
            notificationText.text = "";
        }
    }
    public void SpawnWallForCube()
    {
        VR_BeatManager.instance.Spawn(wallObject, wallInfo);
    }

    public void LosePoint()
    {
        Debug.Log("1 point lost !!!");
    }
    public void BlueParticlesPlay()
    {
        foreach (var item in blueParticles)
        {
            item.SetActive(true);
            StartCoroutine(SetActiveFalse(item));
        }
    }

    public void RedParticlesPlay()
    {
        foreach (var item in redParticles)
        {
            item.SetActive(true);
            StartCoroutine(SetActiveFalse(item));
        }
    }

    IEnumerator SetActiveFalse(GameObject go)
    {
        yield return new WaitForSeconds(4f);
        go.SetActive(false);
    }

    public void OpenShield()
    {
        canSave = true;
        shieldObject.SetActive(true);
        VR_BeatManager.instance.Spawn(wallObject, wallInfo);
        //notificationText.text = "KORUR";
        StartCoroutine(GetBackShield());
    }

    IEnumerator GetBackShield()
    {
        yield return new WaitForFixedUpdate();
        //wallObject.GetComponentInChildren<Text>().text = "KORUR";
        //wallObject.GetComponentInChildren<Text>().text = "KORUR";
        yield return new WaitForSeconds(10f);
        //notificationText.text = "";
        shieldObject.SetActive(false);
        ScoreManager.canErrorIncrease = true;
        canSave = false;
    }

    public IEnumerator SpawnBubbleWall()
    {
        canHold = true;
        VR_BeatManager.instance.Spawn(wallObject, wallInfo);
        yield return new WaitForFixedUpdate();
        //wallObject.GetComponentInChildren<Text>().text = "HAPSEDER";
    }
}
