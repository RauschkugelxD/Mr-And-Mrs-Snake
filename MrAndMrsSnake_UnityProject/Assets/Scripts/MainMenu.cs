using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool IsMrSnake = false;
    public bool IsBlue = false;

    // Set values for character selection
    public void SetBlue()
    {
        IsBlue = true;
    }

    public void SetMrSnake()
    {
        IsMrSnake = true;
    }

    public void ResetValues()
    {
        IsBlue = false;
        IsMrSnake = false;
    }

    // Start the game by loading a new scene and set the chosen character
    public void PlaySnake()
    {
        if(IsMrSnake)
            SnakeMovement.IsMrSnake = true;

        if (IsBlue)
            SnakeMovement.IsBlue = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
