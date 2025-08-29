using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; // only if you load a scene

public class CreditMenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button returnButton;   // drag your Return button here
    [SerializeField] private GameObject firstSelect; // optional: set to Return button for gamepad

    private void OnEnable()
    {
        // Force cursor on when Credits is visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Make gamepad/keyboard navigation work instantly
        if (EventSystem.current != null && firstSelect != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelect);
        }

        // Hook up Return if not assigned in Inspector
        if (returnButton != null)
            returnButton.onClick.AddListener(HandleReturn);
    }

    private void OnDisable()
    {
        // Optional: if your game normally hides the cursor in gameplay, restore it there.
        // Leave as-is if your main menu should also show the cursor.
    }

    private void Update()
    {
        // Keyboard escape as backup
        if (UnityEngine.InputSystem.Keyboard.current != null &&
            UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleReturn();
        }

        // Gamepad B / Circle as backup (new Input System)
        var gamepad = UnityEngine.InputSystem.Gamepad.current;
        if (gamepad != null && gamepad.bButton.wasPressedThisFrame)
        {
            HandleReturn();
        }
    }

    private void HandleReturn()
    {
        // If Credits is a panel, just disable it and re-show your main menu panel
        gameObject.SetActive(false);
        // Example:
        // mainMenuPanel.SetActive(true);

        // If Credits is its own scene, load your menu scene here:
        // SceneManager.LoadScene("MainMenu");
    }
}

