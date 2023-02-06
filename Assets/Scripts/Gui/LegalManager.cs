using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegalManager : MonoBehaviour
{
    // - フィールド

    GameManager gameManager;

    // - メソッド

    internal bool CanPutToCenterStack(int number)
    {
        return true; // gameManager.
    }

    // - イベントハンドラ

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
