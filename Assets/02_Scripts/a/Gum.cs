using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gum : MonoBehaviour
{
    public static Gum instance;

    private void Awake()
    {
        instance = this;
    }

    public float _Damage;
    public float GD;


    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            GD = _Damage;
        }
        else
        {
            GD = 0 ;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GD = 0;
    }

}
