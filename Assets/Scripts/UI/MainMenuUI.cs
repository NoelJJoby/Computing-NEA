using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button loginButton;
    [SerializeField] private GameObject loginPanel;


    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            SceneController.MoveToScene(SceneController.SceneName.LevelSelectorScene);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        loginButton.onClick.AddListener(() =>
        {
            if (loginPanel.activeSelf)
            {
                loginPanel.SetActive(false);
                loginButton.GetComponentInChildren<TextMeshProUGUI>().text = "Login";
            }
            else
            {
                loginPanel.SetActive(true);
                loginButton.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
            }
        });
    }
}

