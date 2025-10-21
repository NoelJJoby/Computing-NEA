using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorUI : MonoBehaviour
{
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button LoadGameButton;
    [SerializeField] private Button StartGameButton;
    [SerializeField] private Button ReturnButton;

    [SerializeField] private GameObject NewGameSettings;
    [SerializeField] private GameObject LoadGameSettings;


    private void Awake()
    {
        LevelDataController.mazeSize = new(8, 8);


        NewGameButton.onClick.AddListener(() =>
        {
            NewGameSettings.SetActive(true);
            LoadGameSettings.SetActive(false);
            LevelDataController.SetGameOption(LevelDataController.GameOption.New);
            StartGameButton.gameObject.SetActive(true);

        });

        LoadGameButton.onClick.AddListener(() =>
        {

            LoadGameSettings.SetActive(true);
            NewGameSettings.SetActive(false);
            LevelDataController.SetGameOption(LevelDataController.GameOption.Load);
            StartGameButton.gameObject.SetActive(true);



        });

        StartGameButton.onClick.AddListener(() =>
        {
            SceneController.MoveToScene(SceneController.SceneName.GameScene);
        });

        ReturnButton.onClick.AddListener(() =>
        {
            SceneController.MoveToScene(SceneController.SceneName.MainMenuScene);
        });
    }

    private void Start()
    {
        LevelDataController.SetGameOption(LevelDataController.GameOption.Unselected);
        LoadGameButton.interactable = Authentication.CheckAuthenticated() || Authentication.CheckDebug();
        StartGameButton.gameObject.SetActive(false);
    }
}
