using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRPG
{
    public class Monster
    {
        //属性攻撃は２ターン消費し、物理攻撃の1~5倍
        //フィールド変数
        public string _Name { get; set; }//モンスター名
        public int _Level { get; set; }//レベルは戦い後に１アップ。
        public string _Type { get; set; }//属性　Fire, Water, Green
        public int _MaxHp { get; set; }//最大体力
        public int _NowHp { get; set; }//現在の体力
        public int _Attack { get; set; }//攻撃力
        public int _Deffence { get; set; }//防御力
        public int _Speed { get; set; }//素早さ...素早い方が先に攻撃を仕掛ける
        public string _DeathBlow { get; set; }//必殺技名
        public bool _WaitingTurnCounter { get; set; }//必殺技の待機ターン数　true == 必殺技発動準備完了
        public bool _LifeAndDeath { get; set; }//モンスターの生死状態　false == 瀕死
        public bool _FellowFlag { get; set; }//モンスターのパーティーステータス　true ==　仲間にできる。

        //コンストラクター
        public Monster(string name, int level, string Type, int maxhp, int nowhp, int attack, int deffence, int speed, string deathblow, bool waitturn, bool lifeanddeath, bool fellow)
        {
            this._Name = name;//0
            this._Level = level;//1
            this._Type = Type;//2
            this._MaxHp = maxhp;//3
            this._NowHp = nowhp;//4
            this._Attack = attack;//5
            this._Deffence = deffence;//6
            this._Speed = speed;//7
            this._DeathBlow = deathblow;//8
            this._WaitingTurnCounter = waitturn;//9
            this._LifeAndDeath = lifeanddeath;//10
            this._FellowFlag = fellow;//11
        }

        //モンスターの攻撃メソッド
        //相手モンスターのインスタンスを引数にダメージを与えた相手モンスター情報を返す。参照型だから返さないでもOK？
        public Monster Attack(Monster TargetMonster)
        {
            //優位な属性の場合攻撃力2倍
            int compability = 1;
            switch (this._Type)
            {
                case "火":
                    if(TargetMonster._Type == "草")
                    {
                        compability = 2;
                    }
                    break;
                case "水":
                    if (TargetMonster._Type == "火")
                    {
                        compability = 2;
                    }
                    break;
                case "草":
                    if (TargetMonster._Type == "水")
                    {
                        compability = 2;
                    }
                    break;
            }

            //必殺技の待機ターンを満たしている場合、必殺技発動
            if (this._WaitingTurnCounter == true)
            {
                Console.WriteLine($"\n{this._Name}の{this._DeathBlow}が発動した");
                //５回に１回は攻撃を回避される処理
                Random random = new Random();
                int destiny = random.Next(1, 6);
                if (1 < destiny)
                {
                    //必殺技の攻撃力をランダムに１〜３倍に
                    Random randomAttack = new Random();
                    int attackCondition = randomAttack.Next(1, 4);
                    TargetMonster._NowHp -= (((this._Attack * compability) * attackCondition) - TargetMonster._Deffence);
                    switch (attackCondition)
                    {
                        case 1:
                            Console.WriteLine($"{this._Name}は調子が悪かったようだ。通常と同じ攻撃力");
                            break;
                        case 2:
                            Console.WriteLine($"通常の{attackCondition}倍の攻撃力");
                            break;
                        case 3:
                            Console.WriteLine($"{this._Name}は絶好調だったようだ。通常の{attackCondition}倍の攻撃力");
                            break;
                    }
                    Console.WriteLine($"{TargetMonster._Name}へ{((this._Attack * compability) * attackCondition) - TargetMonster._Deffence}のダメージ。");
                    //敵モンスターのHPにより表示の変更と瀕死の場合は生死状態をfalseに変更
                    if (0 < TargetMonster._NowHp)
                    {
                        Console.WriteLine($"{TargetMonster._Name}の残りHP:{TargetMonster._NowHp}/{TargetMonster._MaxHp}");
              
                    }
                    else
                    {
                        Console.WriteLine($"{TargetMonster._Name}は瀕死になった。");
                        TargetMonster._LifeAndDeath = false;
                        TargetMonster._NowHp = 0;
                    }
                }
                else
                {
                    Console.WriteLine("攻撃をかわされた。");
                }
                this._WaitingTurnCounter = false;
            }
            //攻撃選択メニュー
            else
            {
                while (true)
                {
                    Console.Write($"\n{this._Name}の攻撃を行います。\n１：通常攻撃　２：{this._DeathBlow}\n＞＞");
                    //正しい攻撃方法を受け取るまで繰り返し処理
                    if (int.TryParse(Console.ReadLine(), out int selectAttack))
                    {
                        //通常攻撃の場合
                        if (selectAttack == 1)
                        {
                            //５回に１回は攻撃を回避される処理
                            Random random = new Random();
                            int destiny = random.Next(1, 6);
                            if (1 < destiny)
                            {
                                //ダメージ　＝　攻撃力　ー　防御力
                                Console.WriteLine($"{this._Name}が{TargetMonster._Name}を攻撃します");
                                TargetMonster._NowHp -= ((this._Attack * compability) - TargetMonster._Deffence);
                                Console.WriteLine($"{this._Name}の攻撃が当たった。");
                                //敵モンスターのHPにより表示の変更と瀕死の場合は生死状態をfalseに変更
                                if (0 < TargetMonster._NowHp)
                                {
                                    Console.WriteLine($"{TargetMonster._Name}へ{(this._Attack * compability) - TargetMonster._Deffence}のダメージ。　{TargetMonster._Name}の残りHP:{TargetMonster._NowHp}/{TargetMonster._MaxHp}");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine($"{TargetMonster._Name}へ{(this._Attack * compability) - TargetMonster._Deffence}のダメージ。　{TargetMonster._Name}は瀕死になった。");
                                    TargetMonster._LifeAndDeath = false;
                                    TargetMonster._NowHp = 0;
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("攻撃をかわされた。");
                                break;
                            }
                        }
                        //必殺技の場合...１ターン力を貯めて（スキップ）２ターン目で1~5倍の攻撃力
                        else if(selectAttack == 2)
                        {
                            Console.WriteLine($"{this._Name}は力を溜め始めた。１ターンはかかりそうだ。");
                            this._WaitingTurnCounter = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("入力した値が正しくありません。半角数字で入力してください。");
                        }
                    }
                    else
                    {
                        Console.WriteLine("入力した値が正しくありません。半角数字で入力してください。");
                    }
                }
            }
            Console.WriteLine("---------------------------------------------------");
            return TargetMonster;
        }
    }
}