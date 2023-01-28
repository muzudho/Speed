using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 日本と海外で　ルールとプレイング・スタイルに違いがあるので
/// 用語に統一感はない
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 西端
    /// </summary>
    readonly float minX = -62.0f;

    /// <summary>
    /// 東端
    /// </summary>
    readonly float maxX = 62.0f;


    /// <summary>
    /// 底端
    /// 
    /// - `0.0f` は盤
    /// </summary>
    readonly float minY = 0.5f;

    readonly float[] handCardsZ = new[] { -28.0f, 42.0f };

    /// <summary>
    /// 手札
    /// 0: １プレイヤー（黒色）
    /// 1: ２プレイヤー（黒色）
    /// </summary>
    List<List<GameObject>> goPlayersPileCards = new() { new(), new() };

    // 手札
    readonly float[] pileCardsX = new[] { 62.0f, -62.0f };
    readonly float[] pileCardsY = new[] { 0.5f, 0.5f };
    readonly float[] pileCardsZ = new[] { -12.0f, 26.0f };

    /// <summary>
    /// 場札
    /// 0: １プレイヤー（黒色）
    /// 1: ２プレイヤー（黒色）
    /// </summary>
    List<List<GameObject>> goPlayersHandCards = new() { new(), new() };

    /// <summary>
    /// 台札
    /// 0: 右
    /// 1: 左
    /// </summary>
    List<List<GameObject>> goCenterStacksCards = new() { new(), new() };

    // 台札
    float[] centerStacksX = { 15.0f, -15.0f };

    /// <summary>
    /// 台札のY座標
    /// 
    /// - 右が 0、左が 1
    /// - 0.0f は盤なので、それより上にある
    /// </summary>
    float[] centerStacksY = { 0.5f, 0.5f };
    float[] centerStacksZ = { 0.0f, 10.0f };

    // Start is called before the first frame update
    void Start()
    {
        // ゲーム開始時、とりあえず、すべてのカードは、いったん右の台札という扱いにする
        for (int i = 1; i < 14; i++)
        {
            // 右の台札
            goCenterStacksCards[0].Add(GameObject.Find($"Clubs {i}"));
            goCenterStacksCards[0].Add(GameObject.Find($"Diamonds {i}"));
            goCenterStacksCards[0].Add(GameObject.Find($"Hearts {i}"));
            goCenterStacksCards[0].Add(GameObject.Find($"Spades {i}"));
        }

        // 右の台札をシャッフル
        var rightLeft = 0;// 右
        goCenterStacksCards[rightLeft] = goCenterStacksCards[rightLeft].OrderBy(i => Guid.NewGuid()).ToList();

        // 右の台札をすべて、色分けして、黒色なら１プレイヤーの、赤色なら２プレイヤーの、手札に乗せる
        while (0 < goCenterStacksCards[rightLeft].Count)
        {
            AddCardsToPileFromCenterStacks(rightLeft);
        }

        // 手札から５枚抜いて、場札として置く（画面上の場札の位置は調整される）
        for (int player = 0; player < 2; player++)
        {
            AddCardsToHandFromPile(player, 5);
        }

        // ２プレイヤーが、場札の１枚目を抜いて、左の台札へ積み上げる
        PutCardToCenterStack(
            player: 1, // ２プレイヤーが
            handIndex: 0, // 場札の１枚目から
            rightLeft: 0 // 左の
            );
        // ２プレイヤーの場札を並べる
        ArrangeHandCards(1);

        // １プレイヤーが、場札の１枚目を抜いて、右の台札へ積み上げる
        PutCardToCenterStack(
            player: 0, // １プレイヤーが
            handIndex: 0, // 場札の１枚目から
            rightLeft: 1 // 右の
            );
        // １プレイヤーの場札を並べる
        ArrangeHandCards(0);

        StartCoroutine("DoDemo");
    }

    /// <summary>
    /// 台札を、手札へ移動する
    /// </summary>
    /// <param name="rightLeft">右:0, 左:1</param>
    void AddCardsToPileFromCenterStacks(int rightLeft)
    {
        // 台札の一番上（一番後ろ）のカードを１枚抜く
        var numberOfCards = 1;
        var length = goCenterStacksCards[rightLeft].Count; // 手札の枚数
        if (1 <= length)
        {
            var startIndex = length - numberOfCards;
            var goCard = goCenterStacksCards[rightLeft].ElementAt(startIndex);
            goCenterStacksCards[rightLeft].RemoveAt(startIndex);

            // 黒いカードは１プレイヤー、赤いカードは２プレイヤー
            int player;
            float angleY;
            if (goCard.name.StartsWith("Clubs") || goCard.name.StartsWith("Spades"))
            {
                player = 0;
                angleY = 180.0f;
            }
            else if (goCard.name.StartsWith("Diamonds") || goCard.name.StartsWith("Hearts"))
            {
                player = 1;
                angleY = 0.0f;
            }
            else
            {
                throw new Exception();
            }

            // プレイヤーの手札を積み上げる
            goPlayersPileCards[player].Add(goCard);
            SetPosRot(goCard, pileCardsX[player], pileCardsY[player], pileCardsZ[player], angleY: angleY, angleZ: 180.0f);
            pileCardsY[player] += 0.2f;
        }
    }

    /// <summary>
    /// 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
    /// 
    /// - 画面上の場札は位置調整される
    /// </summary>
    void AddCardsToHandFromPile(int player, int numberOfCards)
    {
        // 手札の上の方からｎ枚抜いて、場札へ移動する
        var length = goPlayersPileCards[player].Count; // 手札の枚数
        if (numberOfCards <= length)
        {
            var startIndex = length - numberOfCards;
            var goCards = goPlayersPileCards[player].GetRange(startIndex, numberOfCards);
            goPlayersPileCards[player].RemoveRange(startIndex, numberOfCards);
            goPlayersHandCards[player].AddRange(goCards);

            // 場札を並べる
            ArrangeHandCards(player);
        }
    }

    /// <summary>
    /// 場札を並べる
    /// </summary>
    void ArrangeHandCards(int player)
    {
        float angleY;
        float stepSign;
        float x;

        switch (player)
        {
            case 0:
                // １プレイヤーの場札は、画面では、右から左へ並べる
                angleY = 180.0f;
                stepSign = 1;
                x = minX;
                break;

            case 1:
                // ２プレイヤーの場札は、画面では、左から右へ並べる
                angleY = 0.0f;
                stepSign = -1;
                x = maxX;
                break;

            default:
                throw new Exception();
        }

        float xStep = stepSign * (maxX - minX) / (goPlayersHandCards[player].Count - 1);
        foreach (var goCard in goPlayersHandCards[player])
        {
            SetPosRot(goCard, x, minY, handCardsZ[player], angleY: angleY);
            x += xStep;
        }
    }

    IEnumerator DoDemo()
    {
        float seconds = 1.0f;

        yield return new WaitForSeconds(seconds);

        // １プレイヤーの１枚目のカードにフォーカスを当てる
        GetCard(0, 0, (goCard) => SetFocus(goCard));
        yield return new WaitForSeconds(seconds);

        // １プレイヤーの１枚目のカードのフォーカスを外す
        GetCard(0, 0, (goCard) => ResetFocus(goCard));
        yield return new WaitForSeconds(seconds);

        // １プレイヤーの２枚目のカードにフォーカスを当てる
        GetCard(0, 1, (goCard) => SetFocus(goCard));
        yield return new WaitForSeconds(seconds);

        // １プレイヤーの２枚目のカードのフォーカスを外す
        GetCard(0, 1, (goCard) => ResetFocus(goCard));
        yield return new WaitForSeconds(seconds);

        for (int i = 0; i < 3; i++)
        {
            // 右の台札を積み上げる
            {
                PutCardToCenterStack(
                    player: 0, // １プレイヤーが
                    handIndex: 1, // 場札の２枚目から
                    rightLeft: 1 // 右の台札
                    );
                yield return new WaitForSeconds(seconds);
            }
        }

        // １プレイヤーは手札から３枚抜いて、場札として置く
        AddCardsToHandFromPile(0, 3);
        yield return new WaitForSeconds(seconds);

        // -

        // ２プレイヤーの１枚目のカードにフォーカスを当てる
        GetCard(1, 0, (goCard) => SetFocus(goCard));
        yield return new WaitForSeconds(seconds);

        // ２プレイヤーの１枚目のカードのフォーカスを外す
        GetCard(1, 0, (goCard) => ResetFocus(goCard));
        yield return new WaitForSeconds(seconds);

        // ２プレイヤーの２枚目のカードにフォーカスを当てる
        GetCard(1, 1, (goCard) => SetFocus(goCard));
        yield return new WaitForSeconds(seconds);
    }

    /// <summary>
    /// 場札の好きなところから１枚抜いて、台札を１枚置く
    /// </summary>
    /// <param name="player">何番目のプレイヤー</param>
    /// <param name="handIndex">何枚目のカード</param>
    /// <param name="rightLeft">右なら0、左なら1</param>
    private void PutCardToCenterStack(int player, int handIndex, int rightLeft)
    {
        var goCard = goPlayersHandCards[player].ElementAt(handIndex); // カードを１枚抜いて
        goPlayersHandCards[player].RemoveAt(handIndex);
        goCenterStacksCards[rightLeft].Add(goCard); // 台札として置く

        // カードの位置をセット
        SetPosRot(goCard, this.centerStacksX[rightLeft], this.centerStacksY[rightLeft], this.centerStacksZ[rightLeft]);

        // 次に台札に積むカードの高さ
        this.centerStacksY[rightLeft] += 0.2f;
    }

    /// <summary>
    /// カードを取得
    /// </summary>
    /// <param name="player">何番目のプレイヤー</param>
    /// <param name="cardIndex">何枚目のカード</param>
    /// <param name="setCard">カードをセットする関数</param>
    private void GetCard(int player, int cardIndex, LazyArgs.SetValue<GameObject> setCard)
    {
        if (cardIndex < goPlayersHandCards[player].Count)
        {
            var goCard = goPlayersHandCards[player][cardIndex];
            setCard(goCard);
        }
    }

    private void SetPosRot(GameObject card, float x, float y, float z, float angleY = 180.0f, float angleZ = 0.0f)
    {
        card.transform.position = new Vector3(x, y, z);
        card.transform.rotation = Quaternion.Euler(0, angleY, angleZ);
    }

    /// <summary>
    /// カードを持ち上げる
    /// </summary>
    /// <param name="card"></param>
    private void SetFocus(GameObject card)
    {
        var liftY = 5.0f; // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
        var rotateY = -5; // -5°傾ける
        var rotateZ = -5; // -5°傾ける

        card.transform.position = new Vector3(card.transform.position.x, card.transform.position.y + liftY, card.transform.position.z);
        card.transform.rotation = Quaternion.Euler(card.transform.rotation.eulerAngles.x, card.transform.rotation.eulerAngles.y + rotateY, card.transform.eulerAngles.z + rotateZ);
    }

    /// <summary>
    /// 持ち上げたカードを場に戻す
    /// </summary>
    /// <param name="card"></param>
    private void ResetFocus(GameObject card)
    {
        var liftY = 5.0f; // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
        var rotateY = -5; // -5°傾ける
        var rotateZ = -5; // -5°傾ける

        // 逆をする
        liftY = -liftY;
        rotateY = -rotateY;
        rotateZ = -rotateZ;

        card.transform.position = new Vector3(card.transform.position.x, card.transform.position.y + liftY, card.transform.position.z);
        card.transform.rotation = Quaternion.Euler(card.transform.rotation.eulerAngles.x, card.transform.rotation.eulerAngles.y + rotateY, card.transform.eulerAngles.z + rotateZ);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
