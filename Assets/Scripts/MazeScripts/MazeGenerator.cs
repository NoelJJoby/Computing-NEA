using System.Collections.Generic;
using UnityEngine;

public enum GenAlg
{
    RBT,
    Kruskal,
    Prim,
    Eller
}

public enum ObstacleType
{
    Gaps,
    Spawner
}


public class MazeGenerator : MonoBehaviour
{
    public static MazeGenerator Instance { get; private set; }
    private Vector2Int mazeSize;
    private Graph maze;

    public Vector2Int StartVertex { get; private set; }
    public Vector2Int EndVertex { get; private set; }

    public List<Vector2Int> GapPositionList { get; private set; }
    public List<Vector2Int> SpawnerPostionList { get; private set; }
    public List<Vector2Int> AllObstaclePositionList { get; private set; }


    private void Awake()
    {
        Instance = this;
    }


    public Graph CreateNewMaze(GenAlg genAlg, Vector2Int mazeSize)
    {
        maze = new Graph();
        this.mazeSize = mazeSize;
        CreateBaseGraph(mazeSize);

        // check which generation algorithm is selected
        switch (genAlg)
        {
            case GenAlg.RBT:
                RBTMazeGenerator mazeGen = new();
                maze = mazeGen.GenerateMaze(maze, mazeSize);
                //Debug.Log(maze);
                break;
            case GenAlg.Kruskal:

                break;
            case GenAlg.Prim:
                break;
            case GenAlg.Eller:
                break;

        }


        // add multiple solutions to maze by adding cycles
        AddCycles();

        // Adding Obstacles to the maze Randomly

        GapPositionList = new List<Vector2Int>();
        SpawnerPostionList = new List<Vector2Int>();
        AllObstaclePositionList = new List<Vector2Int>();


        AddObstacleByType(ObstacleType.Gaps);
        AddObstacleByType(ObstacleType.Spawner);

        SetStartEndVertices();


        return maze;
    }

    private void AddCycles()
    {
        // randomly select a number of imperfections for the maze based on maze's size
        int imperfectionCount = Random.Range(3, (int)mazeSize.magnitude);

        while (imperfectionCount > 0)
        {
            // randomly select a vertex of the graph
            int randX = Random.Range(0, mazeSize.x);
            int randY = Random.Range(0, mazeSize.y);
            Vector2Int startVertex = new(randX, randY);

            List<Vector2Int> possibleDirections = FindPossibleDirections(startVertex, mazeSize);

            // checks all directions to see which edges aren't already created
            foreach (Vector2Int direction in possibleDirections)
            {
                Vector2Int endVertex = startVertex + direction;
                if (!maze.CheckEdge(startVertex, endVertex))
                {
                    maze.AddEdge(startVertex, endVertex);
                    imperfectionCount--;
                }
            }
        }
    }

    private void AddObstacleByType(ObstacleType obstacleType)
    {
        int obstacleCount = obstacleType switch
        {
            ObstacleType.Gaps => Random.Range(2, (int)mazeSize.magnitude),
            ObstacleType.Spawner => Random.Range(2, 2 * (int)Mathf.Sqrt(mazeSize.magnitude)),
            _ => 2,
        };
        while (obstacleCount > 0)
        {
            // randomly select a vertex of the graph
            int randX = Random.Range(0, mazeSize.x);
            int randY = Random.Range(0, mazeSize.y);
            Vector2Int vertex = new(randX, randY);

            switch (obstacleType)
            {
                case ObstacleType.Gaps:
                    if (!AllObstaclePositionList.Contains(vertex))
                    {
                        GapPositionList.Add(vertex);
                        AllObstaclePositionList.Add(vertex);
                        obstacleCount--;
                    }
                    break;

                case ObstacleType.Spawner:
                    if (!AllObstaclePositionList.Contains(vertex))
                    {
                        SpawnerPostionList.Add(vertex);
                        AllObstaclePositionList.Add(vertex);
                        obstacleCount--;
                    }
                    break;
            }

        }
    }


    public static List<Vector2Int> FindPossibleDirections(Vector2Int currentVertex, Vector2Int mazeSize)
    {
        List<Vector2Int> possibleDirections = new();

        // check if Vertex is on right edge of maze
        if (currentVertex.x < mazeSize.x - 1)
        {
            possibleDirections.Add(new Vector2Int(1, 0));
        }

        // check if Vertex is on left edge of maze
        if (currentVertex.x > 0)
        {
            possibleDirections.Add(new Vector2Int(-1, 0));
        }

        //check if Vertex is on bottom edge of maze
        if (currentVertex.y < mazeSize.y - 1)
        {
            possibleDirections.Add(new Vector2Int(0, 1));
        }

        // check if Vertex is on top edge of maze
        if (currentVertex.y > 0)
        {
            possibleDirections.Add(new Vector2Int(0, -1));
        }
        return possibleDirections;
    }


    private void CreateBaseGraph(Vector2Int mazeSize)
    {
        // adds all vertecies to the maze graph
        for (int x = 0; x < mazeSize.x; x++)
        {
            for (int y = 0; y < mazeSize.y; y++)
            {
                maze.AddVertex(x, y);
            }
        }
    }

    private void SetStartEndVertices()
    {
        while (true)
        {
            StartVertex = new Vector2Int(Random.Range(0, mazeSize.x), 0);
            if (!AllObstaclePositionList.Contains(StartVertex))
            {
                break;
            }
        }

        while (true)
        {
            EndVertex = new Vector2Int(Random.Range(0, mazeSize.x), mazeSize.y - 1);
            if (!AllObstaclePositionList.Contains(EndVertex))
            {
                break;
            }
        }
    }





    public void OnDestroy()
    {
        Instance = null;
    }

}