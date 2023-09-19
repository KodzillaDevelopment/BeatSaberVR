using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] GameObject mainLight;

    [SerializeField] GameObject[] leftLights = new GameObject[8];
    [SerializeField] GameObject[] rightLights = new GameObject[8];

    private bool gameobjectCanRight;
    private bool leftCanRight;
    private bool rightCanRight;

    void Start()
    {
        StartCoroutine(RotateGameobject());

    }

    IEnumerator RotateGameobject()
    {
        while (true)
        {
            gameobjectCanRight = true;
            leftCanRight = true;
            rightCanRight = true;
            yield return new WaitForSeconds(3f);
            gameobjectCanRight = false;
            leftCanRight = false;
            rightCanRight = false;
            yield return new WaitForSeconds(3f);

            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (gameobjectCanRight)
        {
            mainLight.transform.Rotate(0, 1f, 0);
        }
        else
        {
            mainLight.transform.Rotate(0, -1f, 0);
        }
        if (leftCanRight)
        {
            foreach (var item in leftLights)
            {
                item.transform.Rotate(0, 1f, 1f);
            }
            //leftLights.transform.Rotate(0, 2f, 2f);
        }
        else
        {
            foreach (var item in leftLights)
            {
                item.transform.Rotate(0, -1f, -1f);
            }
            //leftLights.transform.Rotate(0, -2f, -2f);
        }
        if (rightCanRight)
        {
            foreach (var item in rightLights)
            {
                item.transform.Rotate(0, 1f, 1f);
            }
            //rightLights.transform.Rotate(0, 2f, 2f);
        }
        else
        {
            foreach (var item in rightLights)
            {
                item.transform.Rotate(0, -1f, -1f);
            }
            //rightLights.transform.Rotate(0, -2f, -2f);
        }
    }
}
