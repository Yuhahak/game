using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Move : MonoBehaviour
{
    //카메라가 따라갈 대상을 받는다
    public Transform target;
    public Vector3 offset;

    void Start()
    {
        
    }

 
    void Update()
    {
        //카메라 이동 코드
        transform.position = target.position + offset;
    }
}
