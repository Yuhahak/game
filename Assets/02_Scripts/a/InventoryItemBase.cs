using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemBase : MonoBehaviour, IInventoryItem //�������� �⺻�� �Ǵ� ��ũ��Ʈ
                                                               //�����۸��� �������� MonoBehaviour��� InventoryItemBase���� ���� ��
{


    public virtual string Name
    {
        get
        {
            return "_base item_";
        }
    }

    public Sprite _Image;

    public Sprite Image
    {
        get { return _Image; }
    }


    public virtual void OnPickup()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnUse()
    {
        transform.localPosition = PickPosition;
        transform.localEulerAngles = PickRotation;
    }

    public virtual void OnUseup()
    {
        gameObject.SetActive(false);
    }




    public virtual void Ondrop()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            gameObject.SetActive(false);
            Destroy(this);
        }
    }

    public Vector3 PickPosition; //���⸦ ������� ��ġ�� ȸ������ ����
    public Vector3 PickRotation;
    public Vector3 DropRotation;

    
}
