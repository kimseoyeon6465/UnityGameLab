using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    public class Player : Character, IHasHealth
    {


        public Inventory Inventory { get; private set; }
        public EquipmentSet Equipment { get; private set; }
        public int Level { get; private set; }
        public int Exp { get; private set; }
        public int ExpToNext { get; private set; }
        public Player(string name) : base(name, 100, 10, 5)
        {
            Inventory = new Inventory();
            Equipment = new EquipmentSet();
            Inventory.Add(new Potion("Small Potion", 10));

            Level = 1;
            Exp = 0;
            ExpToNext = 20;
        }

        public void Heal(int amount)
        {
            HP += amount;
            if (HP > MaxHP) HP = MaxHP;
            Console.WriteLine($"{Name}의 HP가 {amount} 회복되어 {HP}/{MaxHP}가 되었다~!");
        }

        public int GetAttack()
        {
            return Attack + Equipment.GetTotalAttackMod();
        }

        public int GetDefense()
        {
            return Defense + Equipment.GetTotalDefenseMod();
        }

        public void UpdateStats()
        {

        }
        public override void ShowStatus()
        {
            Console.WriteLine($"플레이어: {Name}");
            Console.WriteLine($"HP {HP}/{MaxHP}");
            Console.WriteLine($"공격력: {GetAttack()} (기본 {Attack})");
            Console.WriteLine($"방어력: {GetDefense()} (기본 {Defense})");
            Console.WriteLine($"무기: {(Equipment.Weapon != null ? ((Item)Equipment.Weapon).Name : "없음")}");
            Console.WriteLine($"방어구: {(Equipment.Armor != null ? ((Item)Equipment.Armor).Name : "없음")}");
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0) HP = 0;
            Console.WriteLine($"{Name}가 {damage}만큼 피해를 입었습니다! (HP {HP}/{MaxHP}");
        }

        public void RecoverAfterDefeat()
        {
            HP = MaxHP / 2;
            Console.WriteLine($"패배 패널티로 HP가 절반 ({HP}/{MaxHP}으로 회복되었습니다.");
        }

        public void GainExp(int amount)
        {
            Exp += amount;
            Console.WriteLine($"\n▶ {amount} EXP를 획득했습니다! (현재 {Exp}/{ExpToNext})");

            while(Exp>=ExpToNext)
            {
                Exp -= ExpToNext;
                LevelUp();
            }
        }

        private void LevelUp()
        {
            Level++;
            MaxHP += 5;
            Attack += 2;
            Defense += 1;
            HP = MaxHP;

            ExpToNext = (int)(ExpToNext * 1.5);

            Console.WriteLine($"\n WOW 레벨업! Lv{Level} 달성!");
            Console.WriteLine("HP가 완전히 회복되고 스탯이 상승했습니다~~");
            Console.WriteLine($"새로운 능력치 -> HP {MaxHP}, ATK {Attack}, DEF {Defense}");
        }
    }
    public class EquipmentSet
    {
        public IEquipable Weapon { get; private set; }
        public IEquipable Armor { get; private set; }

        public void Equip(Player player, IEquipable item)
        {
            switch (item.Slot)
            {
                case EquipSlot.Weapon:
                    Weapon = item;
                    break;
                case EquipSlot.Armor:
                    Armor = item;
                    break;
            }

            Console.WriteLine($"{item.Slot} [{((Item)item).Name}]을 장착했습니다.");
            player.UpdateStats();
        }
        public int GetTotalAttackMod()
        {
            int w = Weapon?.AttackMod ?? 0;
            int a = Armor?.AttackMod ?? 0;
            return w + a;
        }

        public int GetTotalDefenseMod()
        {
            int w = Weapon?.DefenseMod ?? 0;
            int a = Weapon?.DefenseMod ?? 0;
            return w + a;
        }


    }
}
