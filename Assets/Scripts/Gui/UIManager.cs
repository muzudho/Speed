using Assets.Scripts.ThinkingEngine;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // フィールド

    [SerializeField] GameObject playerSelectBackground;
    [SerializeField] GameObject playerButtons;
    [SerializeField] GameObject p1Keys;
    [SerializeField] GameObject p2Keys;

    InputManager inputManager;

    // - メソッド

    public void On1pVs2p()
    {
        inputManager.Computers = new Computer[] { null, null };
        playerSelectBackground.SetActive(false);
        playerButtons.SetActive(false);
        p1Keys.SetActive(true);
        p2Keys.SetActive(true);
    }

    public void On1pVsCom()
    {
        inputManager.Computers = new Computer[] { null, new Computer(1) };
        playerSelectBackground.SetActive(false);
        playerButtons.SetActive(false);
        p1Keys.SetActive(true);
    }

    public void OnComVs2p()
    {
        inputManager.Computers = new Computer[] { new Computer(0), null };
        playerSelectBackground.SetActive(false);
        playerButtons.SetActive(false);
        p2Keys.SetActive(true);
    }

    public void OnComVsCom()
    {
        inputManager.Computers = new Computer[] { new Computer(0), new Computer(1) };
        playerSelectBackground.SetActive(false);
        playerButtons.SetActive(false);
    }

    // - イベントハンドラ

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();

        p1Keys.SetActive(false);
        p2Keys.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
