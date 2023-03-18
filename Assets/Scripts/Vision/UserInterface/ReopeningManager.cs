namespace Assets.Scripts.Vision.UserInterface
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// 両プレイヤーが置けるカードがなくなってしまったとき、
    /// カウントダウンを開始して、
    /// ピックアップ中の場札を　強制的に　台札へ置かせます
    /// </summary>
    internal class ReopeningManager : MonoBehaviour
    {
        /// <summary>
        /// 両プレイヤーが置けるカードがなくなってしまったとき、
        /// カウントダウンを開始して、
        /// ピックアップ中の場札を　強制的に　台札へ置かせます
        /// </summary>
        internal void DoIt()
        {
            StartCoroutine(WorkingOfReopening());
        }

        /// <summary>
        /// コルーチン
        /// </summary>
        internal IEnumerator WorkingOfReopening()
        {
            // カウントダウン
            // ==============
            Debug.Log("再開 3");
            yield return new WaitForSeconds(1f);

            Debug.Log("再開 2");
            yield return new WaitForSeconds(1f);

            Debug.Log("再開 1");
            yield return new WaitForSeconds(1f);

            Debug.Log("強制 カード置き");
            yield return null;
        }
    }
}
