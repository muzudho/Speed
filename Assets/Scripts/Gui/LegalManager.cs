using Assets.Scripts.ThinkingEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegalManager : MonoBehaviour
{
    // - フィールド

    GameModel gameModel;

    // - メソッド

    internal bool CanPutToCenterStack(int number)
    {
        return true; // gameManager.
    }

    // - イベントハンドラ

    // Start is called before the first frame update
    void Start()
    {
        gameModel = GameObject.Find("Game Manager").GetComponent<GameManager>().Model;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
