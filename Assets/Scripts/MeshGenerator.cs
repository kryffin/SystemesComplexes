using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    Mesh mesh;

    GameObject[] nodeColliders;
    Vector3[] vertices;
    int[] triangles;
    public Color[] colors;

    private int xSize;
    private int zSize;

    public Gradient gradient;

    public GameObject nodeCollider;

    public Material pathMaterial;
    public Material defaultMaterial;
    public Material selectedMaterial;
    public Material sourceMaterial;
    public Material destinationMaterial;

    public Vector3 GetVerticePosition(int index)
    {
        return vertices[index];
    }

    public void Init(int width, int height, float magnitude)
    {
        xSize = width;
        zSize = height;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape(magnitude);
        UpdateMesh();
    }

    void CreateShape(float magnitude)
    {
        nodeColliders = new GameObject[(xSize + 1) * (zSize + 1)];
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        float variation = Random.Range(.1f, .3f);

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * variation, z * variation) * magnitude;
                nodeColliders[i] = Instantiate(nodeCollider, new Vector3(x, y, z), Quaternion.identity, this.transform);
                nodeColliders[i].name = i + "";
                vertices[i] = new Vector3(x, y, z);

                i++;
            }
        }

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

        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(0f, magnitude, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }

    }

    public void DrawPath(List<Node> path)
    {
        for (int i = 0; i < nodeColliders.Length; i++)
        {
            if (path.Contains(new Node(i + ""))) nodeColliders[i].GetComponent<MeshRenderer>().material = pathMaterial;
            else nodeColliders[i].GetComponent<MeshRenderer>().material = defaultMaterial;
        }

        nodeColliders[int.Parse(path[0].name + "")].GetComponent<MeshRenderer>().material = destinationMaterial;
        nodeColliders[int.Parse(path[path.Count-1].name + "")].GetComponent<MeshRenderer>().material = sourceMaterial;
    }

    public void HoverNode(int n)
    {
        nodeColliders[n].GetComponent<MeshRenderer>().material = selectedMaterial;
    }

    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

}
