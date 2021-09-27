using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DijsktraRiver : MonoBehaviour
{

    public int width, height; //graph size

    public float magnitude; //magnitude for the mesh generation

    public float waterLevel;

    // Controls
    public InputAction leftClick;
    public InputAction rightClick;
    public InputAction mousePosition;

    // Components
    private MeshGenerator mg;
    private LineRenderer lr;

    private Dijkstra d; //Dijkstra algorithm
    private AStar a; //A* algorithm
    private Graph g; //generated graph

    private List<Node> finalPath; //shortest path
    private Node start;
    private Node end;

    // Initializes every nodes
    private void InitNodes()
    {
        for (int i = 0; i < width * height; i++)
        {
            g.nodes.Add(new Node(i)); //name of the node is its position (width * y + x)
        }
    }

    // Computes the weight of a path relative to the elevation of the destination node
    private float Weight(float y)
    {
        return Mathf.Abs(y - (magnitude/2f)) * 10f;
    }

    // Initializes paths for every node to its neighbors (ugly function)
    private void InitPaths()
    {
        for (int h = height - 1; h >= 0; h--)
        {
            for (int w = 0; w < width; w++)
            {
                if (mg.GetVerticePosition(width * h + w).y <= waterLevel) continue;

                // Direct neighbors
                //south
                if (h != 0 && mg.GetVerticePosition(width * (h - 1) + w).y > waterLevel)
                    g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h - 1) + w],
                    Weight(mg.GetVerticePosition(width * (h - 1) + w).y)));

                //east
                if (w != 0 && mg.GetVerticePosition(width * h + (w - 1)).y > waterLevel)
                    g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * h + (w - 1)],
                    Weight(mg.GetVerticePosition(width * h + (w - 1)).y)));

                //north
                if (h != height - 1 && mg.GetVerticePosition(width * (h + 1) + w).y > waterLevel)
                    g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h + 1) + w],
                    Weight(mg.GetVerticePosition(width * (h + 1) + w).y)));

                //west
                if (w != width - 1 && mg.GetVerticePosition(width * h + (w + 1)).y > waterLevel)
                    g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * h + (w + 1)],
                    Weight(mg.GetVerticePosition(width * h + (w + 1)).y)));

                // Diagonal neighbors
                //south east
                if (h != 0 && w != width - 1 && mg.GetVerticePosition(width * (h - 1) + (w + 1)).y > waterLevel)
                    g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h - 1) + (w + 1)],
                    Weight(mg.GetVerticePosition(width * (h - 1) + (w + 1)).y)));

                //north west
                if (w != 0 && h != height - 1 && mg.GetVerticePosition(width * (h + 1) + (w - 1)).y > waterLevel)
                    g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h + 1) + (w - 1)],
                    Weight(mg.GetVerticePosition(width * (h + 1) + (w - 1)).y)));

                //north east
                if (h != height - 1 && w != width - 1 && mg.GetVerticePosition(width * (h + 1) + (w + 1)).y > waterLevel)
                    g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h + 1) + (w + 1)],
                    Weight(mg.GetVerticePosition(width * (h + 1) + (w + 1)).y)));

                //south west
                if (w != 0 && h != 0 && mg.GetVerticePosition(width * (h - 1) + (w - 1)).y > waterLevel)
                    g.paths.Add(new Path(g.nodes[width * h + w], g.nodes[width * (h - 1) + (w - 1)],
                    Weight(mg.GetVerticePosition(width * (h - 1) + (w - 1)).y)));

            }
        }
    }

    // Updates the shortest path
    private void UpdatePaths()
    {
        // Dijkstra
        //d.Run(g, start);
        //finalPath = d.TraceBack(end);

        // A*
        a.Run(g, start, end, mg);
        finalPath = a.TraceBack(end);

        TracePath();
    }

    // Draws the shortest path using a LineRenderer
    private void TracePath()
    {
        List<Vector3> nodes = new List<Vector3>();

        for (int i = 0; i < finalPath.Count; i++)
        {
            if (i == 0 || i == finalPath.Count - 1) nodes.Add(mg.GetVerticePosition(finalPath[i].value));
            else nodes.Add(mg.GetVerticePosition(finalPath[i].value) + Vector3.up * 0.08f);
        }

        lr.positionCount = nodes.Count;
        lr.SetPositions(nodes.ToArray());
    }

    void Start()
    {
        lr = GetComponent<LineRenderer>();

        mg = GetComponent<MeshGenerator>();
        mg.Init(width-1, height-1, magnitude, waterLevel);

        //initialization of the graph
        g = new Graph();
        InitNodes();
        InitPaths();

        start = new Node(Random.Range(0, Mathf.FloorToInt((width * height) / 2f))); //random source node
        end = new Node(Random.Range(Mathf.FloorToInt((width * height) / 2f), width * height)); //random end node

        // Dijkstra
        //d = new Dijkstra();
        //d.Run(g, start);

        // A*
        a = new AStar();
        a.Run(g, start, end, mg);

        // Shortest path
        //finalPath = d.TraceBack(end); //Dijkstra shortest path
        finalPath = a.TraceBack(end); //A* shortest path

        TracePath();
    }

    private void Update()
    {
        // Casts a ray to interact with the first hit object in scene, triggers on click
        if (leftClick.triggered)
        {
            // Left click is used to select the source node
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePosition.ReadValue<Vector2>().x, mousePosition.ReadValue<Vector2>().y, .0f));
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform != null)
                {
                    start = new Node(int.Parse(hit.transform.name));

                    UpdatePaths();
                    mg.DrawPath(finalPath);
                }
            }
        }
        else if (rightClick.triggered)
        {
            // Right click is used to select the end node
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePosition.ReadValue<Vector2>().x, mousePosition.ReadValue<Vector2>().y, .0f));
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform != null)
                {
                    end = new Node(int.Parse(hit.transform.name));

                    UpdatePaths();
                    mg.DrawPath(finalPath);
                }
            }
        }
        else
        {
            // No click triggered, checking for the mouse hovering a node
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePosition.ReadValue<Vector2>().x, mousePosition.ReadValue<Vector2>().y, .0f));
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform != null)
                {
                    mg.HoverNode(int.Parse(hit.transform.name));
                }
            }
            else // Nothing hovered, redrawing the whole nodes
            {
                mg.DrawPath(finalPath);
            }
        }
    }

    // Enables the controls
    private void OnEnable()
    {
        leftClick.Enable();
        rightClick.Enable();
        mousePosition.Enable();
    }

    // Disables the controls
    private void OnDisable()
    {
        leftClick.Disable();
        rightClick.Disable();
        mousePosition.Disable();
    }

    
    private void FixedUpdate()
    {
        // Drawing each path
        foreach (Path p in g.paths)
        {
            Debug.DrawLine(mg.GetVerticePosition(p.from.value), mg.GetVerticePosition(p.to.value));
        }

        // Drawing the shortest path
        for (int i = 0; i < finalPath.Count - 1; i++)
        {
            Debug.DrawLine(mg.GetVerticePosition(finalPath[i].value), mg.GetVerticePosition(finalPath[i+1].value), Color.red);
        }
    }
    

}
