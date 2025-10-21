using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    [SerializeField] private Player player;

    [SerializeField] private GameObject gameViewObject;

    private Vector2Int startVertex;
    private Vector2Int endVertex;

    private MazeNode startNode;
    private MazeNode endNode;
    private MazeNode[,] mazeNodes;
    private List<Vector2Int> solutionPath;
    private List<MazeNode> playerPath;

    private float gameTimer;
    private bool gameFinished = false;
    private bool paused = false;
    private int score = 0;
    private bool gameStart = false;


    private float playerDistanceToEnd = 100000;


    public EventHandler OnGameOver;
    [SerializeField] private MapCamera mazeMapCamera;

    private void Awake()
    {
        Instance = this;

        GameInputProcessor.Instance.OnGamePause += GameInputProcessor_OnGamePause;
    }



    private void Start()
    {
        if (!LevelDataController.LoadLevel())
        {
            SceneController.MoveToScene(SceneController.SceneName.LevelSelectorScene);
            return;
        }
        StartGame();
    }


    void StartGame()
    {

        MazeRenderer.Instance.Render();

        mazeNodes = MazeRenderer.Instance.GetMazeCells();

        startVertex = LevelDataController.StartVertex;
        startNode = mazeNodes[startVertex.x, startVertex.y];
        startNode.SetStartNode();

        endVertex = LevelDataController.EndVertex;
        endNode = mazeNodes[endVertex.x, endVertex.y];
        endNode.SetEndNode();
        endNode.OnNodeCollidePlayer += EndNode_OnNodeCollide;



        player.OnPlayerHasFallen += Player_OnPlayerHasFallen;
        player.transform.position = startNode.transform.position + (2 * Vector3.up);
        player.gameObject.SetActive(true);
        gameViewObject.SetActive(true);


        MazeSolver mazeSolver = new(LevelDataController.MazeGraph);
        solutionPath = mazeSolver.SolveDijkstra(startVertex, endVertex);

        if (Authentication.CheckDebug())
        {
            MazeRenderer.Instance.RenderSolutionPath(solutionPath);
        }
        gameStart = true;

    }

    private void Update()
    {

        if (paused || gameFinished || !gameStart)
        {
            return;
        }


        gameTimer += Time.deltaTime;

        CalculatePlayerDistanceToEnd();


        score = (int)((2 * LevelDataController.mazeSize.sqrMagnitude) + solutionPath.Count + LevelDataController.GetObstacleCount() - gameTimer);
        score ^= 2;
    }


    public float GetPlayerDistanceToEnd()
    {
        return playerDistanceToEnd;
    }

    public float GetGameTimer()
    {
        return gameTimer;
    }

    public float GetScore()
    {
        return score;
    }


    private void CalculatePlayerDistanceToEnd()
    {
        playerDistanceToEnd = Vector3.Distance(player.transform.position, endNode.transform.position);
    }

    private void GameInputProcessor_OnGamePause(object sender, System.EventArgs e)
    {
        paused = !paused;
    }

    private void Player_OnPlayerHasFallen(object sender, System.EventArgs e)
    {
        score = 0;
        EndGame();


    }

    private void EndNode_OnNodeCollide(object sender, System.EventArgs e)
    {
        EndGame();
    }

    private void EndGame()
    {
        gameFinished = true;
        if (score > 0)
        {
            LevelDataController.AddScoreToList(score);
        }

        mazeMapCamera.gameObject.SetActive(true);
        mazeMapCamera.SetCameraToEndPos();

        GetPlayerPath();

        MazeRenderer.Instance.RenderSolutionPath(solutionPath);
        MazeRenderer.Instance.RenderPlayerSolutionPath(playerPath);

        OnGameOver?.Invoke(this, EventArgs.Empty);
    }


    private void GetPlayerPath()
    {
        playerPath = new List<MazeNode>();
        foreach (MazeNode node in mazeNodes)
        {
            if (node.HasPlayerTraversed())
            {
                playerPath.Add(node);
            }
        }

    }

    private void OnDestroy()
    {
        Instance = null;

    }

}

