using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// �}�E�X�{�^��������
    /// </summary>
    private void OnMouseDown()
    {
        // ���Ԃ��܂�
        var oldZ = transform.rotation.eulerAngles.z; // �x���@
        transform.rotation = Quaternion.Euler(0, 0, oldZ + 180); // 180����]
    }
}
