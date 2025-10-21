using System;
using UnityEngine;



public enum NodeType
{
    Start,
    End,
    Passage,
    Path,
    Gap,
    Spawner

}

 
public class MazeNode : MonoBehaviour
{
    [SerializeField] private GameObject PosXWall;
    [SerializeField] private GameObject PosZWall;
    [SerializeField] private GameObject NegXWall;
    [SerializeField] private GameObject NegZWall;
    [SerializeField] private GameObject Floor;
    [SerializeField] private GameObject nodeCollider;


    [SerializeField] private Material FloorPlayerMat;
    [SerializeField] private Material FloorStartMat;
    [SerializeField] private Material FloorEndMat;
    [SerializeField] private Material FloorSolutionPathMat;
    [SerializeField] Material FloorSpawnerTempMat;


    public event EventHandler OnNodeCollidePlayer;
    private bool playerHasTraversed;




    private NodeType nodeType = NodeType.Passage;
    private bool nodeTypeSet = false;

    public NodeType GetNodeType() { return nodeType; }

    public bool GetNodeTypeSet() { return nodeTypeSet; }

    public void SetStartNode()
    {
        if (nodeTypeSet)
        {
            Debug.Log("Node type already set ");
        }
        nodeType = NodeType.Start;
        nodeTypeSet = true;
        Floor.GetComponent<MeshRenderer>().material = FloorStartMat;
    }

    public void SetEndNode()
    {
        if (nodeTypeSet)
        {
            Debug.Log("Node type already set ");
        }

        nodeType = NodeType.End;
        nodeTypeSet = true;
        Floor.GetComponent<MeshRenderer>().material = FloorEndMat;
    }

    public void SetGapNode()
    {
        if (nodeTypeSet)
        {
            Debug.Log("Node type already set ");
        }

        nodeType = NodeType.Gap;
        nodeTypeSet = true;
        DeactivateFloor();


    }

    public void SetSpawnerNode()
    {
        if (nodeTypeSet)
        {
            Debug.Log("Node type already set ");
        }

        nodeType = NodeType.Spawner;
        nodeTypeSet = true;

        // Implement a spawner object
        Floor.GetComponent<MeshRenderer>().material = FloorSpawnerTempMat;
    }

    public void SetPathNode(bool player)
    {
        if (!nodeTypeSet)
        {
            if (player)
            {
                Floor.GetComponent<MeshRenderer>().material = FloorPlayerMat;
            }
            else
            {
                Floor.GetComponent<MeshRenderer>().material = FloorSolutionPathMat;
            }

            nodeType = NodeType.Path;
            nodeTypeSet = true;
        }
    }




    public void DeactivateFloor()
    {
        Floor.SetActive(false);
    }

    public void DeactivatePosXWall()
    {
        PosXWall.SetActive(false);
    }
    public void DeactivatePosZWall()
    {
        PosZWall.SetActive(false);
    }
    public void DeactivateNegXWall()
    {
        NegXWall.SetActive(false);
    }
    public void DeactivateNegZWall()
    {
        NegZWall.SetActive(false);
    }

    public void InvokeColliderEvent(GameObject collidedObject)
    {
        if (collidedObject.TryGetComponent<Player>(out Player player))
        {
            if (nodeType == NodeType.End)
            {
                OnNodeCollidePlayer?.Invoke(this, EventArgs.Empty);
            }
            playerHasTraversed = true;
        }

    }

    public bool HasPlayerTraversed()
    {
        return playerHasTraversed;
    }

}
