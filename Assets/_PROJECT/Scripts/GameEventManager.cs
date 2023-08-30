using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRBeats;

public class GameEventManager : MonoBehaviour
{
    [SerializeField] GameObject[] blueParticles = new GameObject[4];
    [SerializeField] GameObject[] redParticles = new GameObject[4];
    [SerializeField] GameObject shieldObject;

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

    public void OpenShield ()
    {
        shieldObject.SetActive(true);
        StartCoroutine(GetBackShield());
    }

    IEnumerator GetBackShield()
    {
        yield return new WaitForSeconds(10f);
        shieldObject.SetActive(false);
        ScoreManager.canErrorIncrease = true;
    }
}
