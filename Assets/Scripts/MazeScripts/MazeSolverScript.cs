using System.Collections.Generic;
using UnityEngine;



public readonly struct NodePair
{
    public NodePair(Vector2Int startVertex, Vector2Int endVertex)
    {
        StartVertex = startVertex;
        EndVertex = endVertex;
    }

    public Vector2Int StartVertex { get; }
    public Vector2Int EndVertex { get; }
}

public readonly struct PathDistanceDirectionPair
{
    public PathDistanceDirectionPair(int distance, Vector2Int previousVertex)
    {
        this.Distance = distance;
        this.PreviousVertex = previousVertex;
    }
    public int Distance { get; }
    public Vector2Int PreviousVertex { get; }

}


public class MazeSolver
{
    private readonly Graph maze;
    private List<Vector2Int> solutionPath;
    private readonly Dictionary<NodePair, List<Vector2Int>> pathList;

    public MazeSolver(Graph maze)
    {
        this.maze = maze;
        pathList = new Dictionary<NodePair, List<Vector2Int>>();


    }

    public List<Vector2Int> SolveDijkstra(Vector2Int startVertex, Vector2Int endVertex)
    {
        NodePair nodePair = new (startVertex, endVertex);
        if (pathList.ContainsKey(nodePair))
        {
            return pathList[nodePair];
        }


        solutionPath = new ();
        Dictionary<Vector2Int, PathDistanceDirectionPair> distances = new ();
        List<Vector2Int> explored = new();
        Queue<Vector2Int> verticesToVisit = new();


        verticesToVisit.Enqueue(startVertex);

        foreach (Vector2Int vertex in maze.GetVertices())
        {
            distances[vertex] = new PathDistanceDirectionPair(100000000, Vector2Int.one * -1);
        }

        distances[startVertex] = new PathDistanceDirectionPair(0, Vector2Int.one * -1);


        while (verticesToVisit.Count > 0)
        {
            Vector2Int current = verticesToVisit.Dequeue();
            if (explored.Contains(current))
            { continue; }
            explored.Add(current);

            foreach (Vector2Int neighbour in maze.GetEdges()[current])
            {
                int newDistance = distances[current].Distance + 1;

                if (newDistance < distances[neighbour].Distance)
                {
                    distances[neighbour] = new PathDistanceDirectionPair(newDistance, current);
                }
                verticesToVisit.Enqueue(neighbour);
            }

            if (current == endVertex)
            {
                break;
            }
        }

        Vector2Int prevVertex = endVertex;

        while (distances[prevVertex].PreviousVertex != startVertex)
        {
            solutionPath.Add(distances[prevVertex].PreviousVertex);
            prevVertex = distances[prevVertex].PreviousVertex;
        }

        pathList.Add(nodePair, solutionPath);
        return solutionPath;

    }

    public List<Vector2Int> SolveAStar(Vector2Int startVertex, Vector2Int endVertex)
    {
        NodePair pathStartEndPair = new (startVertex, endVertex);

        if (pathList.ContainsKey(pathStartEndPair))
        {
            return pathList[pathStartEndPair];
        }

        solutionPath = new List<Vector2Int>();

        // Dijkstra's Algorithm








        pathList.Add(pathStartEndPair, solutionPath);
        return solutionPath;
    }


}






