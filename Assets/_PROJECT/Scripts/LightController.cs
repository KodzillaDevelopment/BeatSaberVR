using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public float timer = 3f;
    public float rotationSpeed = 1f;

    [SerializeField] GameObject mainLight;

    [SerializeField] GameObject[] leftLights = new GameObject[8];
    [SerializeField] GameObject[] rightLights = new GameObject[8];
    [SerializeField] Material redMaterial;

    private bool gameobjectCanRight;
    private bool leftCanRight;
    private bool rightCanRight;

    void Start()
    {
        timer = 4f;
        rotationSpeed = .8f;
        StartCoroutine(RotateGameobject());

    }

    IEnumerator RotateGameobject()
    {
        while (true)
        {
            gameobjectCanRight = true;
            leftCanRight = true;
            rightCanRight = true;
            yield return new WaitForSeconds(timer);
            redMaterial.SetColor("_EmissionColor", Color.red * 10f);
            gameobjectCanRight = false;
            leftCanRight = false;
            rightCanRight = false;
            yield return new WaitForSeconds(timer);
            redMaterial.SetColor("_EmissionColor", Color.red * 2f);

            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (gameobjectCanRight)
        {
            mainLight.transform.Rotate(0, 0, rotationSpeed);
        }
        else
        {
            mainLight.transform.Rotate(0, 0, -rotationSpeed);
        }
        if (leftCanRight)
        {
            foreach (var item in leftLights)
            {
                item.transform.Rotate(0, rotationSpeed, rotationSpeed);
            }
            //leftLights.transform.Rotate(0, 2f, 2f);
        }
        else
        {
            foreach (var item in leftLights)
            {
                item.transform.Rotate(0, -rotationSpeed, -rotationSpeed);
            }
            //leftLights.transform.Rotate(0, -2f, -2f);
        }
        if (rightCanRight)
        {
            foreach (var item in rightLights)
            {
                item.transform.Rotate(0, rotationSpeed, rotationSpeed);
            }
            //rightLights.transform.Rotate(0, 2f, 2f);
        }
        else
        {
            foreach (var item in rightLights)
            {
                item.transform.Rotate(0, -rotationSpeed, -rotationSpeed);
            }
            //rightLights.transform.Rotate(0, -2f, -2f);
        }
    }
}
