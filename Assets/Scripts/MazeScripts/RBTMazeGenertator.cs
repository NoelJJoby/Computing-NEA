using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RBTMazeGenerator
{
    public Graph GenerateMaze(Graph maze, Vector2Int mazeSize)
    {
        List<Vector2Int> ExploredVertices = new();

        // choosing random starting point
        int randX = Random.Range(0, mazeSize.x);
        int randY = Random.Range(0, mazeSize.y);
        Vector2Int startVertex = new(randX, randY);

        // Recursive Perfect Maze Generation Procedure
        RecursiveBacktraker(maze, startVertex, ExploredVertices, mazeSize);


        return maze;
    }

    private void RecursiveBacktraker(Graph maze, Vector2Int current_Vertex, List<Vector2Int> ExploredVertices, Vector2Int mazeSize)
    {
        ExploredVertices.Add(current_Vertex);
        List<Vector2Int> possible_directions = MazeGenerator.FindPossibleDirections(current_Vertex, mazeSize);

        // randomly shuffles the directions list
        possible_directions = possible_directions.OrderBy(a => Random.Range(0, 5)).ToList();

        // checks if all adjacent tiles have been traversed
        while (possible_directions.Count > 0)
        {
            // selects a direction
            Vector2Int direction = possible_directions[0];
            possible_directions.Remove(direction);

            // finds the new Vertex given by a movement in that direction
            Vector2Int new_Vertex = current_Vertex + direction;

            // checks if the new Vertex has already been explored
            if (!ExploredVertices.Contains(new_Vertex))
            {
                maze.AddEdge(current_Vertex, new_Vertex);

                // recursive call
                RecursiveBacktraker(maze, new_Vertex, ExploredVertices, mazeSize);
            }
        }
    }






}
