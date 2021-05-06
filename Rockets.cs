using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLyingEnemyMovemetn : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Transform target;
    
    [Header("Rocket Settings")]
    public float RocketMass;
    public float maxVelocity;
    public float distance;
    public float Drag;

    [Header("Advanced settings")]
    public bool UseGravity = false;
    public LayerMask layerMask;

    // Start is called before the first frame update
    
    void Start()
    {
        // if there are no Rigidbody attached to the rocket, this script will add one
        if (rb == null)
        {
            gameObject.AddComponent<Rigidbody>();
        }

        // references the Rigidbody script
        rb = gameObject.GetComponent<Rigidbody>();

        // Some settings for the Rocket
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.mass = RocketMass;
        rb.drag = Drag;
        rb.useGravity = UseGravity;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        // add a constant force upward to stabilise the rocket
        rb.AddForce(0, 9.81f, 0);
 
        // Raycast to detect whether the rocket has reached the target
        // if the target has been reached, then the Rigidbody will be set to kinematic
        RaycastHit Hit;
        if(Physics.Raycast(transform.position, transform.forward, out Hit, distance, layerMask)) 
        {
            // Add your eplosion and other effects here
            rb.velocity = new Vector3(0, 0, 0);
        }


        // if the target has not been reached the Rigidbody is not kinematic and so it can be affected by physics
        
        else
        {
            rb.isKinematic = false;
            transform.LookAt(target); // Turns forward vector towards player
            rb.AddRelativeForce(Vector3.forward * Time.deltaTime * maxVelocity, ForceMode.Impulse); // Adds force to the rocket forwards
        }
    }


    // some gizmo stuff
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 0, 1f);
        Vector3 direction = transform.forward * distance;
        Gizmos.DrawRay(transform.position, direction);
    }
}
