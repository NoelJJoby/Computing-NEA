using UnityEngine;
using UnityEngine.UI;

public class PauseViewUI : MonoBehaviour
{

    [SerializeField] private Button ExitButton;
    [SerializeField] private Button ShowControlsButton;
    [SerializeField] private Button SettingsButton;
    private void Awake()
    {
        ExitButton.onClick.AddListener(() =>
        {
            LevelDataController.SetGameOption(LevelDataController.GameOption.New);
            SceneController.MoveToScene(SceneController.SceneName.LevelSelectorScene);
        });

        ShowControlsButton.onClick.AddListener(() =>
        {

        });

        SettingsButton.onClick.AddListener(() =>
        {

        });


    }

}
