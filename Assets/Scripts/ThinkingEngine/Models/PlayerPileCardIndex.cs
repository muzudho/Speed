namespace Assets.Scripts.ThinkingEngine.Models
{
    using System;

    /// <summary>
    /// 手札の配列の添え字
    /// 
    /// - 未選択なら -1
    /// - １プレイヤーから見て左端が０
    /// </summary>
    class PlayerPileCardIndex
    {
        // - 演算子のオーバーロード

        #region 演算子のオーバーロード（== と !=）
        // 📖 [自作クラスの演算子をオーバーロードする](https://dobon.net/vb/dotnet/beginner/operator.html)
        // 📖 [自作クラスのEqualsメソッドをオーバーライドして、等価の定義を変更する](https://dobon.net/vb/dotnet/beginner/equals.html)

        public static bool operator ==(PlayerPileCardIndex c1, PlayerPileCardIndex c2)
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

        public static bool operator !=(PlayerPileCardIndex c1, PlayerPileCardIndex c2)
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
            //if (!(obj is PlayerPileCardIndex))

            //Numberで比較する
            PlayerPileCardIndex c = (PlayerPileCardIndex)obj;
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

        #region 演算子のオーバーロード（大小比較）
        /// <summary>
        /// 自分自身がotherより小さいときはマイナスの数、大きいときはプラスの数、
        /// 同じときは0を返す
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public int CompareTo(object other)
        {
            if ((object)other == null)
                return 1;
            if (this.GetType() != other.GetType())
                throw new ArgumentException();
            return this.source.CompareTo(((PlayerPileCardIndex)other).source);
        }

        /// <summary>
        /// 比較演算子の<と>をオーバーロードする
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool operator <(PlayerPileCardIndex c1, PlayerPileCardIndex c2)
        {
            //nullの確認
            if ((object)c1 == null || (object)c2 == null)
            {
                throw new ArgumentNullException();
            }
            //CompareToメソッドを呼び出す
            return (c1.CompareTo(c2) < 0);
        }

        public static bool operator >(PlayerPileCardIndex c1, PlayerPileCardIndex c2)
        {
            //逆にして"<"で比較
            return (c2 < c1);
        }

        //比較演算子の<=と>=をオーバーロードする
        public static bool operator <=(PlayerPileCardIndex c1, PlayerPileCardIndex c2)
        {
            //nullの確認
            if ((object)c1 == null || (object)c2 == null)
            {
                throw new ArgumentNullException();
            }
            //CompareToメソッドを呼び出す
            return (c1.CompareTo(c2) <= 0);
        }

        public static bool operator >=(PlayerPileCardIndex c1, PlayerPileCardIndex c2)
        {
            //逆にして"<="で比較
            return (c2 <= c1);
        }
        #endregion

        // - その他

        internal PlayerPileCardIndex(int source)
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
        /// </summary>
        internal int AsInt => source;
    }
}
