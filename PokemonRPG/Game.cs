using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRPG
{
    internal class Game
    {
        public Game()
        {
            Console.WriteLine("ようこそ、ポケモンバトルへ、このゲームはプレイヤー１とプレイヤー２が仲間のポケモンを戦わせるバトルです。");
            while (true)
            {
                Console.WriteLine("\n１：バトルを始める。\n２：モンスターリストを見る。\n３：新規モンスターを登録する。\n４：既存のモンスターを削除する。\n５：ポケモンセンターで瀕死のモンスターを回復させる。\n６：ゲームを終了する。");
                Console.Write("\nメニュー番号を選んでください。\n＞＞");
                if(int.TryParse(Console.ReadLine(), out int selectNo))
                {
                    switch (selectNo)
                    {
                        case 1:
                            Play();
                            break;
                        case 2:
                            Console.WriteLine("\nこちらが現在登録されているモンスター一覧です。");
                            MonsterList monsterList = new MonsterList();
                            monsterList.ReadAllMonsterData();
                            monsterList.ShowAllMonsterList();
                            Console.WriteLine("メニューへ戻ります。\n\n\n\n");
                            break;
                        case 3:
                            AddMonster();
                            break;
                        case 4:
                            RemoveMonster();
                            break;
                        case 5:
                            PokemonCenter();
                            break;
                        case 6:
                            Console.WriteLine("\n=========================");
                            Console.WriteLine("ポケモンバトルを終了します。");
                            Console.WriteLine("=========================\n");
                            goto saveEnd;
                        default:
                            Console.WriteLine("\n入力した数値が間違っています。半角数字で入力してください。\n");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\n入力した数値が間違っています。半角数字で入力してください。\n");
                }
            }
        saveEnd:;
        }
        public void Play()
        {
            //戦えるモンスターが6匹以上いないとゲームが開始できないようにする。
            if (CheckMonsterFile())
            {
                //プレイヤー１の登録と所持モンスター選択
                Console.Write("\nポケモンバトルを始めます。まずはプレイヤー１の名前を教えてください。\n＞＞");
                string? name1 = Console.ReadLine();
                if(name1 == "")
                {
                    name1 = "名無し";
                }
                Console.WriteLine($"\nこんにちは {name1} さん\n");
                Person person1 = new Person(name1);
                Console.WriteLine("次に一覧からモンスターを３体選びましょう");
                person1.SelectMonster();

                //プレイヤー２の登録と所持モンスター選択
                Console.Write("\n次にプレイヤー２の名前を教えてください。\n＞＞");
                string? name2 = Console.ReadLine();
                if (name2 == "")
                {
                    name2 = "名無し";
                }
                Console.WriteLine($"\nこんにちは {name2} さん\n");
                Person person2 = new Person(name2);
                Console.WriteLine("次に一覧からモンスターを３体選びましょう");
                person2.SelectMonster();

                //バトルクラスインスタンス生成
                Battle battle = new Battle(person1, person2);
                battle.BattleStart();

                //PersonクラスインスタンスのmyMonsterフィールドを保存
                person1.SaveMyMonster();
                person2.SaveMyMonster();
            }
            else
            {
                Console.WriteLine("戦えるモンスターが６体より少ないです。");
                Console.WriteLine("モンスターを追加して下さい。");
            }
        }

        //モンスターリストに戦えるモンスターが６体以上いるか確認しboolで返すメソッド
        public bool CheckMonsterFile()
        {
            int monsterCounter = 0;
            MonsterList monsterList = new MonsterList();
            monsterList.ReadAllMonsterData();
            foreach (string[] monsterData in monsterList.allMonsterList)
            {
                if (monsterData[10] == "True")
                {
                    monsterCounter++;
                }
            }
            bool leadyBattle = false;
            if(6 <= monsterCounter)
            {
                leadyBattle = true;
            }
            return leadyBattle;
        }

        //モンスターリストのモンスターを削除する
        public void RemoveMonster()
        {
            Console.WriteLine("\nこちらが現在登録されているモンスター一覧です。");
            MonsterList monsterList = new MonsterList();
            monsterList.ReadAllMonsterData();
            monsterList.ShowAllMonsterList();
            while (true)
            {
                Console.WriteLine("削除したいモンスターの番号を押して下さい。　キャンセルする場合は「０」を押して下さい。");
                if(int.TryParse(Console.ReadLine(), out int removemonsterNo) && 0 < removemonsterNo && removemonsterNo <= monsterList.allMonsterList.Count())
                {
                    monsterList.allMonsterList.Remove(monsterList.allMonsterList[removemonsterNo - 1]);
                    monsterList.SavingAllMonsterList();
                    Console.WriteLine("削除されました。");
                    break;
                }
                else if(removemonsterNo == 0)
                {
                    Console.WriteLine("削除をキャンセルします。");
                    break;
                }
                else
                {
                    Console.WriteLine("\n入力した数値が間違っています。半角数字で入力してください。\n");
                }
            }
        }


        //モンスター情報の登録メソッド
        //現在のモンスター一覧を表示　→　モンスター情報入力　→　string[]に変換してallMonsterListリスト追加　→　書き込み
        public void AddMonster()
        {
            MonsterList monsterList = new MonsterList();
            monsterList.CreateMonster();
        }

        //ポケモンセンターへ行きモンスターを回復させる
        public void PokemonCenter()
        {
            MonsterList monsterList = new MonsterList();
            monsterList.PokemonCenter();
        }
    }
}
