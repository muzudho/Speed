using Assets.Scripts;
using System.Collections;
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
        int player0HandIndex = playsersFocusedCardIndex[0]; // 何枚目の場札をピックアップしているか
        int player1HandIndex = playsersFocusedCardIndex[1]; // 何枚目の場札をピックアップしているか

        gameViewModel = new GameViewModel();
        gameViewModel.Init(
            player0HandIndex: player0HandIndex,
            player1HandIndex: player1HandIndex);

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
                player: player,
                direction: 1,
                indexOfFocusedHandCard: playsersFocusedCardIndex[player],
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    playsersFocusedCardIndex[player] = indexOfNextFocusedHandCard;     // 更新
                });
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）右隣のカードをピックアップするように変えます
            var player = 0;
            gameViewModel.MoveFocusToNextCard(
                player: player,
                direction: 0,
                indexOfFocusedHandCard: playsersFocusedCardIndex[player],
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    playsersFocusedCardIndex[player] = indexOfNextFocusedHandCard;     // 更新
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
                player: player,
                direction: 1,
                indexOfFocusedHandCard: playsersFocusedCardIndex[player],
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    playsersFocusedCardIndex[player] = indexOfNextFocusedHandCard;     // 更新
                });
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）右隣のカードをピックアップするように変えます
            var player = 1;
            gameViewModel.MoveFocusToNextCard(
                player: player,
                direction: 0,
                indexOfFocusedHandCard: playsersFocusedCardIndex[player],
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    playsersFocusedCardIndex[player] = indexOfNextFocusedHandCard;     // 更新
                });
        }

        // デバッグ用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 両プレイヤーは手札から１枚抜いて、場札として置く
            for (var player = 0; player < 2; player++)
            {
                // 場札を並べる
                int handIndex = playsersFocusedCardIndex[player]; // 何枚目の場札をピックアップしているか
                gameViewModel.MoveCardsToHandFromPile(player, 1, handIndex);
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
                player: player,
                direction: 0,
                indexOfFocusedHandCard: playsersFocusedCardIndex[player],
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    playsersFocusedCardIndex[player] = indexOfNextFocusedHandCard;     // 更新
                });
        }
        // ２プレイヤーの先頭のカードへフォーカスを移します
        {
            var player = 1;
            gameViewModel.MoveFocusToNextCard(
                player: player,
                direction: 0,
                indexOfFocusedHandCard: playsersFocusedCardIndex[player],
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    playsersFocusedCardIndex[player] = indexOfNextFocusedHandCard;     // 更新
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
                gameViewModel.MoveFocusToNextCard(
                    player: player,
                    direction: 0,
                    indexOfFocusedHandCard: playsersFocusedCardIndex[player],
                    setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                    {
                        playsersFocusedCardIndex[player] = indexOfNextFocusedHandCard;     // 更新
                    });
            }

            // ２プレイヤーの右隣のカードへフォーカスを移します
            {
                var player = 1;
                gameViewModel.MoveFocusToNextCard(
                    player: player,
                    direction: 0,
                    indexOfFocusedHandCard: playsersFocusedCardIndex[player],
                    setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                    {
                        playsersFocusedCardIndex[player] = indexOfNextFocusedHandCard;     // 更新
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
        int player0HandIndex = playsersFocusedCardIndex[0]; // 何枚目の場札をピックアップしているか
        gameViewModel.MoveCardsToHandFromPile(player: 0, numberOfCards: 3, indexOfFocusedHandCard: player0HandIndex);
        // ２プレイヤーは手札から３枚抜いて、場札として置く
        int player1fHandIndex = playsersFocusedCardIndex[1]; // 何枚目の場札をピックアップしているか
        gameViewModel.MoveCardsToHandFromPile(player: 1, numberOfCards: 3, indexOfFocusedHandCard: player1fHandIndex);
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
                gameViewModel.RemoveAtOfHandCard(
                    player: player,
                    place: place,
                    indexOfFocusedHandCard: indexOfFocusedHandCard,
                    setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                    {
                        playsersFocusedCardIndex[player] = indexOfNextFocusedHandCard; // 更新：何枚目の場札をピックアップしているか
                    });
            });
    }

    private void GetIndexOfFocusedHandCard(int player, LazyArgs.SetValue<int> setIndex)
    {
        int handIndex = playsersFocusedCardIndex[player]; // 何枚目の場札をピックアップしているか
        if (handIndex < 0 || gameViewModel.GetLengthOfPlayerHandCards(player) <= handIndex) // 範囲外は無視
        {
            return;
        }

        setIndex(handIndex);
    }
}
