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
    public int xSize = 60;
    public int zSize = 12;
    private float height = 1.0f;
    public GameObject[] audioObjects;
    Color[] colors;
    public Gradient gradient;
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

        Debug.Log(vertices.Length);

        Vector3 origin = transform.position;
        for (int i = 0; i < audioObjects.Length; i++) {
            Vector3 difference = audioObjects[i].transform.position - origin;
            int index = (xSize+1) * (Mathf.RoundToInt(difference.z)+1) + (Mathf.RoundToInt(difference.x)+1);
            vertices[index].y = 5;
            AudioSource audioSource = audioObjects[i].GetComponent<AudioSource>();
            for (int j = 0; j < audioSource.maxDistance; j++) {

            }
        }


        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++) {
                colors[i] = gradient.Evaluate(vertices[i].y);
                i++;
            }
        }
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
        mesh.colors = colors;
    }
    
}
