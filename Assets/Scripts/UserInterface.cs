using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour
{
    PlayerControls controls;

    bool exitGame;
    bool goToMenu;

    void Start()
    {
        controls = new PlayerControls();
        controls.Enable();
    }

    private void Update()
    {
        ReadInput();

        if (exitGame)
            Application.Quit();
        if (goToMenu)
            SceneManager.LoadScene(0);
    }

    void ReadInput()
    {
        exitGame = controls.UserInterface.ExitGame.IsPressed();
        goToMenu = controls.UserInterface.GoMenu.IsPressed();
    }

    public void GoToMarioLevel()
    {
        SceneManager.LoadScene(1);
    }
}
