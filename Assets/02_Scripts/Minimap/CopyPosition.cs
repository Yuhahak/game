
using UnityEngine;

public class CopyPosition : MonoBehaviour
{

    [SerializeField]
    private bool       x, y, z; //�� ���� true �� target�� ��ǥ, false�̸� ���� ��ǥ �״�� ���

    [SerializeField]
    private Transform target; // �Ѿư��� �� ��� Transform
    // Start is called before the first frame update
   
    // Update is called once per frame
    private void Update()
    {
        if (!target) return;

        transform.position = new Vector3(
            (x ? target.position.x : target.position.x),
            100,
            (z ? target.position.z : target.position.z));
    }
}
