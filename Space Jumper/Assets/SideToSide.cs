using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideToSide : MonoBehaviour
{
    public Transform point1;
    public Transform point2;

    public float moveSpeed = 2.0f;
    private float length = 1.0f;

    public GameObject player;

    private void Start()
    {
        
    }
    private void Update()
    {
        float timer = Mathf.PingPong(moveSpeed * Time.time, length);

        transform.position = Vector3.Lerp(point1.position, point2.position, timer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "player")
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            collision.collider.transform.SetParent(null);
        }
    }
}

