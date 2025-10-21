using TMPro;
using UnityEngine;

public class GameTimerTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameTimertext;

    // Update is called once per frame
    void Update()
    {
        gameTimertext.text = Mathf.CeilToInt(GameManager.Instance.GetGameTimer()).ToString();
    }
}
