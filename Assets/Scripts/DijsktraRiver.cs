using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DijsktraRiver : MonoBehaviour
{

    public int width, height;
    public float magnitude;

    private MeshGenerator mg;

    private Graph g;

    private List<Node> finalPath;

    private Node src;
    private Node dest;

    private Dijkstra d;

    public InputAction leftClick;
    public InputAction rightClick;
    public InputAction mousePosition;

    private LineRenderer lr;

    private void OnEnable()
    {
        leftClick.Enable();
        rightClick.Enable();
        mousePosition.Enable();
    }

    private void OnDisable()
    {
        leftClick.Disable();
        rightClick.Disable();
        mousePosition.Disable();
    }

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

    private void UpdatePaths()
    {
        d.Run(g, src);
        finalPath = d.TraceBack(dest);

        TracePath();
    }

    private void TracePath()
    {
        List<Vector3> nodes = new List<Vector3>();
        foreach (Node n in finalPath)
        {
            nodes.Add(mg.GetVerticePosition(int.Parse(n.name)) + Vector3.up * 0.1f);
        }

        lr.positionCount = nodes.Count;
        lr.SetPositions(nodes.ToArray());
    }

    void Start()
    {
        lr = GetComponent<LineRenderer>();

        mg = GetComponent<MeshGenerator>();
        mg.Init(width-1, height-1, magnitude);

        g = new Graph();

        InitNodes();
        InitPaths();

        d = new Dijkstra();
        src = new Node(Random.Range(width * (height - 1), width * height) + "");
        d.Run(g, src);

        dest = new Node(Random.Range(0, width) + "");

        finalPath = d.TraceBack(dest);
        TracePath();
    }

    private void Update()
    {
        // Casts a ray to interact with the first hit object in scene, on click
        if (leftClick.triggered)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePosition.ReadValue<Vector2>().x, mousePosition.ReadValue<Vector2>().y, .0f));
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform != null)
                {
                    src = new Node(hit.transform.name);

                    UpdatePaths();
                    mg.DrawPath(finalPath);
                }
            }
        }
        else if (rightClick.triggered)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePosition.ReadValue<Vector2>().x, mousePosition.ReadValue<Vector2>().y, .0f));
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform != null)
                {
                    dest = new Node(hit.transform.name);

                    UpdatePaths();
                    mg.DrawPath(finalPath);
                }
            }
        }
        else
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePosition.ReadValue<Vector2>().x, mousePosition.ReadValue<Vector2>().y, .0f));
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform != null)
                {
                    mg.HoverNode(int.Parse(hit.transform.name));
                }
            }
            else
            {
                mg.DrawPath(finalPath);
            }
        }
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
            //Debug.DrawLine(mg.GetVerticePosition(int.Parse(finalPath[i].name)), mg.GetVerticePosition(int.Parse(finalPath[i+1].name)), Color.red);
        }
    }

    private void OnDrawGizmos()
    {
        if (mg == null) return;
        Gizmos.color = Color.black;
        //if (selectedNode != -1) Gizmos.DrawSphere(mg.GetVerticePosition(selectedNode), .1f);
    }
}
