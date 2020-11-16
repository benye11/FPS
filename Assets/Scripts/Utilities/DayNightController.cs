using UnityEngine;
using System.Collections;
using UnityEngine.Events;
 
public class DayNightController : MonoBehaviour {
 
    public Light sun;
    public float TimeInDay; //seconds
    private float currentAngle;

    void Start() {
        currentAngle = 0;
    }
    void Update() {
        currentAngle += 360f * Time.deltaTime / TimeInDay;
        sun.transform.localRotation = Quaternion.Euler(currentAngle, 0, 0);
        if (currentAngle >= 360) {
            currentAngle = 0;
        }
    }

}