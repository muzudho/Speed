using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // �t�B�[���h

    GameManager gameManager;
    InputManager inputManager;

    // - ���\�b�h

    public void On1pVs2p()
    {

    }

    public void On1pVsCom()
    {

    }

    public void OnComVs2p()
    {

    }

    public void OnComVsCom()
    {

    }

    // - �C�x���g�n���h��

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
