using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    //NOTE: this ForceReceiver can only be added in gameObjects that contain Character Controller component
    public float deceleration = 2; //how quickly we will go back to speed 0
    public float mass = 3; //mass of the object
    private Vector3 intensity; //direction of force
    private CharacterController character;
    // Start is called before the first frame update
    void Start()
    {
        intensity = Vector3.zero;
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (intensity.magnitude > 0.2f) {
            character.Move(intensity * Time.deltaTime);
        }
        //Lerp means linear interpolation. get a value and make it smoothly go from one value to another one
        //in this case from intensity to Vector3.zero
        intensity = Vector3.Lerp(intensity, Vector3.zero, deceleration * Time.deltaTime);
    }

    public void AddForce(Vector3 direction, float force) {
        intensity += direction.normalized * force / mass;
    }


}
