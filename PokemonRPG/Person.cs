using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PokemonRPG
{
    //このクラスでは登場人物の名前と所有ポケモンを３体選びオブジェクトとして纏める
    public class Person
    {
        public string Name { get;}//名前
        public Monster[] myMonster = new Monster[3];//所持しているモンスター
        public int[] selectedIndexNumbers = new int[3];//選んだインデックス番号

        public Person(string name)
        {
            this.Name = name;
        }

        public void SelectMonster()
        {
            //三体のモンスターを連続で選ぶ繰り返し処理。
            for (int i = 0; i < 3; i++)
            {
                //i番目のモンスターを選ぶ処理
                Console.WriteLine($"「{i + 1}」体目のモンスターNoを選んでください。");
                //MonsterListインスタンス生成後、モンスター一覧を表示
                MonsterList monsterList = new MonsterList();
                monsterList.AddAllMonsterList();
                monsterList.ShowAllMonsterList();
                while (true)
                {
                    //モンスターの正しいインデックスナンバーを入力するまで繰り返し入力を促す処理
                    Console.Write("仲間にするモンスターNoを半角数字で入力してください。\n＞＞");
                    if (int.TryParse(Console.ReadLine(), out int selectNo) && 0 < selectNo && selectNo <= monsterList.allMonsterList.Count)
                    {
                        //既にプレイヤー１orプレイヤー２の仲間になっていないかチェック
                        if (FellowCheckMonster(monsterList, selectNo))
                        {
                            //瀕死のモンスターではないかチェック
                            if (LifeAndDeadMonster(monsterList, selectNo))
                            {
                                //選択したインデックスNoのモンスターをPersonインスタンスのフィールドに代入
                                myMonster[i] = monsterList.GetMonster(selectNo);
                                //選択したインデックスNo.をインスタンス消滅まで保存しとくやーつ
                                selectedIndexNumbers[i] = selectNo;
                                Console.WriteLine("\n=============================");
                                Console.WriteLine($"{myMonster[i]._Name}を仲間にしました。");
                                Console.WriteLine("=============================\n");
                                //選択したモンスターのallMonsterListのFellowFlagをfalseにする処理を
                                monsterList.ChangeFellowMonsterFlag(selectNo);
                                //monsterList.allMonsterListをテキストファイルに出力
                                monsterList.SavingAllMonsterList();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("\nそのモンスターは瀕死なので仲間にできません。");
                                Console.WriteLine("モンスターを選び直してください。\n");

                            }
                        }
                        else
                        {
                            Console.WriteLine("\nそのモンスターは既に仲間になっています。");
                            Console.WriteLine("モンスターを選び直してください。\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n入力値が正しくありません。半角数字で入力してください。\n");
                    }
                }
            }
            Console.WriteLine($"{this.Name}さんのモンスター");
            Console.WriteLine("\n=============================");
            foreach (Monster monsterData in myMonster)
            {
                Console.WriteLine($"{monsterData._Name}");
            }
            Console.WriteLine("=============================\n");
        }

        //選択されたモンスターが瀕死ではないか確認するメソッド
        public static bool LifeAndDeadMonster(MonsterList monsterList, int monsterNo)
        {
            bool deadOrLife = true;

            if (monsterList.deadMonsterNoList.Contains(monsterNo))
            {
                deadOrLife = false;
            }
            return deadOrLife;
        }
        //選択されたモンスターが既に仲間になっていないか確認するメソッド
        public bool FellowCheckMonster(MonsterList monsterList, int monsterNo)
        {
            bool fellowCheck = false;

            string[] monster = monsterList.allMonsterList[monsterNo - 1];
            if (monster[11] == "True")
            {
                fellowCheck = true;
            }
            return fellowCheck;
        }

        //myMonsterのMonster情報をMonsterList.allMonsterListに反映して書き込み
        //fellowFlagもfalseに
        public void SaveMyMonster()
        {
            for (int i = 0; i < 3; i++)
            {
                //Monster型をstring[]に
                Monster monster = myMonster[i];
                string[] monsterArray = new string[12];
                monsterArray[0] = monster._Name;
                monsterArray[1] = monster._Level.ToString();
                monsterArray[2] = monster._Type;
                monsterArray[3] = monster._MaxHp.ToString();
                monsterArray[4] = monster._NowHp.ToString();
                monsterArray[5] = monster._Attack.ToString();
                monsterArray[6] = monster._Deffence.ToString();
                monsterArray[7] = monster._Speed.ToString();
                monsterArray[8] = monster._DeathBlow;
                monsterArray[9] = "False";
                monsterArray[10] = monster._LifeAndDeath.ToString();
                monsterArray[11] = "True";

                MonsterList monsterList = new MonsterList();
                monsterList.AddAllMonsterList();
                int num = selectedIndexNumbers[i] - 1;
                monsterList.allMonsterList[num] = monsterArray;
                monsterList.SavingAllMonsterList();
            }  
        }
    }
}
