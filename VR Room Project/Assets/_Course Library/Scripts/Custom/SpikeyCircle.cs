using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]

public class SpikeyCircle : MonoBehaviour
{
    // Start is called before the first frame update
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    private int xSize;
    public int zSize = 512;
    private AudioSource audioSource;
    private float[] spectrum = new float[512];
    private float height = 1.0f;
    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        xSize = spectrum.Length - 1;

        CreateShape();
        UpdateMesh();    
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying) 
        {
            AnalyzeAudio();
            UpdateShape();
            UpdateMesh();
        }
    }

    void AnalyzeAudio()
    {
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
    }

    void CreateShape()
    {

        vertices = new Vector3[spectrum.Length + 1];
        vertices[0] = new Vector3(0, height, 0);
        for (int i = 1; i < spectrum.Length + 1; i++) 
        {
            float radius = 3f;
            float x = radius * Mathf.Cos(i*(2*Mathf.PI)/spectrum.Length);
            float y = radius * Mathf.Sin(i*(2*Mathf.PI)/spectrum.Length);
            vertices[i] = new Vector3(x, height, y);
        }
        
        triangles = new int[spectrum.Length * 3];
        for(int i = 0; i < spectrum.Length; i++)
        {
            triangles[i*3] = 0;
            triangles[i*3 + 1] = i+1;
            triangles[i*3 + 2] = (i+1)%spectrum.Length + 1;
        }
        triangles = triangles.Reverse().ToArray();
    }

    void UpdateShape()
    {
        for (int i = 1; i < spectrum.Length + 1; i++) 
        {
            float radius = Mathf.Max(0.3f, spectrum[i-1] * 100f);
            float x = radius * Mathf.Cos(i*(2*Mathf.PI)/spectrum.Length);
            float y = radius * Mathf.Sin(i*(2*Mathf.PI)/spectrum.Length);
            vertices[i] = new Vector3(x, height, y);
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
    
}
