
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIMinimap : MonoBehaviour
{

    [SerializeField]
    private Camera minimapCamera;
    [SerializeField]
    private float zoomMin = 1; //ī�޶��� orthographicSize �ּ� ũ��
    [SerializeField]
    private float zoomMax = 30; //ī�޶��� orthographicSize �ִ� ũ��
    [SerializeField]
    private float zoomOneStep = 1; //1ȸ �� �� �� ����/���ҵǴ� ��ġ
    [SerializeField]
    private TextMeshProUGUI textMapName;

    public void ZoomIn()
    {
        //ī�޶��� orthographicSize ���� ���ҽ��� ī�޶� ���̴� �繰 ũ�� Ȯ��
        minimapCamera.orthographicSize = Mathf.Max(minimapCamera.orthographicSize - zoomOneStep, zoomMin);
    }

    public void ZoomOut()
    {
        minimapCamera.orthographicSize = Mathf.Min(minimapCamera.orthographicSize + zoomOneStep, zoomMax);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
