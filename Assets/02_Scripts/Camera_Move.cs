using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Move : MonoBehaviour
{
    //ī�޶� ���� ����� �޴´�
    public Transform target;
    public Vector3 offset;

    void Start()
    {
        
    }

 
    void Update()
    {
        //ī�޶� �̵� �ڵ�
        transform.position = target.position + offset;
    }
}
