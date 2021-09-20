using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Graph
{
    public List<Node> nodes;
    public List<Path> paths;

    //Gets the index from a specific node (the index is the order you place your nodes in)
    public int getIndex(Node n)
    {
        return nodes.IndexOf(n);
    }

    //Gets the node from the index (the index is the order you place your nodes in)
    public Node getNode(int index)
    {
        int i = 0;
        foreach (Node n in nodes)
        {
            if (i == index) return n;
            i++;
        }

        return null;
    }

    //Gets a list of the node n's neighbors
    public List<Node> getNeighborOf(Node n)
    {
        List<Node> neighbors = new List<Node>();

        foreach (Path p in paths)
        {
            if (p.from == n) neighbors.Add(p.to);
        }

        return neighbors;
    }

    //Gets the weight on a specific path
    public float getWeight(int from, int to)
    {
        Node fromN = getNode(from);
        Node toN = getNode(to);

        foreach (Path p in paths)
        {
            if (p.from == fromN && p.to == toN) return p.weight;
        }

        return -1f;
    }

    //Adds all the ns nodes to the graph
    public void addAllNodes(params Node[] ns)
    {
        for (int i = 0; i < ns.Length; i++)
        {
            nodes.Add(ns[i]);
        }
    }

    //Adds all the ps paths to the graph
    public void addAllPaths(params Path[] ps)
    {
        for (int i = 0; i < ps.Length; i++)
        {
            paths.Add(ps[i]);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        foreach (Path p in paths)
        {
            sb.Append("\t" + p + "\n");
        }

        return "G{\n" + sb.ToString() + "\n}";
    }

    public Graph()
    {
        nodes = new List<Node>();
        paths = new List<Path>();
    }

}