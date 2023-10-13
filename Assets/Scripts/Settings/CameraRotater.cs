using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotater : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float rotationSpeed = -20.0f; 
    // Update is called once per frame
    void Update()
    {
        CameraRotate();
    }

    private void CameraRotate(){
        float rotation = rotationSpeed * Time.deltaTime;

        // Áp dụng góc quay cho camera quanh nhân vật
        transform.RotateAround(target.position, Vector3.up, rotation);
    }
}
