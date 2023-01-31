using Assets.Scripts;
using Assets.Scripts.Models;
using Assets.Scripts.Views;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// 日本と海外で　ルールとプレイング・スタイルに違いがあるので
/// 用語に統一感はない
/// </summary>
public class GameManager : MonoBehaviour
{
    GameModelBuffer gameModelBuffer;
    GameModel gameModel;
    GameViewModel gameViewModel;

    // Start is called before the first frame update
    void Start()
    {
        // 全てのカードのゲーム・オブジェクトを、IDに紐づける
        ViewStorage.Add(IdOfPlayingCards.Clubs1, GameObject.Find($"Clubs 1"));
        ViewStorage.Add(IdOfPlayingCards.Clubs2, GameObject.Find($"Clubs 2"));
        ViewStorage.Add(IdOfPlayingCards.Clubs3, GameObject.Find($"Clubs 3"));
        ViewStorage.Add(IdOfPlayingCards.Clubs4, GameObject.Find($"Clubs 4"));
        ViewStorage.Add(IdOfPlayingCards.Clubs5, GameObject.Find($"Clubs 5"));
        ViewStorage.Add(IdOfPlayingCards.Clubs6, GameObject.Find($"Clubs 6"));
        ViewStorage.Add(IdOfPlayingCards.Clubs7, GameObject.Find($"Clubs 7"));
        ViewStorage.Add(IdOfPlayingCards.Clubs8, GameObject.Find($"Clubs 8"));
        ViewStorage.Add(IdOfPlayingCards.Clubs9, GameObject.Find($"Clubs 9"));
        ViewStorage.Add(IdOfPlayingCards.Clubs10, GameObject.Find($"Clubs 10"));
        ViewStorage.Add(IdOfPlayingCards.Clubs11, GameObject.Find($"Clubs 11"));
        ViewStorage.Add(IdOfPlayingCards.Clubs12, GameObject.Find($"Clubs 12"));
        ViewStorage.Add(IdOfPlayingCards.Clubs13, GameObject.Find($"Clubs 13"));

        ViewStorage.Add(IdOfPlayingCards.Diamonds1, GameObject.Find($"Diamonds 1"));
        ViewStorage.Add(IdOfPlayingCards.Diamonds2, GameObject.Find($"Diamonds 2"));
        ViewStorage.Add(IdOfPlayingCards.Diamonds3, GameObject.Find($"Diamonds 3"));
        ViewStorage.Add(IdOfPlayingCards.Diamonds4, GameObject.Find($"Diamonds 4"));
        ViewStorage.Add(IdOfPlayingCards.Diamonds5, GameObject.Find($"Diamonds 5"));
        ViewStorage.Add(IdOfPlayingCards.Diamonds6, GameObject.Find($"Diamonds 6"));
        ViewStorage.Add(IdOfPlayingCards.Diamonds7, GameObject.Find($"Diamonds 7"));
        ViewStorage.Add(IdOfPlayingCards.Diamonds8, GameObject.Find($"Diamonds 8"));
        ViewStorage.Add(IdOfPlayingCards.Diamonds9, GameObject.Find($"Diamonds 9"));
        ViewStorage.Add(IdOfPlayingCards.Diamonds10, GameObject.Find($"Diamonds 10"));
        ViewStorage.Add(IdOfPlayingCards.Diamonds11, GameObject.Find($"Diamonds 11"));
        ViewStorage.Add(IdOfPlayingCards.Diamonds12, GameObject.Find($"Diamonds 12"));
        ViewStorage.Add(IdOfPlayingCards.Diamonds13, GameObject.Find($"Diamonds 13"));

        ViewStorage.Add(IdOfPlayingCards.Hearts1, GameObject.Find($"Hearts 1"));
        ViewStorage.Add(IdOfPlayingCards.Hearts2, GameObject.Find($"Hearts 2"));
        ViewStorage.Add(IdOfPlayingCards.Hearts3, GameObject.Find($"Hearts 3"));
        ViewStorage.Add(IdOfPlayingCards.Hearts4, GameObject.Find($"Hearts 4"));
        ViewStorage.Add(IdOfPlayingCards.Hearts5, GameObject.Find($"Hearts 5"));
        ViewStorage.Add(IdOfPlayingCards.Hearts6, GameObject.Find($"Hearts 6"));
        ViewStorage.Add(IdOfPlayingCards.Hearts7, GameObject.Find($"Hearts 7"));
        ViewStorage.Add(IdOfPlayingCards.Hearts8, GameObject.Find($"Hearts 8"));
        ViewStorage.Add(IdOfPlayingCards.Hearts9, GameObject.Find($"Hearts 9"));
        ViewStorage.Add(IdOfPlayingCards.Hearts10, GameObject.Find($"Hearts 10"));
        ViewStorage.Add(IdOfPlayingCards.Hearts11, GameObject.Find($"Hearts 11"));
        ViewStorage.Add(IdOfPlayingCards.Hearts12, GameObject.Find($"Hearts 12"));
        ViewStorage.Add(IdOfPlayingCards.Hearts13, GameObject.Find($"Hearts 13"));

        ViewStorage.Add(IdOfPlayingCards.Spades1, GameObject.Find($"Spades 1"));
        ViewStorage.Add(IdOfPlayingCards.Spades2, GameObject.Find($"Spades 2"));
        ViewStorage.Add(IdOfPlayingCards.Spades3, GameObject.Find($"Spades 3"));
        ViewStorage.Add(IdOfPlayingCards.Spades4, GameObject.Find($"Spades 4"));
        ViewStorage.Add(IdOfPlayingCards.Spades5, GameObject.Find($"Spades 5"));
        ViewStorage.Add(IdOfPlayingCards.Spades6, GameObject.Find($"Spades 6"));
        ViewStorage.Add(IdOfPlayingCards.Spades7, GameObject.Find($"Spades 7"));
        ViewStorage.Add(IdOfPlayingCards.Spades8, GameObject.Find($"Spades 8"));
        ViewStorage.Add(IdOfPlayingCards.Spades9, GameObject.Find($"Spades 9"));
        ViewStorage.Add(IdOfPlayingCards.Spades10, GameObject.Find($"Spades 10"));
        ViewStorage.Add(IdOfPlayingCards.Spades11, GameObject.Find($"Spades 11"));
        ViewStorage.Add(IdOfPlayingCards.Spades12, GameObject.Find($"Spades 12"));
        ViewStorage.Add(IdOfPlayingCards.Spades13, GameObject.Find($"Spades 13"));

        gameModelBuffer = new GameModelBuffer();
        gameModel = new GameModel(gameModelBuffer);
        gameViewModel = new GameViewModel();

        // ゲーム開始時、とりあえず、すべてのカードは、いったん右の台札という扱いにする
        const int right = 0;// 台札の右
                            // const int left = 1;// 台札の左
        foreach (var idOfCard in ViewStorage.PlayingCards.Keys)
        {
            // 右の台札
            gameModelBuffer.goCenterStacksCards[right].Add(idOfCard);
        }

        // 右の台札をシャッフル
        gameModelBuffer.goCenterStacksCards[right] = gameModelBuffer.goCenterStacksCards[right].OrderBy(i => Guid.NewGuid()).ToList();

        // 右の台札をすべて、色分けして、黒色なら１プレイヤーの、赤色なら２プレイヤーの、手札に乗せる
        while (0 < gameModel.GetLengthOfCenterStackCards(right))
        {
            this.MoveCardsToPileFromCenterStacks(right);
        }

        // １，２プレイヤーについて、手札から５枚抜いて、場札として置く（画面上の場札の位置は調整される）
        this.MoveCardsToHandFromPile(player: 0, numberOfCards: 5);
        this.MoveCardsToHandFromPile(player: 1, numberOfCards: 5);


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
            var player = 0;
            gameViewModel.MoveFocusToNextCard(
                gameModel: gameModel,
                player: player,
                direction: 1,
                indexOfFocusedHandCard: gameModelBuffer.IndexOfFocusedCardOfPlayers[player],
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                });
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）右隣のカードをピックアップするように変えます
            var player = 0;
            gameViewModel.MoveFocusToNextCard(
                gameModel: gameModel,
                player: player,
                direction: 0,
                indexOfFocusedHandCard: gameModelBuffer.IndexOfFocusedCardOfPlayers[player],
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                });
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
            var player = 1;
            gameViewModel.MoveFocusToNextCard(
                gameModel: gameModel,
                player: player,
                direction: 1,
                indexOfFocusedHandCard: gameModelBuffer.IndexOfFocusedCardOfPlayers[player],
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                });
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）右隣のカードをピックアップするように変えます
            var player = 1;
            gameViewModel.MoveFocusToNextCard(
                gameModel:gameModel,
                player: player,
                direction: 0,
                indexOfFocusedHandCard: gameModelBuffer.IndexOfFocusedCardOfPlayers[player],
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                });
        }

        // デバッグ用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 両プレイヤーは手札から１枚抜いて、場札として置く
            for (var player = 0; player < 2; player++)
            {
                // 場札を並べる
                this.MoveCardsToHandFromPile(
                    player:player,
                    numberOfCards:1);
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
        {
            var player = 0;
            gameViewModel.MoveFocusToNextCard(
                gameModel: gameModel,
                player: player,
                direction: 0,
                indexOfFocusedHandCard: gameModelBuffer.IndexOfFocusedCardOfPlayers[player],
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                });
        }
        // ２プレイヤーの先頭のカードへフォーカスを移します
        {
            var player = 1;
            gameViewModel.MoveFocusToNextCard(
                gameModel: gameModel,
                player: player,
                direction: 0,
                indexOfFocusedHandCard: gameModelBuffer.IndexOfFocusedCardOfPlayers[player],
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                });
        }
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
            {
                var player = 0;
                this.gameViewModel.MoveFocusToNextCard(
                    gameModel: gameModel,
                    player: player,
                    direction: 0,
                    indexOfFocusedHandCard: gameModelBuffer.IndexOfFocusedCardOfPlayers[player],
                    setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                    {
                        gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                    });
            }

            // ２プレイヤーの右隣のカードへフォーカスを移します
            {
                var player = 1;
                this.gameViewModel.MoveFocusToNextCard(
                    gameModel: gameModel,
                    player: player,
                    direction: 0,
                    indexOfFocusedHandCard: gameModelBuffer.IndexOfFocusedCardOfPlayers[player],
                    setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                    {
                        gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                    });
            }
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
        this.MoveCardsToHandFromPile(player: 0, numberOfCards: 3);
        // ２プレイヤーは手札から３枚抜いて、場札として置く
        this.MoveCardsToHandFromPile(player: 1, numberOfCards: 3);
        yield return new WaitForSeconds(seconds);
    }

    /// <summary>
    /// 場札の好きなところから１枚抜いて、台札を１枚置く
    /// </summary>
    /// <param name="player">何番目のプレイヤー</param>
    /// <param name="place">右なら0、左なら1</param>
    private void MoveCardToCenterStackFromHand(int player, int place)
    {
        // ピックアップしているカードがあるか？
        GetIndexOfFocusedHandCard(
            player: player,
            (indexOfFocusedHandCard) =>
            {
                this.RemoveAtOfHandCard(
                    player: player,
                    place: place,
                    indexOfHandCardToRemove: indexOfFocusedHandCard,
                    setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                    {
                        gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard; // 更新：何枚目の場札をピックアップしているか

                        // 場札の位置調整
                        gameViewModel.ArrangeHandCards(
                            gameModel: gameModel,
                            player: player);
                    });
            });
    }

    private void GetIndexOfFocusedHandCard(int player, LazyArgs.SetValue<int> setIndex)
    {
        int handIndex = gameModelBuffer.IndexOfFocusedCardOfPlayers[player]; // 何枚目の場札をピックアップしているか
        if (handIndex < 0 || gameModel.GetLengthOfPlayerHandCards(player) <= handIndex) // 範囲外は無視
        {
            return;
        }

        setIndex(handIndex);
    }

    /// <summary>
    /// 台札を、手札へ移動する
    /// </summary>
    /// <param name="place">右:0, 左:1</param>
    internal void MoveCardsToPileFromCenterStacks(int place)
    {
        // 台札の一番上（一番後ろ）のカードを１枚抜く
        var numberOfCards = 1;
        var length = gameModel.GetLengthOfCenterStackCards(place); // 台札の枚数
        if (1 <= length)
        {
            var startIndex = length - numberOfCards;
            var idOfCard = gameModel.GetCardOfCenterStack(place, startIndex);
            this.gameModelBuffer.RemoveCardAtOfCenterStack(place, startIndex);

            // 黒いカードは１プレイヤー、赤いカードは２プレイヤー
            int player;
            float angleY;
            var goCard = ViewStorage.PlayingCards[idOfCard];
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
            this.gameModelBuffer.AddCardOfPlayersPile(player, idOfCard);
            this.gameViewModel.SetPosRot(idOfCard, this.gameViewModel.pileCardsX[player], this.gameViewModel.pileCardsY[player], this.gameViewModel.pileCardsZ[player], angleY: angleY, angleZ: 180.0f);
            this.gameViewModel.pileCardsY[player] += 0.2f;
        }
    }

    /// <summary>
    /// 台札を抜く
    /// </summary>
    /// <param name="player"></param>
    /// <param name="indexOfHandCardToRemove"></param>
    /// <param name="setIndexOfNextFocusedHandCard"></param>
    internal void RemoveAtOfHandCard(int player, int place, int indexOfHandCardToRemove, LazyArgs.SetValue<int> setIndexOfNextFocusedHandCard)
    {
        // 抜く前の場札の数
        var lengthBeforeRemove = gameModel.GetLengthOfPlayerHandCards(player);
        if (indexOfHandCardToRemove < 0 || lengthBeforeRemove <= indexOfHandCardToRemove)
        {
            // 抜くのに失敗
            return;
        }

        // 抜いた後の場札の数
        var lengthAfterRemove = lengthBeforeRemove - 1;

        // 抜いた後の次のピックアップするカードが先頭から何枚目か、先に算出
        int indexOfNextFocusedHandCard;
        if (lengthAfterRemove <= indexOfHandCardToRemove) // 範囲外アクセス防止対応
        {
            // 一旦、最後尾へ
            indexOfNextFocusedHandCard = lengthAfterRemove - 1;
        }
        else
        {
            // そのまま
            indexOfNextFocusedHandCard = indexOfHandCardToRemove;
        }

        var goCard = gameModel.GetCardAtOfPlayerHand(player, indexOfHandCardToRemove); // 場札を１枚抜いて
        this.gameModelBuffer.RemoveCardAtOfPlayerHand(player, indexOfHandCardToRemove);

        this.AddCardOfCenterStack2(goCard, place); // 台札
        setIndexOfNextFocusedHandCard(indexOfNextFocusedHandCard);
    }

    internal void AddCardOfCenterStack2(IdOfPlayingCards idOfCard, int place)
    {
        // 手ぶれ
        var (shakeX, shakeZ, shakeAngleY) = this.gameViewModel.MakeShakeForCenterStack(place);

        // 台札の次の天辺（一番後ろ）のカードの中心座標 X, Z
        var (nextTopX, nextTopZ) = this.gameViewModel.GetXZOfNextCenterStackCard(gameModel, place);

        // 台札の捻り
        var goCard = ViewStorage.PlayingCards[idOfCard];
        float nextAngleY = goCard.transform.rotation.eulerAngles.y;
        var length = gameModel.GetLengthOfCenterStackCards(place);
        if (length < 1)
        {
        }
        else
        {
            nextAngleY += shakeAngleY;
        }

        this.gameModelBuffer.AddCardOfCenterStack(place, idOfCard); // 台札として置く

        // 台札の位置をセット
        this.gameViewModel.SetPosRot(idOfCard, nextTopX + shakeX, this.gameViewModel.centerStacksY[place], nextTopZ + shakeZ, angleY: nextAngleY);

        // 次に台札に積むカードの高さ
        this.gameViewModel.centerStacksY[place] += 0.2f;
    }

    /// <summary>
    /// 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
    /// 
    /// - 画面上の場札は位置調整される
    /// </summary>
    internal void MoveCardsToHandFromPile(int player, int numberOfCards)
    {
        // 手札の上の方からｎ枚抜いて、場札へ移動する
        var length = gameModel.GetLengthOfPlayerPileCards(player); // 手札の枚数
        if (numberOfCards <= length)
        {
            var startIndex = length - numberOfCards;
            var goCards = gameModel.GetRangeCardsOfPlayerPile(player, startIndex, numberOfCards);
            this.gameModelBuffer.RemoveRangeCardsOfPlayerPile(player, startIndex, numberOfCards);
            this.gameModelBuffer.AddRangeCardsOfPlayerHand(player, goCards);

            this.gameViewModel.ArrangeHandCards(gameModel, player);
        }
    }
}
