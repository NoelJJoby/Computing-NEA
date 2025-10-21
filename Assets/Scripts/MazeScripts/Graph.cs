using System.Collections.Generic;
using UnityEngine;



public class Graph
{
    private readonly List<Vector2Int> vertices;
    private Dictionary<Vector2Int, List<Vector2Int>> edges;

    public Graph()
    {
        vertices = new List<Vector2Int>();
        edges = new Dictionary<Vector2Int, List<Vector2Int>>();
    }


    public Graph(GraphSaveData graphSaveData)
    {
        vertices = graphSaveData.vertices;
        
        CreateEdgesDictFromSave(graphSaveData.edgeKeys, graphSaveData.edgeEnds);

    }

    public List<Vector2Int> GetVertices() { return vertices; }
    public Dictionary<Vector2Int, List<Vector2Int>> GetEdges() { return edges; }

    public GraphSaveData GetGraphSaveData()
    {
        GraphSaveData graphSaveData = new()
        {
            vertices = vertices,
            edgeKeys = new(),
            edgeEnds = new()
        };


        foreach (var edgeRow in edges)
        {
            graphSaveData.edgeKeys.Add(edgeRow.Key);
            EndEdgesDataList endEdgesDataList = new()
            {
                endVertices = edgeRow.Value
            };
            graphSaveData.edgeEnds.Add(endEdgesDataList);
        }



        return graphSaveData;
    }



    public void AddVertex(int x, int y)
    {
        vertices.Add(new Vector2Int(x, y));
    }

    public void AddEdge(Vector2Int start, Vector2Int end)
    {
        if (!edges.ContainsKey(start))
        {
            edges.Add(start, new List<Vector2Int>());
        }

        edges[start].Add(end);

        if (!edges.ContainsKey(end))
        {
            edges.Add(end, new List<Vector2Int>());
        }

        edges[end].Add(start);
    } 

    public bool CheckEdge(Vector2Int start, Vector2Int end)
    {
        return edges.ContainsKey(start) && edges[start].Contains(end);
    }
    public void CreateEdgesDictFromSave(List<Vector2Int> edgeKeys, List<EndEdgesDataList> edgeEnds)
    {
        edges = new Dictionary<Vector2Int, List<Vector2Int>>();
        int i = 0;
        while (i < edgeKeys.Count)
        {
            edges.Add(edgeKeys[i], edgeEnds[i].endVertices);
            i++;
        }
    }
}


[System.Serializable]
public struct GraphSaveData
{
    public List<Vector2Int> vertices;
    public List<Vector2Int> edgeKeys;
    public List<EndEdgesDataList> edgeEnds;
}


[System.Serializable]
public struct EndEdgesDataList
{
    public List<Vector2Int> endVertices;
}

