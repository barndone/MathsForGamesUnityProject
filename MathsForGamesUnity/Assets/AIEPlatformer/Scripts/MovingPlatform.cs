using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] [Range(1, 50)] float distanceToMove = 3;
    bool isMoving = true;
    [SerializeField] [Range(0, 1)] float moveSpeed = 1f;

    //convert to either kind of movement rather than just vertical
    //use vector3 instead of float?
    Vector3 startPos;
    Vector3 endPos;

    [SerializeField] Vector3 movementVector = new Vector3(0, 1, 0);


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = transform.position + (movementVector * distanceToMove);
    }

    // Update is called once per frame
    void Update()
    {
        int mod = (isMoving) ? 1 : -1;
        transform.position += (movementVector * distanceToMove) * moveSpeed * mod * Time.deltaTime;


        if (isMoving)
        {
            if (transform.position.y >= endPos.y &&
               transform.position.x >= endPos.x &&
               transform.position.z >= endPos.z)
            {
                transform.position = endPos;             //ensures we actually end at our end position
                isMoving = false;
            }
        }
        else
        {
            if (transform.position.y <= startPos.y &&
                transform.position.x <= startPos.x &&
                transform.position.z <= startPos.z)
            {
                transform.position = startPos;           //ensures we actually end at our start position
                isMoving = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position + (movementVector * distanceToMove), new Vector3(transform.lossyScale.x, transform.lossyScale.y, 0.1f));
        Gizmos.DrawLine(transform.position, transform.position + (movementVector * distanceToMove));
    }
}
