﻿namespace Assets.Scripts.ThinkingEngine.Models
{
    /// <summary>
    /// プレイヤーの配列の添え字
    /// 
    /// - プレイヤー1 は 0
    /// - プレイヤー2 は 1
    /// </summary>
    class Player
    {
        // - 演算子のオーバーロード

        #region 演算子のオーバーロード（== と !=）
        // 📖 [自作クラスの演算子をオーバーロードする](https://dobon.net/vb/dotnet/beginner/operator.html)
        // 📖 [自作クラスのEqualsメソッドをオーバーライドして、等価の定義を変更する](https://dobon.net/vb/dotnet/beginner/equals.html)

        public static bool operator ==(Player c1, Player c2)
        {
            // nullの確認（構造体のようにNULLにならない型では不要）
            // 両方nullか（参照元が同じか）
            // (c1 == c2)とすると、無限ループ
            if (object.ReferenceEquals(c1, c2))
            {
                return true;
            }

            // どちらかがnullか
            // (c1 == null)とすると、無限ループ
            if (((object)c1 == null) || ((object)c2 == null))
            {
                return false;
            }

            return (c1.source == c2.source) && (c1.source == c2.source);
        }

        public static bool operator !=(Player c1, Player c2)
        {
            // (c1 != c2)とすると、無限ループ
            return !(c1 == c2);
        }

        /// <summary>
        /// objと自分自身が等価のときはtrueを返す
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            //objがnullか、型が違うときは、等価でない
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }
            //この型が継承できないクラスや構造体であれば、次のようにできる
            //if (!(obj is Player))

            //Numberで比較する
            Player c = (Player)obj;
            return (this.source == c.source);
            //または、
            //return (this.Number.Equals(c.Number));
        }

        //Equalsがtrueを返すときに同じ値を返す
        public override int GetHashCode()
        {
            return this.source;
        }
        #endregion

        // - その他

        internal Player(int source)
        {
            this.source = source;
        }

        // - フィールド

        /// <summary>
        /// 値
        /// </summary>
        int source;

        // - プロパティー

        /// <summary>
        /// 整数型形式で取得
        /// 
        /// - プレイヤー1 は 0
        /// - プレイヤー2 は 1
        /// </summary>
        internal int AsInt => source;
    }
}
