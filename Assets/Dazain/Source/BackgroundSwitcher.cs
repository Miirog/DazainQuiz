using UnityEngine;
using UnityEngine.UI;

public class BackgroundSwitcherResponsive : MonoBehaviour
{
    public Sprite horizontalBackground; // Assign your background for wider screens
    public Sprite verticalBackground;   // Assign your background for taller screens
    public Image backgroundRenderer; // Assign the SpriteRenderer component

    [SerializeField] private Sprite _endgameDesktopBackground;
    [SerializeField] private Sprite _endgameMobileBackground;
    [SerializeField] private Image _bgImage;

    [SerializeField] private Button _startButton;

    [SerializeField] private GameObject[] _desktopObjects;
    [SerializeField] private GameObject[] _mobileObjects;

    void Start()
    {
        if (backgroundRenderer == null)
        {
            Debug.LogError("Background Renderer not assigned!");
            return;
        }

        CheckOrientation();
    }

    void Update()
    {
        // Check for orientation changes if the screen can be resized
        if (Screen.width != previousWidth || Screen.height != previousHeight)
        {
            CheckOrientation();
        }
        previousWidth = Screen.width;
        previousHeight = Screen.height;
    }

    private int previousWidth;
    private int previousHeight;

    void CheckOrientation()
    {
        float aspectRatio = (float)Screen.width / Screen.height;

        if (aspectRatio > 1f)
        {
            // Screen is wider than it is tall (horizontal or landscape)
            if (horizontalBackground != null)
            {
                backgroundRenderer.sprite = horizontalBackground;
                _bgImage.sprite = _endgameDesktopBackground;
                SetActive(_mobileObjects, false);
                SetActive(_desktopObjects, true);
            }
            else
            {
                Debug.LogWarning("Horizontal background sprite not assigned!");
            }
        }
        else if (aspectRatio < 1f)
        {
            // Screen is taller than it is wide (vertical or portrait)
            if (verticalBackground != null)
            {
                backgroundRenderer.sprite = verticalBackground;
                _bgImage.sprite = _endgameMobileBackground;
                SetActive(_desktopObjects, false);
                SetActive(_mobileObjects, true);
            }
            else
            {
                Debug.LogWarning("Vertical background sprite not assigned!");
            }
        }
        else
        {
            // Aspect ratio is approximately 1:1 (square)
            // You can decide what to do in this case, e.g., use a default background
            Debug.Log("Screen aspect ratio is approximately 1:1.");
            // if (defaultBackground != null) { backgroundRenderer.sprite = defaultBackground; }
        }
    }

    private void SetActive(GameObject[] buttons, bool isActive)
    {
        foreach (var button in buttons)
        {
            button.SetActive(isActive);
        }
    }
}