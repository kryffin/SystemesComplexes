using System.Collections.Generic;
using UnityEngine;

public class GraphTest : MonoBehaviour
{

    public Material src, dest, def, path;

    public GameObject nodePrefab;
    public GameObject[] cubes;

    private List<Node> finalPath;

    private Graph example1;
    private Dijkstra d;
    private Node destination;

    private const int WIDTH = 3, HEIGHT = 3;

    private void createCubes()
    {
        cubes = new GameObject[example1.nodes.Count];

        int i = 0;
        foreach (Node n in example1.nodes)
        {
            cubes[i++] = Instantiate<GameObject>(nodePrefab);
        }
    }

    private void initPaths()
    {
        for (int w = 0; w < WIDTH; w++)
        {
            for (int h = 0; h < HEIGHT; h++)
            {
                if (h != 0) example1.paths.Add(new Path(example1.nodes[WIDTH * h + w], example1.nodes[WIDTH * (h-1) + w], 1f));
                if (w != 0) example1.paths.Add(new Path(example1.nodes[WIDTH * h + w], example1.nodes[WIDTH * h + (w-1)], 1f));
                if (h != HEIGHT - 1) example1.paths.Add(new Path(example1.nodes[WIDTH * h + w], example1.nodes[WIDTH * (h+1) + w], 1f));
                if (w != WIDTH - 1) example1.paths.Add(new Path(example1.nodes[WIDTH * h + w], example1.nodes[WIDTH * h + (w+1)], 1f));
            }
        }
    }

    private void initCubes()
    {
        cubes = new GameObject[WIDTH * HEIGHT];

        int i = 0;
        for (int w = 0; w < WIDTH; w++)
        {
            for (int h = 0; h < HEIGHT; h++)
            {
                example1.nodes.Add(new Node((WIDTH * h + w) + ""));
                cubes[i] = Instantiate<GameObject>(nodePrefab, new Vector3(w * 2f, 0f, h * 2f), Quaternion.identity);
                cubes[i].name = i + "";

                i++;
            }
        }
    }

    private void updatePaths()
    {
        for (int i = 0; i < example1.paths.Count; i++)
        {
            example1.paths[i].weight = Vector3.Distance(cubes[int.Parse(example1.paths[i].from.name)].transform.position, cubes[int.Parse(example1.paths[i].to.name)].transform.position);
        }
    }

    private void Start()
    {
        example1 = new Graph();
        destination = new Node((WIDTH * HEIGHT - 1) + "");

        initCubes();
        initPaths();

        d = new Dijkstra();
    }

    private void resetCubes()
    {
        foreach (GameObject c in cubes)
        {
            c.GetComponent<MeshRenderer>().material = def;
        }
        cubes[0].GetComponent<MeshRenderer>().material = src;
        cubes[int.Parse(destination.name)].GetComponent<MeshRenderer>().material = dest;
    }

    private void FixedUpdate()
    {
        resetCubes();

        updatePaths();

        d.Run(example1, 0);

        // calcul chemin le plus court

        Node prc = d.previous[int.Parse(destination.name)];
        finalPath = new List<Node>();
        while (prc != null && prc.name != "0")
        {
            finalPath.Add(prc);
            cubes[int.Parse(prc.name)].GetComponent<MeshRenderer>().material = path;
            prc = d.previous[int.Parse(prc.name)];
        }

        // affichage

        foreach (Path p in example1.paths)
        {
            Debug.DrawLine(cubes[int.Parse(p.from.name)].transform.position, cubes[int.Parse(p.to.name)].transform.position, Color.green);
        }

        for (int i = 0; i < finalPath.Count-1; i++)
        {
            Debug.DrawLine(cubes[example1.getIndex(finalPath[i])].transform.position, cubes[example1.getIndex(finalPath[i+1])].transform.position, Color.red);
        }
    }

}
