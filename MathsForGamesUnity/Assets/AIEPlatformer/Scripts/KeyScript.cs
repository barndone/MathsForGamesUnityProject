using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private GameObject parent;
    private PlayerMotor motor;

    private Vector3 offset = Vector3.zero;
    public float orbitSpeed = 180.0f;
    public float orbitRadius = 0.75f;

    public float bounceSpeed = 90.0f;
    public float bounceHeight = 0.5f;

    // Update is called once per frame
    void Update()
    {
        //if the key has a parent object (the player)
        if (parent != null)
        {
            offset.x = Mathf.Cos(Mathf.Deg2Rad * orbitSpeed * Time.time);
            offset.z = Mathf.Sin(Mathf.Deg2Rad * orbitSpeed * Time.time);
            offset *= orbitRadius;
            //spin around the parent (the player)
            transform.position = parent.transform.position + offset;

            offset.y = Mathf.Sin(bounceSpeed * Time.time) * bounceHeight;
        }
        //otherwise do nothing
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parent == null)
        { 
            if (other.TryGetComponent<PlayerMotor> (out PlayerMotor player))
            {
                //expensive to do, just update the transform of the object
                parent = other.gameObject;
                player.hasKey = true;
            }
        }   
        else if (other.TryGetComponent<LockScript>(out LockScript obj))
        {

        }
    }


}
