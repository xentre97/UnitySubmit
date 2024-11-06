using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed = 5f; // 移動速度
    public float verticalSpeed = 3f; // 上昇/下降の速度
    public bool cameraAwake=false;

    public GameObject controller;

    void Update()
    {
        if(cameraAwake)
        {
            // 水平方向と前後の移動
            float moveX = 0f;
            float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime; // WとS、または上と下キー

            // 上下の移動
            float moveY = 0f;
            if (Input.GetKey(KeyCode.Q))
            {
                moveY = verticalSpeed * Time.deltaTime; // Qキーで上昇
            }
            else if (Input.GetKey(KeyCode.E))
            {
                moveY = -verticalSpeed * Time.deltaTime; // Eキーで下降
            }

            // ゲームオブジェクトを移動させる
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
