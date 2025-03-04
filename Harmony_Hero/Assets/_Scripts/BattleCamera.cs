using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class BattleCamera : MonoBehaviour
{
    private CinemachineCamera cam;

    [Header("Camera Settings")]
    public Transform playerTarget;
    public Transform enemyTarget;
    public float panSpeed = 2f;
    public float zoomInSize = 4.5f;
    public float defaultZoomSize = 5.5f;
    public float shakeIntensity = 1f;
    public float shakeTime = 0.2f;

    void Start()
    {
        cam = GetComponent<CinemachineCamera>();

        if (cam == null)
        {
            Debug.LogError("CinemachineCamera component is missing!");
            return;
        }

        // Set initial follow target
        cam.Follow = playerTarget;
        cam.LookAt = playerTarget;

        Shake();
    }

    // Switch focus to the enemy
    public void FocusOnEnemy()
    {
        if (enemyTarget != null)
        {
            cam.Follow = enemyTarget;
            cam.LookAt = enemyTarget;
        }
    }

    // Switch focus back to the player
    public void FocusOnPlayer()
    {
        if (playerTarget != null)
        {
            cam.Follow = playerTarget;
            cam.LookAt = playerTarget;
        }
    }

    // Smooth zoom function
    public void ZoomIn()
    {
        StopAllCoroutines();
        StartCoroutine(Zoom(zoomInSize));
    }

    public void ZoomOut()
    {
        StopAllCoroutines();
        StartCoroutine(Zoom(defaultZoomSize));
    }

    IEnumerator Zoom(float targetSize)
    {
        float startSize = cam.Lens.OrthographicSize;
        float elapsed = 0f;
        float duration = 0.3f;

        while (elapsed < duration)
        {
            cam.Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.Lens.OrthographicSize = targetSize;
    }

    // Camera shake effect
    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeEffect());
    }

    IEnumerator ShakeEffect()
    {
        Vector3 originalPosition = cam.transform.position;
        float elapsed = 0f;

        while (elapsed < shakeTime)
        {
            cam.transform.position = originalPosition + (Vector3)Random.insideUnitCircle * shakeIntensity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = originalPosition;
    }
}
