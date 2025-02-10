using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    public Image cover;
    public float delay = 1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cover.enabled = true;
        StartCoroutine(DelayedMethod(delay));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DelayedMethod(float delay)
    {
        yield return new WaitForSeconds(delay);
        cover.enabled = false;
    }
}
