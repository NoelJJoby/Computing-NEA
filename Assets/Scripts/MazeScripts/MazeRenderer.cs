using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    public static MazeRenderer Instance { private set; get; }
    private Graph mazeGraph; 
    [SerializeField] GameObject cellPrefab;
    [SerializeField] private int cellSizeScalar;

    [SerializeField] GameObject enemySpawnerPrefab;

    private MazeNode[,] MazeNodes;
    private Vector2Int mazeSize;
    public void Awake()
    {
        Instance = this;
    }



    public void Render()
    {
        this.mazeGraph = LevelDataController.MazeGraph;
        this.mazeSize = LevelDataController.mazeSize;
        RenderMaze();
        RenderObstacles();
    }

    public MazeNode[,] GetMazeCells()
    {
        return MazeNodes;
    }

    private void RenderMaze()
    {

        MazeNodes = new MazeNode[mazeSize.x, mazeSize.y];
        foreach (Vector2Int vertex in mazeGraph.GetVertices())
        {
            int x = (vertex.x - (mazeSize.x / 2)) * cellSizeScalar;
            int z = (vertex.y - (mazeSize.y / 2)) * cellSizeScalar;
            Vector3 pos = new Vector3(x, 0, z) + transform.position;

            GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity);
            cell.transform.localScale *= cellSizeScalar;


            MazeNodes[vertex.x, vertex.y] = cell.GetComponent<MazeNode>();
            cell.transform.parent = transform;

        }

        // Add Edges
        foreach (var edge in mazeGraph.GetEdges())
        {
            Vector2Int startNode = edge.Key;

            foreach (Vector2Int endNode in edge.Value)
            {
                Vector2Int direction = endNode - startNode;

                if (direction == Vector2Int.up)
                {
                    MazeNodes[startNode.x, startNode.y].DeactivatePosZWall();
                    MazeNodes[endNode.x, endNode.y].DeactivateNegZWall();
                }
                else if (direction == Vector2Int.down)
                {
                    MazeNodes[startNode.x, startNode.y].DeactivateNegZWall();
                    MazeNodes[endNode.x, endNode.y].DeactivatePosZWall();
                }
                else if (direction == Vector2Int.right)
                {
                    MazeNodes[startNode.x, startNode.y].DeactivatePosXWall();
                    MazeNodes[endNode.x, endNode.y].DeactivateNegXWall();
                }
                else if (direction == Vector2Int.left)
                {
                    MazeNodes[startNode.x, startNode.y].DeactivateNegXWall();
                    MazeNodes[endNode.x, endNode.y].DeactivatePosXWall();
                }

            }

        }

        StaticBatchingUtility.Combine(gameObject);
    }

    private void RenderObstacles()
    {
        foreach (Vector2Int vertex in LevelDataController.GapPositionList)
        {
            MazeNodes[vertex.x, vertex.y].SetGapNode();
        }

        foreach (Vector2Int vertex in LevelDataController.SpawnerPostionList)
        {
            MazeNode node = MazeNodes[vertex.x, vertex.y];

            node.SetSpawnerNode();

            GameObject enemySpawner = Instantiate(enemySpawnerPrefab, node.transform.position + Vector3.up, Quaternion.identity);


        }
    }


    public void RenderSolutionPath(List<Vector2Int> path)
    {
        foreach (Vector2Int pathVertex in path)
        {
            MazeNodes[pathVertex.x, pathVertex.y].SetPathNode(false);
        }
    }

    public void RenderPlayerSolutionPath(List<MazeNode> playerPath)
    {
        foreach (MazeNode node in playerPath)
        {
                node.SetPathNode(true);
            
        }
    }

    public int GetCellSizeScalar()
    { 
        return cellSizeScalar;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
