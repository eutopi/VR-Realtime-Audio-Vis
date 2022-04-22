using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]

public class GradientGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public int xSize = 120;
    public int zSize = 24;
    private float height = 1.0f;
    public GameObject[] audioObjects;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();    
    }

    // Update is called once per frame
    void Update()
    {
        UpdateShape();
        UpdateMesh();
    }

    void CreateShape()
    {

        // vertices = new Vector3[spectrum.Length + 1];
        // vertices[0] = new Vector3(0, height, 0);
        // for (int i = 1; i < spectrum.Length + 1; i++) 
        // {
        //     float radius = 3f;
        //     float x = radius * Mathf.Cos(i*(2*Mathf.PI)/spectrum.Length);
        //     float y = radius * Mathf.Sin(i*(2*Mathf.PI)/spectrum.Length);
        //     vertices[i] = new Vector3(x, height, y);
        // }
        
        // triangles = new int[spectrum.Length * 3];
        // for(int i = 0; i < spectrum.Length; i++)
        // {
        //     triangles[i*3] = 0;
        //     triangles[i*3 + 1] = i+1;
        //     triangles[i*3 + 2] = (i+1)%spectrum.Length + 1;
        // }
        // triangles = triangles.Reverse().ToArray();

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++) {
                //float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2f;
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }
        
        int vert = 0;
        int tris = 0;
        triangles = new int[xSize * zSize * 6];
        for (int z = 0; z < zSize; z++) {
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

        // colors = new Color[vertices.Length];
        // for (int i = 0, z = 0; z <= zSize; z++) {
        //     for (int x = 0; x <= xSize; x++) {
        //         colors[i] = gradient.Evaluate(0);
        //         i++;
        //     }
        // }
    }

    void UpdateShape()
    {
        // for (int i = 1; i < spectrum.Length + 1; i++) 
        // {
        //     float radius = Mathf.Max(0.3f, spectrum[i-1] * 100f);
        //     float x = radius * Mathf.Cos(i*(2*Mathf.PI)/spectrum.Length);
        //     float y = radius * Mathf.Sin(i*(2*Mathf.PI)/spectrum.Length);
        //     vertices[i] = new Vector3(x, height, y);
        // }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
    
}
