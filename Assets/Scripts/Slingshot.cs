using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public LineRenderer Strip0;
    public LineRenderer Strip1;
    public Transform Center;
    public Transform IdlePosition;
    public Camera Camera;
    private bool MouseDown;
    private bool CanShoot;
    public GameObject BirdPrefab;
    public Rigidbody2D Bird;
    public float BirdOffset;
    public float Force;
    public GameObject PointPrefab;
    GameObject[] Points;
    public int NumOfPoints;
    public float SpaceBetweenPoints;
    Vector2 Direction;
    void Start()
    {
        InstantiateBird();
        Points = new GameObject[NumOfPoints];
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i] = Instantiate(PointPrefab, Center.position, Quaternion.identity);
        }
    }
    void Update()
    {
        Direction = Bird.transform.position - Center.position;

        if (MouseDown)
        {
            SetStrips();
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].transform.position = PointPosition(-i * SpaceBetweenPoints);
                Points[i].SetActive(true);
            }

        }
        else if (!MouseDown)
        {
            ResetStripes();
            if (CanShoot)
            {
                Shoot();
                CanShoot = false;
            }
        }
    }
    void ResetStripes()
    {
        Strip0.SetPosition(1, Vector2.MoveTowards(Strip0.GetPosition(1), IdlePosition.position, 0.1f));
        Strip1.SetPosition(1, Vector2.MoveTowards(Strip1.GetPosition(1), IdlePosition.position, 0.1f));
    }
    void SetStrips()
    {
        Vector2 MousePosition = Camera.ScreenToWorldPoint(Input.mousePosition);
        MousePosition = new Vector2(Mathf.Clamp(MousePosition.x, -29.18f, -21.18f), Mathf.Clamp(MousePosition.y, -8.05f, -0.05f));
        Strip0.SetPosition(1, MousePosition);
        Strip1.SetPosition(1, MousePosition);

        Vector2 dir = Strip0.GetPosition(1) - Center.position;
        Bird.position = (Vector2)Strip0.GetPosition(1) + dir.normalized * BirdOffset;
        Bird.transform.right = -dir;
        CanShoot = true;
    }
    void Shoot()
    {
        Bird.isKinematic = false;
        Bird.GetComponent<Collider2D>().enabled = true;
        Bird.AddForce(-Direction * Force, ForceMode2D.Impulse);
        Bird.GetComponent<Bird>().enabled = true;
        Bird.GetComponent<Bird>().Slingshot = gameObject.GetComponent<Slingshot>();
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].SetActive(false);
        }
    }
    public void InstantiateBird()
    {
        Vector2 dir = Strip0.GetPosition(1) - Center.position;
        Bird = Instantiate(BirdPrefab, (Vector2)Strip0.GetPosition(1) + dir.normalized * BirdOffset, Quaternion.identity).GetComponent<Rigidbody2D>();
    }
    private void OnMouseDown()
    {
        MouseDown = true;
    }
    private void OnMouseUp()
    {
        MouseDown = false;
    }
    Vector2 PointPosition(float t)
    {
        Vector2 Position = (Vector2)Center.position + ((Force - 2f) * t * Direction) + 0.5f * t * t * Physics2D.gravity;
        return Position;
    }
}
