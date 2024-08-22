using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLook : MonoBehaviour
{

    public Transform headObject, targetObject;
    public bool isInRange;

    void Update()
    {
        if (isInRange)
        {
            Vector3 direction = targetObject.position - headObject.position;

            direction.y = 0;

            Quaternion rotation = Quaternion.LookRotation(direction);
            headObject.rotation = Quaternion.Slerp(headObject.rotation, rotation, Time.deltaTime * 2.0f);


        }
    }

}
