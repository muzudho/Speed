using Assets.Scripts.ThinkingEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegalManager : MonoBehaviour
{
    // - �t�B�[���h

    GameModel gameModel;

    // - ���\�b�h

    internal bool CanPutToCenterStack(int number)
    {
        return true; // gameManager.
    }

    // - �C�x���g�n���h��

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
