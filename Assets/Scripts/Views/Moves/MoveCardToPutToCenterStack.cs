﻿namespace Assets.Scripts.Views.Moves
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Simulators.Timeline;
    using Assets.Scripts.Views.Timeline;
    using System;
    using UnityEngine;

    /// <summary>
    /// 台札へ置く
    /// </summary>
    internal static class MoveCardToPutToCenterStack
    {
        /// <summary>
        /// ムーブメント生成
        /// 
        /// - 場札を持ち上げる
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="getBegin"></param>
        /// <param name="idOfCard">カードId</param>
        internal static ViewMovement Generate(
            float startSeconds,
            float duration,
            int player,
            int place,
            Vector3 nextTop,
            IdOfPlayingCards target)
        {
            // 台札の位置をセット
            var targetGo = Specification.GetIdOfGameObject(target);

            // 手ぶれ
            var shakePosition = GameView.ShakePosition(place);

            Vector3? startPosition = null;
            Quaternion? startRotation = null;
            Vector3? endPosition = null;
            Quaternion? endRotation = null;

            return new ViewMovement(
                startSeconds:startSeconds,
                duration:duration,
                target: targetGo,
                getBegin: () => new PositionAndRotationLazy(
                    getPosition: () =>
                    {
                        // 初回アクセス時に、値固定
                        if (startPosition == null)
                        {
                            startPosition = GameObjectStorage.Items[targetGo].transform.position; // 抜いた場札
                        }
                        return startPosition ?? throw new Exception();
                    },
                    getRotation: () =>
                    {
                        // 初回アクセス時に、値固定
                        if (startRotation == null)
                        {
                            startRotation = GameObjectStorage.Items[targetGo].transform.rotation; // 抜いた場札
                        }
                        return startRotation ?? throw new Exception();
                    }),
                getEnd: () => new PositionAndRotationLazy(
                    getPosition: () =>
                    {
                        // 初回アクセス時に、値固定
                        if (endPosition == null)
                        {
                            endPosition = nextTop + shakePosition;
                        }
                        return endPosition ?? throw new Exception();
                    },
                    getRotation: () =>
                    {
                        // 初回アクセス時に、値固定
                        if (endRotation == null)
                        {
                            // １プレイヤー、２プレイヤーでカードの向きが違う
                            // また、元の捻りを保存していないと、補間で大回転してしまうようだ

                            var src = GameObjectStorage.Items[targetGo].transform.rotation; // 抜いた場札
                            var shake = GameView.ShakeRotation();
                            float yByPlayer;
                            if (player == 0) // １プレイヤーの方を 180°回転させる
                            {
                                yByPlayer = 180.0f;
                            }
                            else
                            {
                                yByPlayer = 0.0f;
                            }

                            endRotation = Quaternion.Euler(
                                x: src.x + shake.x,
                                y: src.y + shake.y + yByPlayer,
                                z: src.z + shake.z);
                        }
                        return endRotation ?? throw new Exception();
                    }));
        }

    }
}
