using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardUI : MonoBehaviour
{
    [SerializeField] GameObject scoreRowPrefab;
    private List<ScorePair> scoreList;
    private void Start()
    {
        scoreList = LevelDataController.GetScoreList();

        for (int i = 0; i < scoreList.Count; i++)
        {
            string name = scoreList[i].Name;
            int score = scoreList[i].Score;

            Vector3 position = new(0, 200 - (50 * i), 0);
            GameObject textObject = Instantiate(scoreRowPrefab, transform, false);
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            textObject.transform.SetLocalPositionAndRotation(position, rotation);

            TextMeshProUGUI textComp = textObject.GetComponent<TextMeshProUGUI>();
            textComp.text = (i + 1).ToString()+ ". " + name + "     " + score.ToString();
        }

    }


}
