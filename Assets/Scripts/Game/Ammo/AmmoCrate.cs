using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject wrapper;
    public float rotationSpeed = 120f;
    [Header("Gameplay")]
    public int ammo = 12;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
