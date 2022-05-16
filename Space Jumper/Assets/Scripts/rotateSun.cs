using UnityEngine;

public class rotateSun : MonoBehaviour
{
    public float rotateSpeed;
    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}