using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private GameObject parent;
    private PlayerMotor motor;

    [SerializeField] private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        //if the key has a parent object (the player)
        if (parent != null)
        {
            //spin around the parent (the player)
            gameObject.transform.position = parent.transform.position + offset;
        }
        //otherwise do nothing
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parent = other.gameObject;
            parent.TryGetComponent<PlayerMotor>(out motor);
            motor.hasKey = true;
        }
        if (other.CompareTag("lock"))
        {

        }
    }


}
