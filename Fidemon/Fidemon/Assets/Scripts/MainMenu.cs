using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("Level_1");
    }
    public void ContinueGame()
    {
        // placeholder: si tenés guardado, cargar estado. Por ahora recarga Level_1.
        SceneManager.LoadScene("Level_1");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
