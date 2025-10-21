using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool FirstUpdate = true;
    private void Update()
    {
        if (FirstUpdate)
        {
            FirstUpdate = false;
            SceneController.LoaderCallback();
        }
    }
}
