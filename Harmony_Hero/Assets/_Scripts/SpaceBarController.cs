using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SpaceBarController : MonoBehaviour
{
    [SerializeField] private GameObject _actionBar;
    [SerializeField] private float _actionBarValue;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private Button actionButton;

    // Perfect 0.8
    // Pass 0.75 to 0.85

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _actionBar = GameObject.Find("ActionBar");
        _actionBar.GetComponent<Slider>().value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        _actionBarValue = _actionBar.GetComponent<Slider>().value;
        if (speed > 0)
        {
            _actionBar.GetComponent<Slider>().value = Mathf.Repeat(Time.time * speed, 1f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckHit();
        }
    }

    private void BarReset()
    {
        _actionBar.GetComponent<Slider>().value = 0.0f;
        speed = 0.0f;
        StartCoroutine(ResumeSpeed());
    }

    private IEnumerator ResumeSpeed()
    {
        yield return new WaitForSeconds(0.1f);
        speed = 0.5f;
    }

    private void CheckHit()
    {
        speed = 0.0f;
        Debug.Log("Space Bar Pressed at " + Mathf.Round(_actionBarValue * 100f) / 100);
        _actionBar.GetComponent<Slider>().value = _actionBarValue;

        float actionPoint = Mathf.Round(_actionBarValue * 100f) / 100f;

        if (actionPoint >= 0.75f && actionPoint <= 0.85f)
        {
            Debug.Log("Perfect");
            actionButton.onClick.Invoke();
        }
        else if (actionPoint >= 0.7f && actionPoint < 0.75f)
        {
            Debug.Log("Pass");
            actionButton.onClick.Invoke();
        }
        else if (actionPoint > 0.85f && actionPoint < 0.9f)
        {
            Debug.Log("Pass");
            actionButton.onClick.Invoke();
        }
        else
        {
            Debug.Log("Fail");
            BarReset();
        }

        
    }
}
