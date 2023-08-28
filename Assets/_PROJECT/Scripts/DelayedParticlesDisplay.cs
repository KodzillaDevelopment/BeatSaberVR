using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedParticlesDisplay : MonoBehaviour
{
    [SerializeField] GameObject[] childs = new GameObject[4];
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

        foreach (var item in childs)
        {
            item.SetActive(true);
        }
    }

}
