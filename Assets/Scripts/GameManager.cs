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

        // �Q�v���C���[�̏�D����ׂ�
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

        // �Q�v���C���[�̎�D��ςݏグ��
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

        // ���̑�D��ςݏグ��
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

        // �E�̑�D��ςݏグ��
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

        // �P�v���C���[�̎�D��ςݏグ��
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

        // �P�v���C���[�̏�D����ׂ�
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
