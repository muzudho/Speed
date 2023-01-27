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

        // ２プレイヤーの場札を並べる
        {
            float minX = -62.0f;
            float maxX = 62.0f;
            float x = -62.0f;
            float y = 0.0f;
            float z = 42.0f;
            float xStep = (maxX - minX) / goPlayersHandCards[1].Count;
            foreach (var goCard in goPlayersHandCards[1])
            {
                SetPosRot(goCard, x, y, z);
                x += xStep;
            }
        }

        // ２プレイヤーの手札を積み上げる
        {
            float x = -62.0f;
            float y = 0.0f;
            float z = 26.0f;
            foreach (var goCard in goPlayersPileCards[1])
            {
                SetPosRot(goCard, x, y, z);
                y += 0.2f;
            }
        }

        // 左の台札を積み上げる
        {
            float x = -15.0f;
            float y = 0.0f;
            float z = 10.0f;
            foreach (var goCard in goCenterStacksCards[1])
            {
                SetPosRot(goCard, x, y, z);
                y += 0.2f;
            }
        }

        // 右の台札を積み上げる
        {
            float x = 15.0f;
            float y = 0.0f;
            float z = 0.0f;
            foreach (var goCard in goCenterStacksCards[0])
            {
                SetPosRot(goCard, x, y, z);
                y += 0.2f;
            }
        }

        // １プレイヤーの手札を積み上げる
        {
            float x = 62.0f;
            float y = 0.0f;
            float z = -28.0f;
            foreach (var goCard in goPlayersPileCards[0])
            {
                SetPosRot(goCard, x, y, z);
                y += 0.2f;
            }
        }

        // １プレイヤーの場札を並べる
        {
            float minX = -62.0f;
            float maxX = 62.0f;
            float x = -62.0f;
            float y = 0.0f;
            float z = -28.0f;
            float xStep = (maxX - minX) / goPlayersHandCards[0].Count;
            foreach (var goCard in goPlayersHandCards[0])
            {
                SetPosRot(goCard, x, y, z);
                x += xStep;
            }
        }
    }

    private void SetPosRot(GameObject card, float x, float y, float z)
    {
        card.transform.position = new Vector3(x, y, z);
        card.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
