using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Dijkstra
{

    private Graph g;

    public float[] distances; //distances towards each node (index) from the starting node
    public Node[] previous; //previous node to the index relative to the shortest path found

    // Initializes the algorithm
    private void Init(int source)
    {
        distances = new float[g.nodes.Count];
        previous = new Node[g.nodes.Count];

        int i = 0;
        foreach (Node n in g.nodes)
        {
            distances[i] = float.MaxValue;
            previous[i] = null;

            i++;
        }
        distances[source] = 0f;
    }

    // returns the minimum distance node based on a graph's subset Q
    private Node MinDistance(HashSet<Node> Q)
    {
        float min = float.MaxValue;
        Node node = null;

        foreach (Node n in Q)
        {
            if (distances[n.value] < min)
            {
                min = distances[n.value];
                node = n;
            }
        }

        return node;
    }

    // Updates distances between two nodes
    private void UpdateDistances(Node s1, Node s2)
    {
        float weight = g.GetWeight(s1.value, s2.value);

        if (distances[s2.value] > distances[s1.value] + weight)
        {
            distances[s2.value] = distances[s1.value] + weight;
            previous[s2.value] = g.GetNode(s1.value);
        }
    }

    // Displays the algorithm results
    private void DisplayResults(Node source)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("------------Dijkstra-------------\n");
        sb.Append("Source : " + source.value + "\n");
        sb.Append("Les distances sont :\n");
        int i = 0;
        foreach (float f in distances)
        {
            sb.Append("\td[" + (i++) + "] = " + f + "\n");
        }

        sb.Append("Les précédents sont :\n");
        i = 0;
        foreach (Node n in previous)
        {
            sb.Append("\tprev[" + (i++) + "] = " + n + "\n");
        }
        sb.Append("---------------------------------");

        Debug.Log(sb.ToString());
    }
    
    // Displays the Dijkstra results only if the previous array is glitched (used to debug only)
    private void DisplayResultsIfOneEmpty(Node source)
    {
        bool display = false;
        foreach (Node p in previous)
        {
            if (p == null && p != source) display = true;
        }

        if (!display) return;

        DisplayResults(source);
    }

    // Returns a list of node which is the shortest path towards a given ending node
    public List<Node> TraceBack (Node end)
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

    // Runs the Dijkstra algorithm on given graph from given starting node
    public void Run(Graph g, Node source)
    {
        this.g = g;
        Init(source.value);
        HashSet<Node> Q = new HashSet<Node>();

        foreach (Node n in g.nodes) Q.Add(n);

        Node s1;
        while (Q.Count != 0)
        {
            s1 = MinDistance(Q);
            Q.Remove(s1);

            foreach (Path p in g.paths)
            {
                if (p.from == s1) UpdateDistances(s1, p.to);
            }
        }

        //DisplayResults(source);
        //DisplayResultsIfOneEmpty(source);
    }

}
