using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MazeSliderUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MazeSizeXText;
    [SerializeField] private Slider MazeSizeXSlider;
    [SerializeField] private TextMeshProUGUI MazeSizeYText;
    [SerializeField] private Slider MazeSizeYSlider;


    private void Awake()
    {
        MazeSizeXSlider.onValueChanged.AddListener((value) =>
        {
            MazeSizeXText.text = "Maze Size X: " + value.ToString();
            LevelDataController.mazeSize.x = (int)value;
        });

        MazeSizeYSlider.onValueChanged.AddListener((value) =>
        {
            MazeSizeYText.text = "Maze Size Y: " + value.ToString();
            LevelDataController.mazeSize.y = (int)value;
        });
    }


}
