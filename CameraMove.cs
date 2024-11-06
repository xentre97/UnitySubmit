using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed = 5f; // �ړ����x
    public float verticalSpeed = 3f; // �㏸/���~�̑��x
    public bool cameraAwake=false;

    public GameObject controller;

    void Update()
    {
        if(cameraAwake)
        {
            // ���������ƑO��̈ړ�
            float moveX = 0f;
            float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime; // W��S�A�܂��͏�Ɖ��L�[

            // �㉺�̈ړ�
            float moveY = 0f;
            if (Input.GetKey(KeyCode.Q))
            {
                moveY = verticalSpeed * Time.deltaTime; // Q�L�[�ŏ㏸
            }
            else if (Input.GetKey(KeyCode.E))
            {
                moveY = -verticalSpeed * Time.deltaTime; // E�L�[�ŉ��~
            }

            // �Q�[���I�u�W�F�N�g���ړ�������
            transform.Translate(new Vector3(moveX, moveY, moveZ));
        }
    }
    public void ChangeRotation()
    {
        Control control =controller.GetComponent<Control>();
        if (control.P1turn)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (control.P2turn)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
