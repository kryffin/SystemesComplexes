using System.Collections.Generic;
using UnityEngine;

public class GraphTest : MonoBehaviour
{

    public Material src, dest, def, path;

    public GameObject[] cubes;

    private void Start()
    {
        Node A = new Node("A");
        Node B = new Node("B");
        Node C = new Node("C");
        Node D = new Node("D");
        Node E = new Node("E");

        Path ab = new Path(A, B, 3f);
        Path ac = new Path(A, C, 1f);
        Path bc = new Path(B, C, 7f);
        Path bd = new Path(B, D, 5f);
        Path be = new Path(B, E, 1f);
        Path cd = new Path(C, D, 2f);
        Path de = new Path(D, E, 7f);

        Graph example1 = new Graph();
        example1.addAllNodes(A, B, C, D, E);
        example1.addAllPaths(ab, ac, bc, bd, be, cd, de);
        
        Debug.Log(example1);

        Dijkstra d = new Dijkstra();
        d.Run(example1, 0);

        Node destination = E;

        foreach (GameObject g in cubes)
        {
            g.GetComponent<MeshRenderer>().material = def;
        }

        cubes[0].GetComponent<MeshRenderer>().material = src;
        cubes[example1.getIndex(destination)].GetComponent<MeshRenderer>().material = dest;
    }

}
