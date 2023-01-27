using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ���{�ƊC�O�Ł@���[���ƃv���C���O�E�X�^�C���ɈႢ������̂�
/// �p��ɓ��ꊴ�͂Ȃ�
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// ��D
    /// 0: �P�v���C���[�i���F�j
    /// 1: �Q�v���C���[�i���F�j
    /// </summary>
    List<List<GameObject>> goPlayersPileCards = new() { new(), new() };

    /// <summary>
    /// ��D
    /// 0: �P�v���C���[�i���F�j
    /// 1: �Q�v���C���[�i���F�j
    /// </summary>
    List<List<GameObject>> goPlayersHandCards = new() { new(), new() };

    /// <summary>
    /// ��D
    /// 0: �E
    /// 1: ��
    /// </summary>
    List<List<GameObject>> goCenterStacksCards = new() { new(), new() };

    // Start is called before the first frame update
    void Start()
    {
        // ��D
        // �Q�U�����J�[�h���W�߂�
        for (int i = 1; i < 14; i++)
        {
            // �P�v���C���[�i���F�j
            goPlayersPileCards[0].Add(GameObject.Find($"Clubs {i}"));
            goPlayersPileCards[0].Add(GameObject.Find($"Spades {i}"));

            // �Q�v���C���[�i�ԐF�j
            goPlayersPileCards[1].Add(GameObject.Find($"Diamonds {i}"));
            goPlayersPileCards[1].Add(GameObject.Find($"Hearts {i}"));
        }

        for (int player = 0; player < 2; player++)
        {
            // �V���b�t��
            goPlayersPileCards[player] = goPlayersPileCards[player].OrderBy(i => Guid.NewGuid()).ToList();

            // ��D���S���z��
            var goCards = goPlayersPileCards[player].GetRange(0, 4);
            goPlayersPileCards[player].RemoveRange(0, 4);
            goPlayersHandCards[player].AddRange(goCards);

            // ��D���P���z��
            goCards = goPlayersPileCards[player].GetRange(0, 1);
            goPlayersPileCards[player].RemoveRange(0, 1);
            goCenterStacksCards[player].AddRange(goCards);
        }

        float minX = -62.0f;
        float leftCenterStackX = -15.0f;
        float rightCenterStackX = 15.0f;
        float maxX = 62.0f;
        float player2HandZ = 42.0f;
        float player2PileZ = 26.0f;
        float leftCenterStackZ = 10.0f;
        float rightCenterStackZ = 0.0f;
        float player1PileZ = -12.0f;
        float player1HandZ = -28.0f;


        // �Q�v���C���[�̏�D����ׂ�
        {
            float x = minX;
            float y = 0.0f;
            float z = player2HandZ;
            float xStep = (maxX - minX) / (goPlayersHandCards[1].Count - 1);
            foreach (var goCard in goPlayersHandCards[1])
            {
                SetPosRot(goCard, x, y, z, angleY: 0.0f);
                x += xStep;
            }
        }

        // �Q�v���C���[�̎�D��ςݏグ��
        {
            float x = minX;
            float y = 0.0f;
            float z = player2PileZ;
            foreach (var goCard in goPlayersPileCards[1])
            {
                SetPosRot(goCard, x, y, z, angleY: 0.0f, angleZ: 180.0f);
                y += 0.2f;
            }
        }

        // ���̑�D��ςݏグ��
        {
            float x = leftCenterStackX;
            float y = 0.0f;
            float z = leftCenterStackZ;
            foreach (var goCard in goCenterStacksCards[1])
            {
                SetPosRot(goCard, x, y, z, angleY:0.0f);
                y += 0.2f;
            }
        }

        // �E�̑�D��ςݏグ��
        {
            float x = rightCenterStackX;
            float y = 0.0f;
            float z = rightCenterStackZ;
            foreach (var goCard in goCenterStacksCards[0])
            {
                SetPosRot(goCard, x, y, z);
                y += 0.2f;
            }
        }

        // �P�v���C���[�̎�D��ςݏグ��
        {
            float x = maxX;
            float y = 0.0f;
            float z = player1PileZ;
            foreach (var goCard in goPlayersPileCards[0])
            {
                SetPosRot(goCard, x, y, z, angleZ:180.0f);
                y += 0.2f;
            }
        }

        // �P�v���C���[�̏�D����ׂ�
        {
            float x = minX;
            float y = 0.0f;
            float z = player1HandZ;
            float xStep = (maxX - minX) / (goPlayersHandCards[0].Count - 1);
            foreach (var goCard in goPlayersHandCards[0])
            {
                SetPosRot(goCard, x, y, z);
                x += xStep;
            }
        }
    }

    private void SetPosRot(GameObject card, float x, float y, float z, float angleY=180.0f, float angleZ = 0.0f)
    {
        card.transform.position = new Vector3(x, y, z);
        card.transform.rotation = Quaternion.Euler(0, angleY, angleZ);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
