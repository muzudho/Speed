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
    /// プレイヤーが選択している場札は、先頭から何枚目
    /// 
    /// - 選択中の場札が無いなら、-1
    /// </summary>
    int[] playsersFocusedCardIndex = { -1, -1 };

    GameViewModel gameViewModel;

    // Start is called before the first frame update
    void Start()
    {
        gameViewModel = new GameViewModel();
        gameViewModel.Init();

        const int right = 0;// 台札の右
        // const int left = 1;// 台札の左

        // 右の台札をすべて、色分けして、黒色なら１プレイヤーの、赤色なら２プレイヤーの、手札に乗せる
        while (0 < gameViewModel.GetLengthOfCenterStackCards(right))
        {
            MoveCardsToPileFromCenterStacks(right);
        }

        // １，２プレイヤーについて、手札から５枚抜いて、場札として置く（画面上の場札の位置は調整される）
        MoveCardsToHandFromPile(player: 0, numberOfCards: 5);
        MoveCardsToHandFromPile(player: 1, numberOfCards: 5);

        StartCoroutine("DoDemo");
    }

    // Update is called once per frame
    void Update()
    {
        const int right = 0;// 台札の右
        const int left = 1;// 台札の左

        // １プレイヤー
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
            MoveCardToCenterStackFromHand(
                player: 0, // １プレイヤーが
                place: left // 左の
                );
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
            MoveCardToCenterStackFromHand(
                player: 0, // １プレイヤーが
                place: right // 右の
                );
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）左隣のカードをピックアップするように変えます
            MoveFocusToNextCard(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）右隣のカードをピックアップするように変えます
            MoveFocusToNextCard(0, 0);
        }

        // ２プレイヤー
        if (Input.GetKeyDown(KeyCode.W))
        {
            // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
            MoveCardToCenterStackFromHand(
                player: 1, // ２プレイヤーが
                place: right // 右の
                );
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
            MoveCardToCenterStackFromHand(
                player: 1, // ２プレイヤーが
                place: left // 左の
                );
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）左隣のカードをピックアップするように変えます
            MoveFocusToNextCard(1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）右隣のカードをピックアップするように変えます
            MoveFocusToNextCard(1, 0);
        }

        // デバッグ用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 両プレイヤーは手札から１枚抜いて、場札として置く
            for (var player = 0; player < 2; player++)
            {
                MoveCardsToHandFromPile(player, 1);
            }
        }
    }

    IEnumerator DoDemo()
    {
        // 卓準備
        const int right = 0;// 台札の右
        const int left = 1;// 台札の左

        float seconds = 1.0f;
        yield return new WaitForSeconds(seconds);

        // １プレイヤーの先頭のカードへフォーカスを移します
        MoveFocusToNextCard(player: 0, direction: 0);
        // ２プレイヤーの先頭のカードへフォーカスを移します
        MoveFocusToNextCard(player: 1, direction: 0);
        yield return new WaitForSeconds(seconds);

        // １プレイヤーが、ピックアップ中の場札を抜いて、右の台札へ積み上げる
        MoveCardToCenterStackFromHand(
            player: 0, // １プレイヤーが
            place: right // 右の
            );
        // ２プレイヤーが、ピックアップ中の場札を抜いて、左の台札へ積み上げる
        MoveCardToCenterStackFromHand(
            player: 1, // ２プレイヤーが
            place: left // 左の
            );
        yield return new WaitForSeconds(seconds);

        // ゲーム開始

        for (int i = 0; i < 2; i++)
        {
            // １プレイヤーの右隣のカードへフォーカスを移します
            MoveFocusToNextCard(0, 0);
            // ２プレイヤーの右隣のカードへフォーカスを移します
            MoveFocusToNextCard(1, 0);
            yield return new WaitForSeconds(seconds);
        }

        // 台札を積み上げる
        {
            MoveCardToCenterStackFromHand(
                player: 0, // １プレイヤーが
                place: 1 // 左の台札
                );
            MoveCardToCenterStackFromHand(
                player: 1, // ２プレイヤーが
                place: 0 // 右の台札
                );
            yield return new WaitForSeconds(seconds);
        }

        // １プレイヤーは手札から３枚抜いて、場札として置く
        MoveCardsToHandFromPile(0, 3);
        // ２プレイヤーは手札から３枚抜いて、場札として置く
        MoveCardsToHandFromPile(1, 3);
        yield return new WaitForSeconds(seconds);
    }

    /// <summary>
    /// 台札を、手札へ移動する
    /// </summary>
    /// <param name="place">右:0, 左:1</param>
    void MoveCardsToPileFromCenterStacks(int place)
    {
        // 台札の一番上（一番後ろ）のカードを１枚抜く
        var numberOfCards = 1;
        var length = gameViewModel.GetLengthOfCenterStackCards(place); // 台札の枚数
        if (1 <= length)
        {
            var startIndex = length - numberOfCards;
            var goCard = gameViewModel.GetCardOfCenterStack(place, startIndex);
            gameViewModel.RemoveCardAtOfCenterStack(place, startIndex);

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
            gameViewModel.AddCardOfPlayersPile(player, goCard);
            SetPosRot(goCard, gameViewModel.pileCardsX[player], gameViewModel.pileCardsY[player], gameViewModel.pileCardsZ[player], angleY: angleY, angleZ: 180.0f);
            gameViewModel.pileCardsY[player] += 0.2f;
        }
    }

    /// <summary>
    /// 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
    /// 
    /// - 画面上の場札は位置調整される
    /// </summary>
    void MoveCardsToHandFromPile(int player, int numberOfCards)
    {
        // 手札の上の方からｎ枚抜いて、場札へ移動する
        var length = gameViewModel.GetLengthOfPlayerPileCards(player); // 手札の枚数
        if (numberOfCards <= length)
        {
            var startIndex = length - numberOfCards;
            var goCards = gameViewModel.GetRangeCardsOfPlayerPile(player, startIndex, numberOfCards);
            gameViewModel.RemoveRangeCardsOfPlayerPile(player, startIndex, numberOfCards);
            gameViewModel.AddRangeCardsOfPlayerHand(player, goCards);

            // 場札を並べる
            ArrangeHandCards(player);
        }
    }

    /// <summary>
    /// 場札を並べる
    /// </summary>
    void ArrangeHandCards(int player)
    {
        // 25枚の場札が並べるように調整してある

        int numberOfCards = gameViewModel.GetLengthOfPlayerHandCards(player); // 場札の枚数
        if (numberOfCards < 1)
        {
            return; // 何もしない
        }

        float cardAngleZ = -5; // カードの少しの傾き

        int range = 200; // 半径。大きな円にするので、中心を遠くに離したい
        int offsetCircleCenterZ; // 中心位置の調整

        float angleY;
        float playerTheta;
        // float leftestAngle = 112.0f;
        float angleStep = -1.83f;
        float startTheta = (numberOfCards * Mathf.Abs(angleStep) / 2 - Mathf.Abs(angleStep) / 2 + 90.0f) * Mathf.Deg2Rad;
        float thetaStep = angleStep * Mathf.Deg2Rad; ; // 時計回り

        float ox = 0.0f;
        float oz = gameViewModel.handCardsZ[player];


        switch (player)
        {
            case 0:
                // １プレイヤー
                angleY = 180.0f;
                playerTheta = 0;
                offsetCircleCenterZ = -190;
                break;

            case 1:
                // ２プレイヤー
                angleY = 0.0f;
                playerTheta = 180 * Mathf.Deg2Rad;
                offsetCircleCenterZ = 188;  // カメラのパースペクティブが付いているから、目視で調整
                break;

            default:
                throw new Exception();
        }

        float theta = startTheta;
        foreach (var goCard in gameViewModel.GetCardsOfPlayerHand(player))
        {
            float x = range * Mathf.Cos(theta + playerTheta) + ox;
            float z = range * Mathf.Sin(theta + playerTheta) + oz + offsetCircleCenterZ;

            SetPosRot(goCard, x, gameViewModel.minY, z, angleY: angleY, angleZ: cardAngleZ);
            theta += thetaStep;
        }

        // 場札を並べなおすと、持ち上げていたカードを下ろしてしまうので、再度、持ち上げる
        ResumeCardPickup(player);
    }

    /// <summary>
    /// 場札を並べなおすと、持ち上げていたカードを下ろしてしまうので、再度、持ち上げる
    /// </summary>
    void ResumeCardPickup(int player)
    {
        int handIndex = playsersFocusedCardIndex[player]; // 何枚目の場札をピックアップしているか
        if (0 <= handIndex && handIndex < gameViewModel.GetLengthOfPlayerHandCards(player)) // 範囲内なら
        {
            // 抜いたカードの右隣のカードを（有れば）ピックアップする
            var goNewPickupCard = gameViewModel.GetCardAtOfPlayerHand(player, handIndex);
            SetFocusHand(goNewPickupCard);
        }
    }

    /// <summary>
    /// ぴったり積むと不自然だから、X と Z を少しずらすための仕組み
    /// 
    /// - １プレイヤー、２プレイヤーのどちらも右利きと仮定
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    (float, float, float) MakeShakeForCenterStack(int player)
    {
        // １プレイヤーから見て。左上にずれていくだろう
        var left = -1.5f;
        var right = 0.5f;
        var bottom = -0.5f;
        var top = 1.5f;
        var angleY = UnityEngine.Random.Range(-10, 40); // 反時計回りに大きく捻りそう

        switch (player)
        {
            case 0:
                return (UnityEngine.Random.Range(left, right), UnityEngine.Random.Range(bottom, top), angleY);

            case 1:
                return (UnityEngine.Random.Range(-right, -left), UnityEngine.Random.Range(-top, -bottom), angleY);

            default:
                throw new Exception();
        }
    }

    /// <summary>
    /// 場札の好きなところから１枚抜いて、台札を１枚置く
    /// </summary>
    /// <param name="player">何番目のプレイヤー</param>
    /// <param name="place">右なら0、左なら1</param>
    private void MoveCardToCenterStackFromHand(int player, int place)
    {
        int handIndex = playsersFocusedCardIndex[player]; // 何枚目の場札をピックアップしているか
        if (handIndex < 0 || gameViewModel.goPlayersHandCards[player].Count <= handIndex) // 範囲外は無視
        {
            return;
        }

        var goCard = gameViewModel.GetCardAtOfPlayerHand(player, handIndex); // 場札を１枚抜いて
        gameViewModel.RemoveCardAtOfPlayerHand(player, handIndex);
        if (handIndex < 0 && 0 < gameViewModel.GetLengthOfPlayerHandCards(player))
        {
            handIndex = 0;
        }
        else if (gameViewModel.GetLengthOfPlayerHandCards(player) <= handIndex) // 範囲外アクセス防止対応
        {
            // 一旦、最後尾へ
            handIndex = gameViewModel.GetLengthOfPlayerHandCards(player) - 1;
        }
        // それでも範囲外なら、負の数
        playsersFocusedCardIndex[player] = handIndex; // 更新：何枚目の場札をピックアップしているか

        // 台札の一番上（一番後ろ）のカードの中心座標 X, Z
        float nextTopX;
        float nextTopZ;
        float nextAngleY = goCard.transform.rotation.eulerAngles.y;

        var (shakeX, shakeZ, shakeAngleY) = MakeShakeForCenterStack(place);

        var length = gameViewModel.GetLengthOfCenterStackCards(place);
        if (length < 1)
        {
            nextTopX = gameViewModel.centerStacksX[place];
            nextTopZ = gameViewModel.centerStacksZ[place];
        }
        else
        {
            var goLastCard = gameViewModel.GetCardOfCenterStack(place, length - 1); // 最後のカード
            nextTopX = (gameViewModel.centerStacksX[place] - goLastCard.transform.position.x) / 2 + gameViewModel.centerStacksX[place];
            nextTopZ = (gameViewModel.centerStacksZ[place] - goLastCard.transform.position.z) / 2 + gameViewModel.centerStacksZ[place];
            nextAngleY += shakeAngleY;
        }

        gameViewModel.AddCardOfCenterStack(place, goCard); // 台札として置く

        // カードの位置をセット
        SetPosRot(goCard, nextTopX + shakeX, gameViewModel.centerStacksY[place], nextTopZ + shakeZ, angleY: nextAngleY);

        // 次に台札に積むカードの高さ
        gameViewModel.centerStacksY[place] += 0.2f;

        // 場札の位置調整
        ArrangeHandCards(player);
    }

    /// <summary>
    /// 場札を取得
    /// </summary>
    /// <param name="player">何番目のプレイヤー</param>
    /// <param name="cardIndex">何枚目のカード</param>
    /// <param name="setCard">カードをセットする関数</param>
    private void GetCardOfHand(int player, int cardIndex, LazyArgs.SetValue<GameObject> setCard)
    {
        if (cardIndex < gameViewModel.GetLengthOfPlayerHandCards(player))
        {
            var goCard = gameViewModel.GetCardAtOfPlayerHand(player, cardIndex);
            setCard(goCard);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="card"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="angleY"></param>
    /// <param name="angleZ"></param>
    /// <param name="motionProgress">Update関数の中でないと役に立たない</param>
    private void SetPosRot(GameObject card, float x, float y, float z, float angleY = 180.0f, float angleZ = 0.0f, float motionProgress = 1.0f)
    {
        var beginPos = card.transform.position;
        var endPos = new Vector3(x, y, z);
        card.transform.position = Vector3.Lerp(beginPos, endPos, motionProgress);

        card.transform.rotation = Quaternion.Euler(0, angleY, angleZ);
    }

    /// <summary>
    /// 場札カードを持ち上げる
    /// </summary>
    /// <param name="card"></param>
    private void SetFocusHand(GameObject card)
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
    private void ResetFocusHand(GameObject card)
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

    /// <summary>
    /// 隣のカードへフォーカスを移します
    /// </summary>
    /// <param name="player"></param>
    /// <param name="direction">後ろ:0, 前:1</param>
    void MoveFocusToNextCard(int player, int direction)
    {
        int previous = playsersFocusedCardIndex[player];
        int current;
        var length = gameViewModel.GetLengthOfPlayerHandCards(player);

        if (length < 1)
        {
            // 場札が無いなら、何もピックアップされていません
            current = -1;
        }
        else
        {
            switch (direction)
            {
                // 後ろへ
                case 0:
                    if (previous == -1 || length <= previous + 1)
                    {
                        // （ピックアップしているカードが無いとき）先頭の外から、先頭へ入ってくる
                        current = 0;
                    }
                    else
                    {
                        current = previous + 1;
                    }
                    break;

                // 前へ
                case 1:
                    if (previous == -1 || previous - 1 < 0)
                    {
                        // （ピックアップしているカードが無いとき）最後尾の外から、最後尾へ入ってくる
                        current = length - 1;
                    }
                    else
                    {
                        current = previous - 1;
                    }
                    break;

                default:
                    throw new Exception();
            }
        }

        // 更新
        playsersFocusedCardIndex[player] = current;

        if (0 <= previous && previous < gameViewModel.GetLengthOfPlayerHandCards(player)) // 範囲内なら
        {
            // 前にフォーカスしていたカードを、盤に下ろす
            var goPreviousCard = gameViewModel.GetCardAtOfPlayerHand(player, previous);
            ResetFocusHand(goPreviousCard);
        }

        if (0 <= current && current < gameViewModel.GetLengthOfPlayerHandCards(player)) // 範囲内なら
        {
            // 今回フォーカスするカードを持ち上げる
            var goCurrentCard = gameViewModel.GetCardAtOfPlayerHand(player, current);
            SetFocusHand(goCurrentCard);
        }
    }
}
