using System;
using UnityEngine;
using UnityEngine.UI;
public class RestartLevel : MonoBehaviour
{
    [SerializeField] private Button[] restartButton; // Reference to the restart button
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Add listener to the restart button
        foreach (var button in restartButton)
        {
            button.onClick.AddListener(OnRestartButtonClicked);
        }
    }

    private void OnRestartButtonClicked()
    {
        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
