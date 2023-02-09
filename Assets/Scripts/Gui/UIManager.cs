using Assets.Scripts.ThinkingEngine;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // �t�B�[���h

    GameManager gameManager;
    InputManager inputManager;

    // - ���\�b�h

    public void On1pVs2p()
    {
        inputManager.Computers = new Computer[] { null, null };
    }

    public void On1pVsCom()
    {
        inputManager.Computers = new Computer[] { null, new Computer(1) };
    }

    public void OnComVs2p()
    {
        inputManager.Computers = new Computer[] { new Computer(0), null };
    }

    public void OnComVsCom()
    {
        inputManager.Computers = new Computer[] { new Computer(0), new Computer(1) };
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
