using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] GameObject mainLight;

    [SerializeField] GameObject leftLights;
    [SerializeField] GameObject rightLights;

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
        //if (leftCanRight)
        //{
        //    leftLights.transform.Rotate(0, 2f, 2f);
        //}
        //else
        //{
        //    leftLights.transform.Rotate(0, -2f, -2f);
        //}
        //if (rightCanRight)
        //{
        //    rightLights.transform.Rotate(0, 2f, 2f);
        //}
        //else
        //{
        //    rightLights.transform.Rotate(0, -2f, -2f);
        //}
    }
}
