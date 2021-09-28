using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    // Mesh
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    public Color[] colors;

    // Mesh size
    private int xSize;
    private int zSize;

    private float waterLevel;

    public Gradient gradient; //elevation gradient

    public GameObject nodeObject; //node prefab
    private GameObject[] nodeObjects; //nodes

    // Materials
    public Material pathMaterial;
    public Material defaultMaterial;
    public Material selectedMaterial;
    public Material sourceMaterial;
    public Material destinationMaterial;
    public Material obstacleMaterial;

    // Returns a vertice's position
    public Vector3 GetVerticePosition(int index)
    {
        return vertices[index];
    }

    // Initializes the mesh
    public void Init(int width, int height, float magnitude, float waterLevel)
    {
        this.waterLevel = waterLevel;

        xSize = width;
        zSize = height;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape(magnitude);
        UpdateMesh();
    }

    // Creates the random mesh's shape
    void CreateShape(float magnitude)
    {
        nodeObjects = new GameObject[(xSize + 1) * (zSize + 1)];
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        // VERTEX & NODE POSITIONS

        float variation = Random.Range(.1f, .3f); //variation for the Perlin noise
        float offset = -0.06f; //offsetting the node's elevation to avoid sticking out to much

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * variation, z * variation) * magnitude; //returns the y position of each node

                nodeObjects[i] = Instantiate(nodeObject, new Vector3(x, y + offset, z), Quaternion.identity, this.transform); //creating the nodes
                nodeObjects[i].name = i + "";

                if (y <= waterLevel)
                {
                    nodeObjects[i].GetComponent<MeshRenderer>().enabled = false;
                    nodeObjects[i].GetComponent<SphereCollider>().enabled = false;
                }

                vertices[i] = new Vector3(x, y, z);

                i++;
            }
        }

        // TRIANGLES

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        // COLORS

        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(0f, magnitude, vertices[i].y);
                colors[i] = gradient.Evaluate(height); //evaluates the elevation of the node, giving its color
                i++;
            }
        }

    }

    // Changes the path's nodes' material
    public void DrawPath(List<Node> path, Graph g)
    {
        for (int i = 0; i < nodeObjects.Length; i++)
        {
            if (GetVerticePosition(i).y <= waterLevel) continue;

            if (path.Contains(new Node(i))) nodeObjects[i].GetComponent<MeshRenderer>().material = pathMaterial;
            else if (g.nodes[i].obstacle) nodeObjects[i].GetComponent<MeshRenderer>().material = obstacleMaterial;
            else nodeObjects[i].GetComponent<MeshRenderer>().material = defaultMaterial;
        }

        nodeObjects[path[0].value].GetComponent<MeshRenderer>().material = destinationMaterial; //ending node
        nodeObjects[path[path.Count-1].value].GetComponent<MeshRenderer>().material = sourceMaterial; //starting node
    }

    // Changes the hovered node's material
    public void HoverNode(int n)
    {
        nodeObjects[n].GetComponent<MeshRenderer>().material = selectedMaterial;
    }

    // Updates the whole mesh
    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    // Updates the mesh's colors
    public void UpdateColors()
    {
        mesh.colors = colors;
    }

    /*
    private void OnDrawGizmos()
    {
        if (nodeColliders == null) return;

        //Printing nodes' name on screen
        foreach (GameObject go in nodeColliders)
        {
            Handles.Label(go.transform.position, go.name);
        }
    }
    */

}
