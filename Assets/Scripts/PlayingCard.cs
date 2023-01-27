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
    /// マウスボタン押下時
    /// </summary>
    private void OnMouseDown()
    {
        // 裏返します
        var oldZ = transform.rotation.eulerAngles.z; // 度数法
        transform.rotation = Quaternion.Euler(0, 0, oldZ + 180); // 180°回転
    }
}
