using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{//Initial commit for textRPG
    public class MainLogic//입출력 담당
    {
        private Player player;

        public void Start()
        {

            Console.Write("플레이어 이름 입력: ");
            string name = Console.ReadLine();

            player = new Player(name); //
            //Monster monster = new Monster("적");
            player.Inventory.Add(new Weapon("Beginner Sword", 1));
            player.Inventory.Add(new Armor("Cloth Armor", 1));

            GameLoop();//내부에서 직접 호출
        }


        public void GameLoop()
        {

            while (true)
            {
                Console.WriteLine("1)상태보기 2)던전입장 3)인벤토리 4)아이템 강화 5)종료");
                Console.Write("선택: ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        player.ShowStatus();
                        break;
                    case "2":
                        EnterDungeon();
                        break;
                    case "3":
                        ShowInventory();
                        break;
                    case "4":
                        UpgradeItemMenu();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;

                }

            }

        }
        private void ShowInventory()
        {
            Console.WriteLine("\n----인벤토리----");
            player.Inventory.ShowAll();

            Console.Write("장착할 아이템 번호 선택 (0 = 취소): ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= player.Inventory.GetItems().Count)
            {//1. 사용자 입력 string으로 받아서 int로 변환(TryParse 함수)
             //2. 해당 번호가 올바른 범위안에 위치하는 지 검사
             //player.Inventory.GetItems().Count는 유효한 아이템 갯수
                var item = player.Inventory.GetItems()[choice - 1];
                if (item is IUsable usable)
                {
                    usable.UseOn(player);
                    player.Inventory.Remove(item);
                }
                else if (item is IEquipable equipable)
                {
                    player.Equipment.Equip(player, equipable);
                }
                else
                {
                    Console.WriteLine("이 아이템은 사용할 수 없습니다.");
                }
            }
            else
            {
                Console.WriteLine("취소됨.");
            }



        }
        private void EnterDungeon()
        {
            Console.WriteLine("\n=== 던전 입장 ===");
            Console.WriteLine("난이도 선택 (1~5):");

            if (int.TryParse(Console.ReadLine(), out int level) && level >= 1 && level <= 5)
            {


                Monster monster = new Monster("Goblin", level);

                Console.WriteLine($"\n{monster.Name} 등장!");
                monster.ShowStatus();
                Combat.Fight(player, monster);
                Console.WriteLine("\n던전 끝");
            }
            else
            {
                Console.WriteLine("잘못된 난이도");
            }
        }
        private void UpgradeItemMenu()
        {
            Console.WriteLine("\n===아이템 강화===");
            player.Inventory.ShowAll();

            Console.Write("강화할 아이템 번호 선택 (0= 취소) ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= player.Inventory.GetItems().Count)
            {
                var item = player.Inventory.GetItems()[choice - 1];//var은 컴파일러가 추론해서 형식 지정
                if (item is IEquipable equipable)
                {
                    Random rand = new Random();
                    int chance = rand.Next(0, 100);

                    if (chance < 70)
                    {
                        if (item is Weapon weapon)
                        {
                            weapon.Upgrade();
                            Console.WriteLine($" {weapon.Name} 강화 성공! +{weapon.UpgradeLevel} (공격력 {weapon.AttackMod})");
                        }
                        else if (item is Armor armor)
                        {
                            armor.Upgrade();
                            Console.WriteLine($"  {armor.Name} 강화 성공! +{armor.UpgradeLevel} (방어력 {armor.DefenseMod})");

                        }
                    }
                    else
                    {
                        Console.WriteLine($"강화 실패...! {item.Name}이 부서졌습니다...");
                        player.Inventory.Remove(item);
                    }
                }
                else
                {
                    Console.WriteLine("이 아이템은 강화 불가능합니다!");
                }
            }
            else
            {
                Console.WriteLine("취소됨");
            }
        }
    }
}
