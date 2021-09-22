using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijsktraRiver : MonoBehaviour
{

    public int width, height;

    private MeshGenerator mg;

    private Graph g;

    private List<Node> finalPath;

    // Initializes every nodes
    private void InitNodes()
    {
        for (int i = 0; i < width * height; i++)
        {
            g.nodes.Add(new Node(i + ""));
        }
    }

    // Initializes paths for every node to its neighbors
    private void InitPaths()
    {
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                // VOISINS DIRECTS
                if (h != 0) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h - 1) + w],
                    Mathf.Abs(mg.GetVerticePosition(width * h + w).y - mg.GetVerticePosition(width * (h - 1) + w).y)));

                if (w != 0) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * h + (w - 1)],
                    Mathf.Abs(mg.GetVerticePosition(width * h + w).y - mg.GetVerticePosition(width * h + (w - 1)).y)));

                if (h != height - 1) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h + 1) + w],
                    Mathf.Abs(mg.GetVerticePosition(width * h + w).y - mg.GetVerticePosition(width * (h + 1) + w).y)));

                if (w != width - 1) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * h + (w + 1)],
                    Mathf.Abs(mg.GetVerticePosition(width * h + w).y - mg.GetVerticePosition(width * h + (w + 1)).y)));

                // DIAGONALES
                if (h != 0 && w != width - 1) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h - 1) + (w + 1)],
                    Mathf.Abs(mg.GetVerticePosition(width * h + w).y - mg.GetVerticePosition(width * (h - 1) + (w + 1)).y))); //sud est
                if (w != 0 && h != height - 1) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h + 1) + (w - 1)],
                    Mathf.Abs(mg.GetVerticePosition(width * h + w).y - mg.GetVerticePosition(width * (h + 1) + (w - 1)).y))); // nord ouest
                if (h != height - 1 && w != width - 1) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h + 1) + (w + 1)],
                    Mathf.Abs(mg.GetVerticePosition(width * h + w).y - mg.GetVerticePosition(width * (h + 1) + (w + 1)).y))); //nord est
                if (w != 0 && h != 0) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h - 1) + (w - 1)],
                    Mathf.Abs(mg.GetVerticePosition(width * h + w).y - mg.GetVerticePosition(width * (h - 1) + (w - 1)).y))); //sud ouest
            }
        }
    }

    void Start()
    {
        mg = GetComponent<MeshGenerator>();
        mg.Init();

        g = new Graph();

        InitNodes();
        InitPaths();

        Dijkstra d = new Dijkstra();
        int src = Random.Range(width * (height - 1), (width * height) - 1);
        d.Run(g, src);

        float minValue = float.MaxValue;
        Node closest = new Node("0");
        for (int i = 0; i < width; i++)
        {
            if (minValue > d.distances[i])
            {
                minValue = d.distances[i];
                closest = new Node(i + "");
            }
        }
        Debug.Log(minValue);

        finalPath = d.TraceBack(closest);
    }

    private void FixedUpdate()
    {
        foreach (Path n in g.paths)
        {
            Debug.DrawLine(mg.GetVerticePosition(int.Parse(n.from.name)), mg.GetVerticePosition(int.Parse(n.to.name)));
        }

        for (int i = 0; i < finalPath.Count - 1; i++)
        {
            Debug.DrawLine(mg.GetVerticePosition(int.Parse(finalPath[i].name)), mg.GetVerticePosition(int.Parse(finalPath[i+1].name)), Color.red);
        }
    }

}
