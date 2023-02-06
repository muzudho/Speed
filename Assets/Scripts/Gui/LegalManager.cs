using Assets.Scripts.ThinkingEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegalManager : MonoBehaviour
{
    // - フィールド

    GameModel gameModel;

    // - メソッド

    internal bool CanPutToCenterStack(int player, int place)
    {
        int index = gameModel.GetIndexOfFocusedCardOfPlayer(player);
        // Debug.Log($"[LegalManager CanPutToCenterStack] index:{index}");
        if (index == -1)
        {
            return false;
        }

        IdOfPlayingCards topCard = gameModel.GetLastCardOfCenterStack(place);
        if (topCard == IdOfPlayingCards.None)
        {
            return false;
        }

        var numberOfPickup = gameModel.GetCardsOfPlayerHand(player)[index].Number();
        int numberOfTopCard = topCard.Number();

        // とりあえず差分を取る。
        // 負数が出ると、負数の剰余はプログラムによって結果が異なるので、面倒だ。
        // 割る数を先に足しておけば、剰余をしても負数にはならない
        int divisor = 13; // 法
        int remainder = (numberOfTopCard - numberOfPickup + divisor) % divisor;
        Debug.Log($"[LegalManager CanPutToCenterStack] numberOfPickup:{numberOfPickup} numberOfTopCard:{numberOfTopCard} remainder:{remainder}");
        return remainder == 1 || remainder == divisor - 1;
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
