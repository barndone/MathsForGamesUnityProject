using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockScript : MonoBehaviour
{
    [SerializeField] GameObject lockedDoor;
    private PlayerMotor motor;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerMotor>(out motor))
        {
            Unlock(motor.hasKey);
            motor.hasKey = false;
        }
    }
    public void Unlock(bool hasKey)
    {
        if (hasKey)
        {
            Debug.Log("Door unlocked.");
            Destroy(lockedDoor);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("You do not have the key, the door remains locked.");
        }
    }
}
