using Assets.Scripts.ThinkingEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegalManager : MonoBehaviour
{
    // - �t�B�[���h

    GameModel gameModel;

    // - ���\�b�h

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

        // �Ƃ肠�������������B
        // �������o��ƁA�����̏�]�̓v���O�����ɂ���Č��ʂ��قȂ�̂ŁA�ʓ|���B
        // ���鐔���ɑ����Ă����΁A��]�����Ă������ɂ͂Ȃ�Ȃ�
        int divisor = 13; // �@
        int remainder = (numberOfTopCard - numberOfPickup + divisor) % divisor;
        Debug.Log($"[LegalManager CanPutToCenterStack] numberOfPickup:{numberOfPickup} numberOfTopCard:{numberOfTopCard} remainder:{remainder}");
        return remainder == 1 || remainder == divisor - 1;
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
