using System;
using System.Threading;

namespace PokemonRPG
{
    public class Battle
    {
        Person Player1;
        Person Player2;
        public Battle(Person player1, Person player2)
        {
            this.Player1 = player1;
            this.Player2 = player2;
        }

        //素早い方が先行
        public void BattleStart()
        {
            Console.WriteLine("\nこのバトルはどちらかのモンスターが全員瀕死になるまで続きます。");
            Console.WriteLine("また、バトル中のモンスターチェンジはできません。\n");

            while (true)
            {
                if (Player1.myMonster[0]._LifeAndDeath == false && Player1.myMonster[1]._LifeAndDeath == false && Player1.myMonster[2]._LifeAndDeath == false)
                {
                    Console.WriteLine("\n===========================================================================================");
                    Console.WriteLine($"\n！！！！！！！！！！！！！！！{Player2.Name}の勝利！！！！！！！！！！！！！！！\n");
                    Console.WriteLine("===========================================================================================\n");
                    Console.WriteLine($"モンスターたちのレベルが１つずつ上がりました！！");
                    Console.WriteLine("---------------------------------------------------");
                    foreach (Monster monster in Player2.myMonster)
                    {
                        Console.WriteLine($"{monster._Name,-10}　レベル：{monster._Level,3} ⇨　{++monster._Level,3}");
                    }
                    Console.WriteLine("---------------------------------------------------");
                    Console.WriteLine("バトルを終了しメインメニューへ戻ります。\n\n\n\n\n\n");
                    break;
                }
                else if (Player2.myMonster[0]._LifeAndDeath == false && Player2.myMonster[1]._LifeAndDeath == false && Player2.myMonster[2]._LifeAndDeath == false)
                {
                    Console.WriteLine("\n\n\n\n\n\n===========================================================================================");
                    Console.WriteLine($"\n！！！！！！！！！！！！！！！{Player1.Name}の勝利！！！！！！！！！！！！！！！\n");
                    Console.WriteLine("===========================================================================================\n");
                    Console.WriteLine($"モンスターたちのレベルが１つずつ上がりました！！");
                    Console.WriteLine("---------------------------------------------------");
                    foreach (Monster monster in Player1.myMonster)
                    {
                        Console.WriteLine($"{monster._Name,-10}　レベル：{monster._Level,3} ⇨　{++monster._Level,3}");
                    }
                    Console.WriteLine("---------------------------------------------------");
                    Console.WriteLine("バトルを終了しメインメニューへ戻ります。\n\n\n\n\n\n");
                    break;
                }
                else
                {
                    Console.WriteLine($"\n{Player1.Name}さん　戦うモンスターを選びます。");
                    Monster monster1 = SelectBattleMonster(Player1);
                    Console.WriteLine($"\n{Player2.Name}さん　戦うモンスターを選びます。");
                    Monster monster2 = SelectBattleMonster(Player2);
                    //どちらかのモンスターが瀕死になるまで続ける
                    while (true)
                    {
                        if (monster1._LifeAndDeath == true && monster2._LifeAndDeath == true)
                        {
                            //プレイヤー１のモンスターが早い場合
                            if (CheckMonsterSpeed(monster1, monster2))
                            {
                                monster2 = monster1.Attack(monster2);
                                if(monster2._LifeAndDeath == true)
                                {
                                    monster1 = monster2.Attack(monster1);
                                }
                            }
                            //プレイヤー２のモンスターが早い場合
                            else
                            {
                                monster1 = monster2.Attack(monster1);
                                if(monster1._LifeAndDeath == true)
                                {
                                    monster2 = monster1.Attack(monster2);
                                }
                            }
                        }
                        else
                        {
                            if(monster1._LifeAndDeath == false)
                            {
                                Console.WriteLine("\n====================================================");
                                Console.WriteLine($"{monster1._Name}と{monster2._Name}の勝負は{monster2._Name}が勝った");
                                Console.WriteLine("====================================================");
                            }
                            else
                            {
                                Console.WriteLine("\n====================================================");
                                Console.WriteLine($"{monster2._Name}と{monster1._Name}の勝負は{monster1._Name}が勝った");
                                Console.WriteLine("====================================================");
                            }
                            break;
                        }
                    }
                }
            }
        }

        //各プレイヤーがすぐに戦わせるポケモンを選ぶ処理
        public Monster SelectBattleMonster(Person player)
        {
            Monster selectMonster;
            Console.WriteLine("--------------------------------------------------------------------------------");
            //自分のモンスター一覧をmyMonsterからコンソール出力
            int counter = 1;
            foreach (Monster monster in player.myMonster)
            {
                Console.WriteLine($"\nNo.{counter,3}");//モンスターのインデックス番号
                Console.Write($"{monster._Name}　レベル:{monster._Level,2}　属性:{monster._Type}　体力:{monster._NowHp,3}/{monster._MaxHp,3}　攻撃力:{monster._Attack,2}　防御力:{monster._Deffence,2}　素早さ:{monster._Speed,2}　必殺技:{monster._DeathBlow}");
                //瀕死のモンスターの場合下記一文を追加
                if (monster._LifeAndDeath == false)
                {
                    Console.Write($" ※ 瀕死");
                }
                Console.WriteLine("");
                counter++;
                Thread.Sleep(200);

            }
            Console.WriteLine("\n\t\t\t\t\t※ 相手モンスターより有利な属性だと攻撃力が２倍になります。　火＜水, 水＜草, 草＜火");
            Console.WriteLine("\t\t\t\t\t※ 必殺技は１ターンの溜めを必要とし、通常攻撃力の1~3倍の攻撃力になります。");
            Console.WriteLine("\t\t\t\t\t※ 攻撃は素早さの数値が高い方から先に行われます。");
            Console.WriteLine("\t\t\t\t\t※ 瀕死のモンスターは戦闘できません。\n");
            Console.WriteLine("--------------------------------------------------------------------------------");
            while (true)
            {
                Console.Write($"{player.Name}：\nどのモンスターに戦ってもらいますか？\n番号を入力してください\n＞＞");
                if (int.TryParse(Console.ReadLine(), out int selectNo) && 0 < selectNo && selectNo <= 3)
                {
                    selectMonster = player.myMonster[selectNo - 1];
                    //瀕死のモンスターを弾く処理
                    if (selectMonster._LifeAndDeath == true)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"\n{selectMonster._Name}は瀕死です。戦いに参加できません。選び直してください。\n");
                    }
                }
                else
                {
                    Console.WriteLine("入力値が正しくありません。半角数字で入力してください。");
                }
            }
            Console.WriteLine("\n=========================");
            Console.WriteLine($"{selectMonster._Name}、君に決めた！");
            Console.WriteLine("=========================\n");

            return selectMonster;
        }
        //引数にあるモンスターの素早さを比較し、player1のモンスターの方が早い場合trueを返す
        public bool CheckMonsterSpeed(Monster monster1, Monster monster2)
        {
            bool SpeedCheck = false;
            if(monster2._Speed <= monster1._Speed)
            {
                SpeedCheck = true;
            }
            return SpeedCheck;
        }

    }
}

