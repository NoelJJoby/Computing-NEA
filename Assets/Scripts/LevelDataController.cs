using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;




public static class LevelDataController
{
    public enum GameOption
    {
        Unselected,
        New,
        Restart,
        Load
    }



    public static Vector2Int mazeSize;

    public static Graph MazeGraph { get; private set; }
    public static List<Vector2Int> GapPositionList { get; private set; }
    public static List<Vector2Int> SpawnerPostionList { get; private set; }
    public static List<Vector2Int> AllObstaclePositionList { get; private set; }
    public static Vector2Int StartVertex { get; private set; }
    public static Vector2Int EndVertex { get; private set; }


    private static List<ScorePair> scoreList = new();

    private static GameOption gameOption = GameOption.Unselected;

    public static int selectedSaveNumber = 1;



    public static bool LoadLevel()
    {
        switch (gameOption)
        {
            case GameOption.Unselected:
                return false;

            case GameOption.New:
                scoreList.Clear();

                MazeGraph = MazeGenerator.Instance.CreateNewMaze(GenAlg.RBT, mazeSize);
                GapPositionList = MazeGenerator.Instance.GapPositionList;
                SpawnerPostionList = MazeGenerator.Instance.SpawnerPostionList;
                AllObstaclePositionList = MazeGenerator.Instance.AllObstaclePositionList;
                StartVertex = MazeGenerator.Instance.StartVertex;
                EndVertex = MazeGenerator.Instance.EndVertex;
                return true;

            case GameOption.Restart:
                // no change in maze
                // You restart in the same maze
                return true;

            case GameOption.Load:
                scoreList.Clear();
                // get save number
                return LoadSavedLevel();

            default:
                break;
        }
        return false;
    }

    public static void SaveLevel()
    {
        // save in given save slot
        GameSaveData saveData = new();

        saveData.mazeData.mazeSize = mazeSize;

        saveData.mazeData.graphSaveData = MazeGraph.GetGraphSaveData();
        saveData.mazeData.gapPositionList = GapPositionList;
        saveData.mazeData.spawnerPostionList = SpawnerPostionList;
        saveData.mazeData.startVertex = StartVertex;
        saveData.mazeData.endVertex = EndVertex;

        saveData.scores = GetScoreList();


        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(saveData, true));
    }

    public static bool LoadSavedLevel()
    {
        string saveContent;
        try
        {
            saveContent = File.ReadAllText(SaveFileName());
        }
        catch (FileNotFoundException)
        {
            saveContent = null;
        }

        if (saveContent is null || saveContent == "") { return false; }

        GameSaveData savedData = JsonUtility.FromJson<GameSaveData>(saveContent);


        // setting to to static variables
        MazeGraph = new Graph(savedData.mazeData.graphSaveData);
        GapPositionList = savedData.mazeData.gapPositionList;
        SpawnerPostionList = savedData.mazeData.spawnerPostionList;
        mazeSize = savedData.mazeData.mazeSize;
        StartVertex = savedData.mazeData.startVertex;
        EndVertex = savedData.mazeData.endVertex;

        AllObstaclePositionList = new List<Vector2Int>(GapPositionList);
        AllObstaclePositionList.AddRange(SpawnerPostionList);

        scoreList = savedData.scores;
        return true;
    }

    private static string SaveFileName()
    {
        return Application.persistentDataPath + "/save_" + Authentication.GetCurrentUsername() + selectedSaveNumber.ToString() + ".save";
    }
    public static void AddScoreToList(int score)
    {
        ScorePair pair = new(Authentication.GetCurrentUsername(), score);
        scoreList.Add(pair);
    }

    public static void SetGameOption(GameOption option)
    {
        gameOption = option;
    }

    public static List<ScorePair> GetScoreList()
    {
        scoreList.Sort();
        scoreList.Reverse();
        return scoreList;
    }

    public static int GetObstacleCount()
    {
        return AllObstaclePositionList.Count;
    }

}


[Serializable]
public struct GameSaveData
{
    public List<ScorePair> scores;
    public MazeSaveData mazeData;
}


[System.Serializable]
public struct MazeSaveData
{

    public Vector2Int mazeSize;
    public Vector2Int startVertex;
    public Vector2Int endVertex;

    public GraphSaveData graphSaveData;
    public List<Vector2Int> gapPositionList;
    public List<Vector2Int> spawnerPostionList;


}



[Serializable]
public struct ScorePair : IComparable
{
    public string Name; public int Score;

    public ScorePair(string name, int score)
    {
        Name = name;
        Score = score;
    }

    public readonly int CompareTo(object obj)
    {
        if (obj == null) return 1;

        ScorePair other = (ScorePair)obj;

        return Score.CompareTo(other.Score);

    }
}