using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public Canvas settingsMenu;
    public GameObject controlsPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsMenu.gameObject.activeSelf)
            {
                CloseSettings();
            }
            else
            {
                OpenSettings();
            }
        }
    }

    public void OpenSettings()
    {
        settingsMenu.gameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        HideControls();
        settingsMenu.gameObject.SetActive(false);
    }

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
    }

    public void HideControls()
    {
        controlsPanel.SetActive(false);
    }

    public void LoadLevel1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
    }

    public void LoadLevel3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
