using UnityEngine;

public class NodeCollider : MonoBehaviour
{
    [SerializeField] MazeNode mazeNode;
    private void OnTriggerEnter(Collider other)
    {
        mazeNode.InvokeColliderEvent(other.gameObject);
    }
}
