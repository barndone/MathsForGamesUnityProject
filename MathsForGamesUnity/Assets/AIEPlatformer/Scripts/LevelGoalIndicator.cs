using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoalIndicator : MonoBehaviour
{
    public Transform anchor;
    public float anchorDistance = 1.5f;

    public LevelGoal goal;

    private void LateUpdate()
    {
        Vector3 direction = (goal.transform.position - anchor.position).normalized;
        Debug.DrawRay(anchor.position, direction * 5.0f);


    }
}
