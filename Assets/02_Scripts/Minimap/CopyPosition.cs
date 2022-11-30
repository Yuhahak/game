
using UnityEngine;

public class CopyPosition : MonoBehaviour
{

    [SerializeField]
    private bool       x, y, z; //이 값이 true 면 target의 좌표, false이면 현재 좌표 그대로 사용

    [SerializeField]
    private Transform target; // 쫓아가야 할 대상 Transform
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
