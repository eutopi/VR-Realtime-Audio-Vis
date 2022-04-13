using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using System.Linq;

public class AmplitudeIndicator : MonoBehaviour
{
    public Material amplitudeSphereMaterial;
    Mesh mesh;
    private AudioSource audioSource;    
    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();

        createInnerSphere();
        createOuterSphere();
    }

    void createInnerSphere() 
    {
        GameObject amplitudeSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        amplitudeSphere.transform.parent = transform;
        amplitudeSphere.transform.position = transform.position;

        SphereCollider sphereCollider = amplitudeSphere.GetComponent<SphereCollider>();
        Destroy (sphereCollider);
        amplitudeSphere.transform.localScale = new Vector3(audioSource.maxDistance, audioSource.maxDistance, audioSource.maxDistance);
        amplitudeSphere.GetComponent<MeshRenderer> ().material = amplitudeSphereMaterial;
    }

    void createOuterSphere()
    {
        GameObject amplitudeSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        amplitudeSphere.transform.parent = transform;
        amplitudeSphere.transform.position = transform.position;

        SphereCollider sphereCollider = amplitudeSphere.GetComponent<SphereCollider>();
        Destroy (sphereCollider);
        amplitudeSphere.transform.localScale = new Vector3(audioSource.maxDistance, audioSource.maxDistance, audioSource.maxDistance);
        amplitudeSphere.GetComponent<MeshRenderer> ().material = amplitudeSphereMaterial;
        Mesh amplitudeSphereMesh = amplitudeSphere.GetComponent<MeshFilter>().mesh;
        amplitudeSphereMesh.triangles = amplitudeSphereMesh.triangles.Reverse().ToArray();
    }

    void Update() 
    {
        
    }
}
