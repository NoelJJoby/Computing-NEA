using TMPro;
using UnityEngine;

public class DistanceCounterTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceCounterUIText;

    // Update is called once per frame
    void Update()
    {
        distanceCounterUIText.text = Mathf.CeilToInt(GameManager.Instance.GetPlayerDistanceToEnd()).ToString();
    }
}
