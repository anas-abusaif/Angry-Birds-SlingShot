using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform IdlePosition;
    public Slingshot SlingShot;
    void Start()
    {

    }

    void Update()
    {
        if (SlingShot.Bird.position.x > IdlePosition.position.x)
        {
            transform.position = new Vector3(SlingShot.Bird.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, IdlePosition.transform.position, 0.05f);
        }
    }
}
