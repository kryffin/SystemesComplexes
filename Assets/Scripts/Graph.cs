using System.Collections.Generic;
using System.Text;

public class Graph
{

    public List<Node> nodes;
    public List<Path> paths;

    // Gets the index from a specific node (the index is the order you place your nodes in)
    public int GetIndex(Node n)
    {
        return nodes.IndexOf(n);
    }

    // Gets the node from the index (the index is the order you place your nodes in)
    public Node GetNode(int index)
    {
        int i = 0;
        foreach (Node n in nodes)
        {
            if (i == index) return n;
            i++;
        }

        return null;
    }

    // Gets a list of the node n's neighbors
    public List<Node> GetNeighborOf(Node n)
    {
        List<Node> neighbors = new List<Node>();

        foreach (Path p in paths)
        {
            if (p.from == n) neighbors.Add(p.to);
        }

        return neighbors;
    }

    // Gets the weight on a specific path
    public float GetWeight(int from, int to)
    {
        Node fromN = GetNode(from);
        Node toN = GetNode(to);

        foreach (Path p in paths)
        {
            if (p.from == fromN && p.to == toN) return p.weight;
        }

        return -1f;
    }

    // Adds all the ns nodes to the graph
    public void AddAllNodes(params Node[] ns)
    {
        for (int i = 0; i < ns.Length; i++)
        {
            nodes.Add(ns[i]);
        }
    }

    // Adds all the ps paths to the graph
    public void AddAllPaths(params Path[] ps)
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

    // Constructs a graph based on a .txt file
    public Graph(string filename)
    {
        nodes = new List<Node>();
        paths = new List<Path>();

        string[] lines = System.IO.File.ReadAllLines(filename);
        foreach (string line in lines)
        {
            string[] path = line.Split(' ');
            Node src = new Node(int.Parse(path[0]));
            Node dest = new Node(int.Parse(path[1]));
            if (!nodes.Contains(src)) nodes.Add(src);
            if (!nodes.Contains(dest)) nodes.Add(dest);
            paths.Add(new Path(src, dest, float.Parse(path[2])));
        }
    }

}
