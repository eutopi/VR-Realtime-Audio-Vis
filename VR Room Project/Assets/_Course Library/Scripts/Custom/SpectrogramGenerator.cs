using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(MeshFilter))]
public class SpectrogramGenerator : MonoBehaviour
{
    Mesh mesh;
    public PlayContinuousSound playContinuousSound;
    public GameObject boomBox;
    public GameObject reticleText;

    Vector3[] vertices;
    Color[] colors;
    int[] triangles;
    public int zSize = 512;
    private int xSize;
    public float[] spectrum = new float[512];
    public Gradient gradient;
    private AudioSource audioSource;
    private TextMesh stats;
    private float minTerrainHeight;
    private float maxTerrainHeight;
    private bool Ydown = false;
    private bool Xdown = false;
    private bool isHovering = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = boomBox.GetComponent<AudioSource>();
        stats = reticleText.GetComponent<TextMesh>();
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;
        xSize = spectrum.Length - 1;

        CreateShape();
        UpdateMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void Update() 
    {
        if (Ydown) {
            audioSource.volume += 0.01f;
        }
        if (Xdown) {
            audioSource.volume -= 0.01f;
        }
        if (audioSource.isPlaying) 
        {
            AnalyzeAudio();
            UpdateShape();
            UpdateMesh();
        }
        ShowStats();
    }

    void AnalyzeAudio()
    {
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
    }

    public void ToggleY() {
        Ydown = !Ydown;
    }

    public void ToggleX() {
        Xdown = !Xdown;
    }

    public void ToggleHover() {
        isHovering = !isHovering;
    }

    void ShowStats() {
        // Debug.Log("uh");
        // Debug.Log(reticleText.transform.position);
        // Debug.Log(inverseIntersectPos);
        // Debug.Log("oh");
        if (isHovering)
        {
            GameObject ret = GameObject.Find("VR_Reticle_Circular(Clone)");
            // reverse rotation
            Quaternion inverseRot = Quaternion.Inverse(transform.rotation);
            Vector3 inverseIntersectPos = RotatePointAroundPivot(ret.transform.position, transform.position, inverseRot);

            int deltaX = (int)Mathf.Round((inverseIntersectPos.x - transform.position.x) * 2);
            float freq = ((float)AudioSettings.outputSampleRate / 2f / spectrum.Length) * deltaX;

            int deltaZ = (int)Mathf.Round((inverseIntersectPos.z - transform.position.z) * 2);
            if (deltaX > 0 && deltaZ > 0)
            {
                float amplitude = vertices[(deltaZ * xSize) + deltaX].y;

                reticleText.transform.position = ret.transform.position;
                reticleText.transform.rotation = ret.transform.rotation;
                stats.text = freq + "Hz, " + amplitude + "dB";
            }
        }
        else 
        {
            stats.text = "";
        }
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angles) {
        return angles * (point - pivot) + pivot;
    }

    void UpdateShape()
    {
        Vector3[] newVertices = new Vector3[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++) {
                float y;
                if (i <= xSize) {
                    y = spectrum[i] * 1000;
                    newVertices[i] = new Vector3(x, y, z);
                }
                else {
                    y = vertices[i - (xSize+1)].y;
                    newVertices[i] = new Vector3(x, y, z);
                }
                if (y > maxTerrainHeight) 
                {
                    maxTerrainHeight = y;
                }
                if (y < minTerrainHeight)
                {
                    minTerrainHeight = y;
                }
                i++;
            }
        }

        Color[] newColors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++) {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, newVertices[i].y);
                newColors[i] = gradient.Evaluate(height);
                i++;
            }
        }

        vertices = newVertices;
        colors = newColors;
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

    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
    }
}
