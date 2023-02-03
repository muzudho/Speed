namespace Assets.Scripts.Views
{
    using System;

    internal static class ViewHelper
    {
        /// <summary>
        /// ぴったり積むと不自然だから、X と Z を少しずらすための仕組み
        /// 
        /// - １プレイヤー、２プレイヤーのどちらも右利きと仮定
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        internal static (float, float, float) MakeShakeForCenterStack(int player)
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
    }
}
