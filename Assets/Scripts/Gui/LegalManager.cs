using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegalManager : MonoBehaviour
{
    // - �t�B�[���h

    GameManager gameManager;

    // - ���\�b�h

    internal bool CanPutToCenterStack(int number)
    {
        return true; // gameManager.
    }

    // - �C�x���g�n���h��

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
