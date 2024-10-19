using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour
{
    // Singleton
    public static PositionController instance;

    private void Awake()
    {
        instance = this;
    }

    public float GetYPosition()
    {
        return transform.position.y;
    }

    public float GetXPosition()
    {
        return transform.position.x;
    }

    public float GetZPosition()
    {
        return transform.position.z;
    }
}
