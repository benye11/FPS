using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpawnableItem : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject wrapper;
    public float rotationSpeed = 60f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
