using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField] Transform followTranform;
    [SerializeField] MazeRenderer mazeRenderer;



    public void LateUpdate()
    {
        transform.SetPositionAndRotation(new Vector3(followTranform.position.x, 50, followTranform.position.z), Quaternion.Euler(90f, followTranform.eulerAngles.y, 0));
    }


    public void SetCameraToEndPos()
    {
        Camera camera = gameObject.GetComponent<Camera>();
        camera.orthographicSize = (int)(LevelDataController.mazeSize.magnitude * mazeRenderer.GetCellSizeScalar()/2);
    }
}
