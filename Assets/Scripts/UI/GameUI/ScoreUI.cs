using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Update()
    {
        scoreText.text = "Score: " + ((int) GameManager.Instance.GetScore()).ToString();
    }
}
