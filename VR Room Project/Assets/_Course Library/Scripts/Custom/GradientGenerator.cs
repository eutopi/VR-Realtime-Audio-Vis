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

    bool isSameRow(int index, int newIndex) {
        return index % xSize == newIndex % xSize;
    }

    bool distanceWithinRadius(int index1, int index2, float radius) {
        Vector2 p1 = new Vector2(index1 % (xSize + 1), index1 / (xSize+1));
        Vector2 p2 = new Vector2(index2 % (xSize + 1), index2 / (xSize+1));
        return Vector2.Distance(p1,p2) <= radius;
    }

    bool isViable(int index, int originalIndex, float radius) {
        return index > -1 && index < colors.Length && distanceWithinRadius(originalIndex, index, radius);
    }

    void FillRowColor(int index, int displacement, float maxDistance, int column, int originalIndex) 
    {
        float value = maxDistance - displacement;
        if (isViable(index, originalIndex, maxDistance)) {
            colors[index] = gradient.Evaluate(value);
        }
        for (int k = 1; k < displacement; k++) {
            if (isViable(index+k, originalIndex, maxDistance) && column + k < xSize + 1) {
                colors[index+k] = gradient.Evaluate(value);
            }
            if (isViable(index-k, originalIndex, maxDistance) && column - k > -1) {
                colors[index-k] = gradient.Evaluate(value);
            }
        }
    }

    void FillColumnColor(int index, int displacement, float maxDistance, int column, int originalIndex) 
    {   
        float value = maxDistance - displacement;
        if (isViable(index, originalIndex, maxDistance)) {
            colors[index] = gradient.Evaluate(value);
        }
        for (int k = 1; k < displacement+1; k++) {
            if (isViable(index+k*(xSize+1), originalIndex, maxDistance)) {
                colors[index+k*(xSize+1)] = gradient.Evaluate(value);
            }
            if (isViable(index-k*(xSize+1), originalIndex, maxDistance)) {
                colors[index-k*(xSize+1)] = gradient.Evaluate(value);
            }
        }
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

        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++) {
                colors[i] = gradient.Evaluate(0);
                i++;
            }
        }

        Vector3 origin = transform.position;
        for (int i = 0; i < audioObjects.Length; i++) {
            Vector3 difference = audioObjects[i].transform.position - origin;
            int index = (xSize+1) * (Mathf.RoundToInt(difference.z)+1) + (Mathf.RoundToInt(difference.x)+1);
            AudioSource audioSource = audioObjects[i].GetComponent<AudioSource>();
            int column = Mathf.RoundToInt(difference.x)+1;
            for (int j = 1; j < audioSource.maxDistance+1; j++) {
                FillRowColor(index+j*(xSize+1), j, audioSource.maxDistance, column, index);
                FillRowColor(index-j*(xSize+1), j, audioSource.maxDistance, column, index);
                if (column - j > -1) {
                    FillColumnColor(index-j, j, audioSource.maxDistance, column, index);
                }
                if (column + j < xSize+1) {
                    FillColumnColor(index+j, j, audioSource.maxDistance, column, index);
                }
            }
        }
    }

    void UpdateShape()
    {
        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++) {
                colors[i] = gradient.Evaluate(0);
                i++;
            }
        }

        Vector3 origin = transform.position;
        for (int i = 0; i < audioObjects.Length; i++) {
            Vector3 difference = audioObjects[i].transform.position - origin;
            int index = (xSize+1) * (Mathf.RoundToInt(difference.z)+1) + (Mathf.RoundToInt(difference.x)+1);
            AudioSource audioSource = audioObjects[i].GetComponent<AudioSource>();
            int column = Mathf.RoundToInt(difference.x)+1;
            for (int j = 1; j < audioSource.maxDistance+1; j++) {
                FillRowColor(index+j*(xSize+1), j, audioSource.maxDistance, column, index);
                FillRowColor(index-j*(xSize+1), j, audioSource.maxDistance, column, index);
                if (column - j > -1) {
                    FillColumnColor(index-j, j, audioSource.maxDistance, column, index);
                }
                if (column + j < xSize+1) {
                    FillColumnColor(index+j, j, audioSource.maxDistance, column, index);
                }
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
    }
    
}
