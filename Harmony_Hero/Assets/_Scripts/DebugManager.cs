using UnityEngine;
using TMPro;

public class DebugManager : MonoBehaviour
{
    public GameObject debugPanel;
    public TextMeshProUGUI debugText;
    private static DebugManager instance;
    private InputSystem_Actions _inputs;
    private void Awake()
    {
        instance = this;
        debugPanel.SetActive(false);
        _inputs = new InputSystem_Actions();

        _inputs.Player.Debug.performed += context =>
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
        };
    }

    private void OnEnable() => _inputs.Enable();
    private void OnDisable() => _inputs.Disable();

    public static void Log(string message)
    {
        instance.ClearLog();
        if (instance != null)
            instance.debugText.text += message + "\n";
    }

    public void ClearLog()
    {
        debugText.text = "";
    }
}
