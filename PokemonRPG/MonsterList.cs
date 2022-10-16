using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonRPG
{
    //このクラスではモンスター情報を外部テキストファイルから生成してオブジェクトをリスト化。
    //瀕死のポケモンは別ファイルに保存され、利用できなくなる。
    public class MonsterList
    {
        public List<string[]> allMonsterList = new List<string[]>();
        public List<int> deadMonsterNoList = new List<int>();
        //絶対パス
        //string path = @"/Users/minato/Projects/PokemonRPG/PokemonRPG/PokemonData.txt";
        //相対パス ビルド後の実行ファイルはソースファイルと場所が異なるため、パスを一部置換
        static string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string path = dir.Replace("bin/Debug/net6.0", "PokemonData.txt");



        //モンスターの情報を読み取り、allMonsterListへ登録するメソッド
        public void AddAllMonsterList()
        {
            Console.WriteLine(path);
            //最新のモンスターデータをテキストファイルから１行ずつ読み込み、[,]で区切り配列化、それをさらにフィールドリストのallMonsterListに追加
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.EndOfStream == false)
                {
                    var line = sr.ReadLine();
                    string[] monsterStatus = line.Split(",");
                    allMonsterList.Add(monsterStatus);
                }
            }
        }


        //フィールドにあるallMonsterListをテキストファイルに上書き保存。
        public void SavingAllMonsterList()
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                for (int i = 0; i < allMonsterList.Count; i++)
                {
                    string[] monster = allMonsterList[i];
                    sw.WriteLine($"{monster[0]},{monster[1]},{monster[2]},{monster[3]},{monster[4]},{monster[5]},{monster[6]},{monster[7]},{monster[8]},{monster[9]},{monster[10]},{monster[11]}");
                }
            }
        }


        //リストのモンスターの情報を読み取り、コンソールへ表示するメソッド
        public void ShowAllMonsterList()
        {
            Console.WriteLine("--------------------------------------------------------------------------------");
            //最新のモンスターデータをallMonsterListからコンソール出力
            int counter = 1;
            foreach (string[] monster in allMonsterList)
            {
                Console.WriteLine($"\nNo.{counter,3}");//モンスターのインデックス番号
                Console.Write($"{monster[0],-10}　レベル:{monster[1],2}　属性:{monster[2]}　体力:{monster[4],3}/{monster[3],3}　攻撃力:{monster[5],2}　防御力:{monster[6],2}　素早さ:{monster[7],2}　必殺技:{monster[8]}");
                //瀕死のモンスターの場合下記一文を追加し瀕死のモンスターリストに追加
                if (monster[10] == "False")
                {
                    Console.WriteLine($" ※ 瀕死");
                    deadMonsterNoList.Add(counter);
                }
                //既にプレイヤー１orプレイヤー２の仲間になっている場合下記一文を追加
                else if (monster[11] == "false")
                {
                    Console.WriteLine($" ※ {monster[0]}は既に自分or相手の仲間になっています。");
                }
                else
                {
                    Console.WriteLine("");
                }
                counter++;
            }
            Console.WriteLine("\n\t\t\t\t\t※ 相手モンスターより有利な属性だと攻撃力が２倍になります。　火＜水, 水＜草, 草＜火");
            Console.WriteLine("\t\t\t\t\t※ 必殺技は１ターンの溜めを必要とし、通常攻撃力の1~3倍の攻撃力になります。");
            Console.WriteLine("\t\t\t\t\t※ 攻撃は素早さの数値が高い方から先に行われます。");
            Console.WriteLine("\t\t\t\t\t※ 瀕死のモンスターは仲間にできません。\n");
            Console.WriteLine("--------------------------------------------------------------------------------");
        }
        //表示されたモンスター一覧から番号を選択して、モンスター情報を取得しpersonへMonster型を返すメソッド
        public Monster GetMonster(int monsterNo)
        {
            string[] monster = allMonsterList[monsterNo - 1];

            string name = monster[0];
            int level = int.Parse(monster[1]);
            string montype = monster[2];
            int maxhp = int.Parse(monster[3]);
            int nowhp = int.Parse(monster[4]);
            int attack = int.Parse(monster[5]);
            int deffence = int.Parse(monster[6]);
            int speed = int.Parse(monster[7]);
            string deathblow = monster[8];
            bool waitturn = bool.Parse(monster[9]);
            bool lifeanddeath = bool.Parse(monster[10]);
            bool fellowflag = bool.Parse(monster[11]);

            Monster monsterdata = new Monster(name, level, montype, maxhp, nowhp, attack, deffence, speed, deathblow, waitturn, lifeanddeath, fellowflag);
            return monsterdata;
        }

        //選択したモンスターのallMonsterListのFellowFlagをfalseにする処理
        public void ChangeFellowMonsterFlag(int selectNo)
        {
            string[] monster = allMonsterList[selectNo - 1];
            monster[11] = "false";
            allMonsterList[selectNo - 1] = monster;
        }

        //ポケモンセンターメソッド
        //NowHpの全回復、LifeAndDeathをtrueに
        public void PokemonCenter()
        {
            AddAllMonsterList();
            for (int i = 0; i < allMonsterList.Count; i++)
            {
                string[] monster = allMonsterList[i];
                monster[4] = monster[3];
                monster[10] = "True";
                monster[11] = "True";

            }
            SavingAllMonsterList();
            Console.WriteLine("\n--------------------------------------------------------------------------------");
            Console.WriteLine("\n♪ ♪ チャンッ ♪ ♪");
            Console.WriteLine("\t♪ ♪ チャンッ ♪ ♪");
            Console.WriteLine("\t\t♪ ♪ チャララーーンッ ♪ ♪");
            Console.WriteLine("\nポケモンたちは全回復しました。\n");
            Console.WriteLine("--------------------------------------------------------------------------------");
        }

        //モンスター追加メソッド
        //現在のモンスター一覧を表示　→　モンスター情報入力　→　string[]に変換してallMonsterListリスト追加　→　書き込み
        public void CreateMonster()
        {
            Console.WriteLine("\nこちらは現在登録されているモンスター一覧です。");
            AddAllMonsterList();
            ShowAllMonsterList();

            int point = 100;
            string[] monsterArray = new string[12];
            string[] fieldTitle = new string[]
            {
                "モンスター名",
                "レベル",
                "属性",
                "体力",
                "現在の体力",
                "攻撃力",
                "防御力",
                "すばやさ",
                "必殺技名",
                "必殺技の待機ターン数",
                "モンスターの生死状態",
                "モンスターのパーティーステータス"
            };
            for (int i = 0; i < 12; i++)
            {
                //string
                if (i == 0 || i == 8)
                {
                    Console.Write($"\n追加をする{fieldTitle[i]}を入力して下さい。\n＞＞");
                    string? stri = Console.ReadLine();
                    if(stri == "")
                    {
                        monsterArray[i] = "名無し";
                    }
                    else
                    {
                        monsterArray[i] = stri;
                    }
                    Console.WriteLine("--------------------------------------------------------------------------------");
                }
                //int
                else if(i == 3 || i == 5 || i == 6 || i == 7)
                {
                    Console.WriteLine("\n合計100ポイントを「体力」「攻撃力」「防御力」「すばやさ」に振り分けます。\n推奨　体力:30以上　攻撃力:20~30　防御力:20以下　すばやさ:5以下");
                    while (true)
                    {
                        Console.Write($"{fieldTitle[i]}には何ポイント振り分けますか？　残りポイント:{point}\n＞＞");
                        if (int.TryParse(Console.ReadLine(), out int num) && 0 <= num && num <= point)
                        {
                            point -= num;
                            Console.WriteLine($"{fieldTitle[i]}に{num}ポイント振り分けます。　残りポイント:{point}");
                            monsterArray[i] = num.ToString();
                            Console.WriteLine("--------------------------------------------------------------------------------");
                            break;
                        }
                        else if(point == 0)
                        {
                            Console.WriteLine("振り分けられるポイントがありませんでした。");
                            monsterArray[i] = "0";
                            break;
                        }
                        else
                        {
                            Console.WriteLine("半角数字、現在のポイント残高以下で入力して下さい");
                        }
                    }
                }
                //属性
                else if(i == 2)
                {
                    while (true)
                    {
                        Console.WriteLine("\n属性を追加します。");
                        Console.Write("「火」, 「水」, 「草」のうちどれか一文字を入力して下さい。　火＜水, 水＜草, 草＜火\n＞＞");
                        string? type = Console.ReadLine();
                        if(type == "火" || type == "水" || type == "草")
                        {
                            Console.WriteLine("--------------------------------------------------------------------------------");
                            monsterArray[2] = type;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("漢字一文字で入力して下さい。");
                        }
                    }
                }
            }
            monsterArray[1] = "1";
            monsterArray[4] = monsterArray[3];
            monsterArray[9] = "False";
            monsterArray[10] = "True";
            monsterArray[11] = "True";
            Console.WriteLine("新モンスターの登録を完了しました。");
            Console.Write($"{monsterArray[0],-10}　レベル:{monsterArray[1],2}　属性:{monsterArray[2]}　体力:{monsterArray[4],3}/{monsterArray[3],3}　攻撃力:{monsterArray[5],2}　防御力:{monsterArray[6],2}　素早さ:{monsterArray[7],2}　必殺技:{monsterArray[8]}\n\n\n\n\n");

            allMonsterList.Add(monsterArray);
            SavingAllMonsterList();
        }
    }
}
