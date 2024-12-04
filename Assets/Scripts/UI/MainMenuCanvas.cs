using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void SettingsPanel(bool isOpen)
    {
        settingsPanel.SetActive(isOpen);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
