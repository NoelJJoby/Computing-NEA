using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSelectViewUI : MonoBehaviour
{
    [SerializeField] Button saveFileTabPrefab;
    [SerializeField] Transform parentObject;
    private Button[] saveFileTabs;


    private void Start()
    {
        saveFileTabs = new Button[10];

        for (int i = 0; i < 10; i++)
        {
            Button tab = Instantiate(saveFileTabPrefab, parentObject, false);
            Vector3 position = new(0, 300 - (50 * i), 0);
            
            tab.transform.SetLocalPositionAndRotation(position, tab.transform.rotation);

            

            tab.GetComponentInChildren<TextMeshProUGUI>().text = "Save " + (i+1).ToString();
            int iC = i + 1;
            tab.onClick.AddListener(() =>
            {
                LevelDataController.selectedSaveNumber = iC;
            }
           );
            saveFileTabs[i] = tab;

        }
    }



}
