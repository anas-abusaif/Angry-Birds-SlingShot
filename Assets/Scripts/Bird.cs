using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    Rigidbody2D rb;
    bool InAir = true;
    public Slingshot Slingshot;
    public float MaxDistance = 30f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (InAir)
        {
            float Angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
        }
        if (rb.velocity.magnitude < 0.4f && rb.isKinematic == false || transform.position.x > MaxDistance)
        {
            Slingshot.Invoke(nameof(Slingshot.InstantiateBird), 1);
            GetComponent<Bird>().enabled = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        InAir = false;

    }
}
