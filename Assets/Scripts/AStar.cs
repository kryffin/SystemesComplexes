using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private MeshGenerator mg;

    private List<Node> openList;
    private List<Node> closedList;

    public float[] distances;
    public Node[] previous;

    public float magnitude;

    public void Init(Graph g, MeshGenerator mg)
    {
        this.mg = mg;

        openList = new List<Node>();
        closedList = new List<Node>();

        distances = new float[g.nodes.Count];
        previous = new Node[g.nodes.Count];

        foreach (Node n in g.nodes)
        {
            distances[n.value] = float.MaxValue;
            previous[n.value] = null;
        }

    }

    private Node MinDistance()
    {
        float min = float.MaxValue;
        Node X = new Node(-1);
        foreach (Node n in openList)
        {
            if (distances[n.value] < min)
            {
                min = distances[n.value];
                X = n;
            }
        }

        return X;
    }

    private int DistanceFromSource(Node Y)
    {
        Node prev = previous[Y.value];
        int dist = 0;
        while (prev != null)
        {
            dist++;

            prev = previous[prev.value];
        }

        return dist;
    }

    private float DistanceToDestination(Node Y, Node dest)
    {
        // gestion 2d du machin
        /*return Mathf.Max(Mathf.Abs(mg.GetVerticePosition(dest.value).x - mg.GetVerticePosition(Y.value).x),
                         Mathf.Abs(mg.GetVerticePosition(dest.value).z - mg.GetVerticePosition(Y.value).z));*/

        // Seems to be working ??? although 10f might be a bit too much (scared of heights)
        // nope
        return Mathf.Abs(mg.GetVerticePosition(Y.value).y - mg.GetVerticePosition(dest.value).y) * 1.5f;
    }

    private float Weight(Node Y, Node dest)
    {
        return DistanceFromSource(Y) + DistanceToDestination(Y, dest);
    }

    public void Run(Graph g, Node source, Node destination, MeshGenerator mg)
    {
        Init(g, mg);

        openList.Add(source);
        distances[source.value] = 0f;

        while (openList.Count != 0)
        {
            Node X = MinDistance();
            if (X == destination) return;

            closedList.Add(X);
            openList.Remove(X);

            foreach (Node Y in g.GetNeighborOf(X))
            {
                if (Y.obstacle) continue;

                if (!openList.Contains(Y) && !closedList.Contains(Y))
                {
                    openList.Add(Y);
                    distances[Y.value] = Weight(Y, destination);
                    previous[Y.value] = X;
                }
            }
        }
    }

    // Returns a list of node which is the shortest path towards a given ending node
    public List<Node> TraceBack(Node end)
    {
        List<Node> nodes = new List<Node>();
        nodes.Add(end);
        Node cur = end;
        while (previous[cur.value] != null)
        {
            cur = previous[cur.value];
            nodes.Add(cur);
        }

        return nodes;
    }

}
