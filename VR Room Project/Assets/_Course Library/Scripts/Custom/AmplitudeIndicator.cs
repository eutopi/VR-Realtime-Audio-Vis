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
    private GameObject amplitudeSphereInner;
    private GameObject amplitudeSphereOuter;
    private PhotonView photonView;
    private bool isHovered = false;
    private bool isIncreasing = false;
    private bool isDecreasing = false;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();

        CreateInnerSphere();
        CreateOuterSphere();
    }

    void Update() 
    {
        if (isHovered) 
        {
            if (isIncreasing) 
            {
                photonView.RPC("ChangeAmplitude", RpcTarget.All, 0.05f);
            }
            else if (isDecreasing)
            {   
                if (audioSource.maxDistance > 1.1f) {
                    photonView.RPC("ChangeAmplitude", RpcTarget.All, -0.05f);
                }
            }
            // amplitudeSphereInner.transform.localScale = new Vector3(audioSource.maxDistance, audioSource.maxDistance, audioSource.maxDistance);
            // amplitudeSphereOuter.transform.localScale = new Vector3(audioSource.maxDistance, audioSource.maxDistance, audioSource.maxDistance);
        }
    }

    [PunRPC]
    void ChangeAmplitude(float amount)
    {
        audioSource.maxDistance += amount;
        amplitudeSphereInner.transform.localScale = new Vector3(audioSource.maxDistance, audioSource.maxDistance, audioSource.maxDistance);
        amplitudeSphereOuter.transform.localScale = new Vector3(audioSource.maxDistance, audioSource.maxDistance, audioSource.maxDistance);
    }
    // public void ToggleHovered() {
    //     isHovered = !isHovered;
    // }

    // public void ToggleIncrease() {
    //     isIncreasing = !isIncreasing;
    // }

    // public void ToggleDecrease() {
    //     isDecreasing = !isDecreasing;
    // }

    public void SetHoverTrue() {
        isHovered = true;
    }

    public void SetHoverFalse() {
        isHovered = false;
    }

    public void SetIncreaseTrue() {
        isIncreasing = true;
    }

    public void SetIncreaseFalse() {
        isIncreasing = false;
    }

    public void SetDecreaseTrue() {
        isDecreasing = true;
    }

    public void SetDecreaseFalse() {
        isDecreasing = false;
    }

    void CreateInnerSphere() 
    {
        amplitudeSphereInner = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        amplitudeSphereInner.transform.parent = transform;
        amplitudeSphereInner.transform.position = transform.position;

        SphereCollider sphereCollider = amplitudeSphereInner.GetComponent<SphereCollider>();
        Destroy (sphereCollider);
        amplitudeSphereInner.transform.localScale = new Vector3(audioSource.maxDistance, audioSource.maxDistance, audioSource.maxDistance);
        amplitudeSphereInner.GetComponent<MeshRenderer> ().material = amplitudeSphereMaterial;
    }

    void CreateOuterSphere()
    {
        amplitudeSphereOuter = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        amplitudeSphereOuter.transform.parent = transform;
        amplitudeSphereOuter.transform.position = transform.position;

        SphereCollider sphereCollider = amplitudeSphereOuter.GetComponent<SphereCollider>();
        Destroy (sphereCollider);
        amplitudeSphereOuter.transform.localScale = new Vector3(audioSource.maxDistance, audioSource.maxDistance, audioSource.maxDistance);
        amplitudeSphereOuter.GetComponent<MeshRenderer> ().material = amplitudeSphereMaterial;
        Mesh amplitudeSphereMesh = amplitudeSphereOuter.GetComponent<MeshFilter>().mesh;
        amplitudeSphereMesh.triangles = amplitudeSphereMesh.triangles.Reverse().ToArray();
    }
}
