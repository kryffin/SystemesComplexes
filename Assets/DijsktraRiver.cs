using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DijsktraRiver : MonoBehaviour
{

    public int width, height;
    public float magnitude;

    private MeshGenerator mg;

    private Graph g;

    private List<Node> finalPath;

    private Node src;
    private Node dest;

    // Initializes every nodes
    private void InitNodes()
    {
        for (int i = 0; i < width * height; i++)
        {
            g.nodes.Add(new Node(i + ""));
        }
    }

    private float Weight(float y)
    {
        return Mathf.Abs(y - (magnitude/2f)) * 10f;
    }

    // Initializes paths for every node to its neighbors
    private void InitPaths()
    {
        for (int h = height - 1; h >= 0; h--)
        {
            for (int w = 0; w < width; w++)
            {
                // VOISINS DIRECTS
                if (h != 0) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h - 1) + w],
                    Weight(mg.GetVerticePosition(width * (h - 1) + w).y))); //sud

                if (w != 0) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * h + (w - 1)],
                    Weight(mg.GetVerticePosition(width * h + (w - 1)).y))); //est

                if (h != height - 1) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h + 1) + w],
                    Weight(mg.GetVerticePosition(width * (h + 1) + w).y))); //nord

                if (w != width - 1) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * h + (w + 1)],
                    Weight(mg.GetVerticePosition(width * h + (w + 1)).y))); //ouest

                // DIAGONALES
                if (h != 0 && w != width - 1) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h - 1) + (w + 1)],
                    Weight(mg.GetVerticePosition(width * (h - 1) + (w + 1)).y))); //sud est

                if (w != 0 && h != height - 1) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h + 1) + (w - 1)],
                    Weight(mg.GetVerticePosition(width * (h + 1) + (w - 1)).y))); // nord ouest

                if (h != height - 1 && w != width - 1) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h + 1) + (w + 1)],
                    Weight(mg.GetVerticePosition(width * (h + 1) + (w + 1)).y))); //nord est

                if (w != 0 && h != 0) g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h - 1) + (w - 1)],
                    Weight(mg.GetVerticePosition(width * (h - 1) + (w - 1)).y))); //sud ouest

            }
        }
    }

    void Start()
    {
        mg = GetComponent<MeshGenerator>();
        mg.Init(width-1, height-1, magnitude);

        g = new Graph();

        InitNodes();
        InitPaths();

        Dijkstra d = new Dijkstra();
        src = new Node(Random.Range(width * (height - 1), width * height) + "");
        d.Run(g, src);

        dest = new Node(Random.Range(0, width) + "");

        finalPath = d.TraceBack(dest);
    }

    private void FixedUpdate()
    {
        // Drawing each path
        foreach (Path p in g.paths)
        {
            //Debug.DrawLine(mg.GetVerticePosition(int.Parse(p.from.name)), mg.GetVerticePosition(int.Parse(p.to.name)));
        }

        // Drawing the shortest path
        for (int i = 0; i < finalPath.Count - 1; i++)
        {
            Debug.DrawLine(mg.GetVerticePosition(int.Parse(finalPath[i].name)), mg.GetVerticePosition(int.Parse(finalPath[i+1].name)), Color.red);
        }
    }

    private void OnDrawGizmos()
    {
        if (mg == null) return;

        // Drawing the shortest path
        Gizmos.color = Color.red;
        for (int i = 0; i < finalPath.Count - 1; i++)
        {
            Gizmos.DrawSphere(mg.GetVerticePosition(int.Parse(finalPath[i].name)), .1f);
        }
    }

    private void OnGUI()
    {
        foreach (Path p in g.paths)
        {
            Handles.Label((mg.GetVerticePosition(int.Parse(p.from.name)) + mg.GetVerticePosition(int.Parse(p.to.name))) / 2f, p.weight + "");
        }
    }

}
