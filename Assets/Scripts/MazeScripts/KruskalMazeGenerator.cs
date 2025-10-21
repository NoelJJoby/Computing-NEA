using UnityEngine;
using Random = UnityEngine.Random;

public class KruskalMazeGenerator
{


    public static Graph GenerateMaze(Graph maze, Vector2Int mazeSize)
    {
        int[,] setDistribution = new int[mazeSize.x, mazeSize.y];
        for (int i = 0; i < mazeSize.x; i++)
        {
            for (int j = 0; j < mazeSize.y; j++)
            {
                setDistribution[i, j] = i + j;
            }
        }

        //KruskalAlg(maze, setDistribution, mazeSize);

        return maze;
    }

    // kruskal perfect maze generator
    //public static void KruskalAlg(Graph maze, int[,] setDistribution, Vector2Int mazeSize)
    //{

    //    // choosing random starting point
    //    int randX = Random.Range(0, mazeSize.x);
    //    int randY = Random.Range(0, mazeSize.y);
    //    Vector2Int startVertex = new Vector2Int(randX, randY);





    //}



}