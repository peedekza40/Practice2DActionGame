using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ParallaxController : MonoBehaviour
{
    private Transform CameraTransform;
    private Vector3 CameraStartPosition;
    private float Distance;

    private GameObject[] Backgrounds;
    private Material[] Materials;
    private float[] BackSpeed;

    private float FarthestBack;

    [Range(0.01f, 0.05f)]
    public float ParallaxSpeed = 0.01f;

    private void Start()
    {
        CameraTransform = Camera.main.transform;
        CameraStartPosition = CameraTransform.position;

        var backCount = transform.childCount;
        Materials = new Material[backCount];
        BackSpeed = new float[backCount];
        Backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            Backgrounds[i] = transform.GetChild(i).gameObject;
            Materials[i] = Backgrounds[i].GetComponent<Renderer>().material;
        }

        BackSpeedCalculate(backCount);
    }

    private void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++) // find the farthest background
        {
            if((Backgrounds[i].transform.position.z - CameraTransform.position.z) > FarthestBack)
            {
                FarthestBack = Backgrounds[i].transform.position.z - CameraTransform.position.z;
            }
        }

        for (int i = 0; i < backCount; i++) // set the speed of background
        {
            BackSpeed[i] = 1 - (Backgrounds[i].transform.position.z - CameraTransform.position.z) / FarthestBack;
        }
    }

    private void LateUpdate() 
    {
        Distance = CameraTransform.position.x - CameraStartPosition.x;
        transform.position = new Vector3(CameraTransform.position.x, transform.position.y, 0);

        for (int i = 0; i < Backgrounds.Length; i++)
        {
            var speed = BackSpeed[i] * ParallaxSpeed;
            Materials[i].SetTextureOffset("_MainTex", new Vector2(Distance, 0) * speed);
        }  
    }
}