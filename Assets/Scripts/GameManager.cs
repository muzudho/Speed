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
    /// 手札
    /// 0: １プレイヤー（黒色）
    /// 1: ２プレイヤー（黒色）
    /// </summary>
    List<List<GameObject>> goPlayersPileCards = new() { new(), new() };

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

    // Start is called before the first frame update
    void Start()
    {
        // 手札
        // ２６枚ずつカードを集める
        for (int i = 1; i < 14; i++)
        {
            // １プレイヤー（黒色）
            goPlayersPileCards[0].Add(GameObject.Find($"Clubs {i}"));
            goPlayersPileCards[0].Add(GameObject.Find($"Spades {i}"));

            // ２プレイヤー（赤色）
            goPlayersPileCards[1].Add(GameObject.Find($"Diamonds {i}"));
            goPlayersPileCards[1].Add(GameObject.Find($"Hearts {i}"));
        }

        for (int player = 0; player < 2; player++)
        {
            // シャッフル
            goPlayersPileCards[player] = goPlayersPileCards[player].OrderBy(i => Guid.NewGuid()).ToList();

            // 場札を４枚配る
            var goCards = goPlayersPileCards[player].GetRange(0, 4);
            goPlayersPileCards[player].RemoveRange(0, 4);
            goPlayersHandCards[player].AddRange(goCards);

            // 台札を１枚配る
            goCards = goPlayersPileCards[player].GetRange(0, 1);
            goPlayersPileCards[player].RemoveRange(0, 1);
            goCenterStacksCards[player].AddRange(goCards);
        }

        float minX = -62.0f;
        float leftCenterStackX = -15.0f;
        float rightCenterStackX = 15.0f;
        float maxX = 62.0f;
        float minY = 0.5f; // 0.0f は盤
        float player2HandZ = 42.0f;
        float player2PileZ = 26.0f;
        float leftCenterStackZ = 10.0f;
        float rightCenterStackZ = 0.0f;
        float player1PileZ = -12.0f;
        float player1HandZ = -28.0f;


        // ２プレイヤーの場札を並べる（画面では、左から右へ並べる）
        {
            float x = maxX;
            float y = minY;
            float z = player2HandZ;
            float xStep = (maxX - minX) / (goPlayersHandCards[1].Count - 1);
            foreach (var goCard in goPlayersHandCards[1])
            {
                SetPosRot(goCard, x, y, z, angleY: 0.0f);
                x -= xStep;
            }
        }

        // ２プレイヤーの手札を積み上げる
        {
            float x = minX;
            float y = minY;
            float z = player2PileZ;
            foreach (var goCard in goPlayersPileCards[1])
            {
                SetPosRot(goCard, x, y, z, angleY: 0.0f, angleZ: 180.0f);
                y += 0.2f;
            }
        }

        // 左の台札を積み上げる
        {
            float x = leftCenterStackX;
            float y = minY;
            float z = leftCenterStackZ;
            foreach (var goCard in goCenterStacksCards[1])
            {
                SetPosRot(goCard, x, y, z, angleY: 0.0f);
                y += 0.2f;
            }
        }

        // 右の台札を積み上げる
        {
            float x = rightCenterStackX;
            float y = minY;
            float z = rightCenterStackZ;
            foreach (var goCard in goCenterStacksCards[0])
            {
                SetPosRot(goCard, x, y, z);
                y += 0.2f;
            }
        }

        // １プレイヤーの手札を積み上げる
        {
            float x = maxX;
            float y = minY;
            float z = player1PileZ;
            foreach (var goCard in goPlayersPileCards[0])
            {
                SetPosRot(goCard, x, y, z, angleZ: 180.0f);
                y += 0.2f;
            }
        }

        // １プレイヤーの場札を並べる
        {
            float x = minX;
            float y = minY;
            float z = player1HandZ;
            float xStep = (maxX - minX) / (goPlayersHandCards[0].Count - 1);
            foreach (var goCard in goPlayersHandCards[0])
            {
                SetPosRot(goCard, x, y, z);
                x += xStep;
            }
        }

        // １プレイヤーの１枚目のカードにフォーカスを当てる
        {
            if (0 < goPlayersHandCards[0].Count)
            {
                var goCard = goPlayersHandCards[0][0];
                SetFocus(goCard);
            }
        }
        // １プレイヤーの１枚目のカードのフォーカスを外す
        {
            if (0 < goPlayersHandCards[0].Count)
            {
                var goCard = goPlayersHandCards[0][0];
                ResetFocus(goCard);
            }
        }
        // １プレイヤーの１枚目のカードにフォーカスを当てる
        {
            if (1 < goPlayersHandCards[0].Count)
            {
                var goCard = goPlayersHandCards[0][1];
                SetFocus(goCard);
            }
        }

        // ２プレイヤーの１枚目のカードにフォーカスを当てる
        {
            if (0 < goPlayersHandCards[1].Count)
            {
                var goCard = goPlayersHandCards[1][0];
                SetFocus(goCard);
            }
        }
        // ２プレイヤーの１枚目のカードのフォーカスを外す
        {
            if (0 < goPlayersHandCards[1].Count)
            {
                var goCard = goPlayersHandCards[1][0];
                ResetFocus(goCard);
            }
        }
        // ２プレイヤーの２枚目のカードにフォーカスを当てる
        {
            if (1 < goPlayersHandCards[1].Count)
            {
                var goCard = goPlayersHandCards[1][1];
                SetFocus(goCard);
            }
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
