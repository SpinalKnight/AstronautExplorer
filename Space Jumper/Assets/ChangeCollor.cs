using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCollor : MonoBehaviour
{
    public Material full;
    public Material medium;
    public Material low;
    private Material material;

    public int fuel;


    public ThirdPersonMovement playerMovement;
    void Start()
    {

    }

    void Update()
    {
        fuel = playerMovement.fuel;
        if (fuel > 350)
        {
            GetComponent<MeshRenderer>().material = full;

        }
        if(fuel > 0 && fuel < 200)
        {
            GetComponent<MeshRenderer>().material = medium;
        }
        if (fuel == 0)
        {
            GetComponent<MeshRenderer>().material = low;
        }

    }
}
