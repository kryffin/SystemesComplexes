using System.Collections.Generic;
using UnityEngine;

public class Dijkstra
{

    private Graph g;

    private float[] distances;
    private Node[] previous;
    private HashSet<Node> Q;

    private void Init(int source)
    {
        distances = new float[g.nodes.Count];
        previous = new Node[g.nodes.Count];
        Q = new HashSet<Node>();

        int i = 0;
        foreach (Node n in g.nodes)
        {
            distances[i] = float.MaxValue;
            previous[i] = null;

            i++;
        }
        distances[source] = 0f;
    }

    private Node MinDistance(HashSet<Node> Q)
    {
        float min = float.MaxValue;
        Node node = null;

        int i = 0;
        foreach (Node n in Q)
        {
            if (distances[i] < min)
            {
                min = distances[i];
                node = n;
            }

            i++;
        }

        return node;
    }

    private void UpdateDistances(int s1, int s2)
    {
        float weight = g.getWeight(s1, s2);

        if (distances[s2] > distances[s1] + weight)
        {
            distances[s2] = distances[s1] + weight;
            previous[s2] = g.getNode(s1);
        }
    }

    private void DisplayResults()
    {
        Debug.Log("Les distances sont :");
        int i = 0;
        foreach (float f in distances)
        {
            Debug.Log("d[" + (i++) + "] = " + f);
        }

        Debug.Log("Les pr�c�dents sont :");
        i = 0;
        foreach (Node n in previous)
        {
            Debug.Log("prev[" + (i++) + "] = " + n);
        }
        Debug.Log("Fin");
    }

    public void Run(Graph g, int source)
    {
        this.g = g;
        Init(source);

        foreach (Node n in g.nodes) Q.Add(n);

        Node s1;
        while (Q.Count != 0)
        {
            s1 = MinDistance(Q);
            Q.Remove(s1);

            foreach (Path p in g.paths)
            {
                if (p.from == s1) UpdateDistances(g.getIndex(s1), g.getIndex(p.to));
            }
        }

        DisplayResults();
    }

}