using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    public class Item
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public Item(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
    public interface IUsable
    {
        void UseOn(Object target);
    }

    public interface IEquipable
    {
        EquipSlot Slot { get; }
        int AttackMod { get; }
        int DefenseMod { get; }
    }

    public enum EquipSlot
    {
        Weapon,
        Armor
    }

    public class Weapon : Item, IEquipable
    {
        public int AttackMod { get; private set; }
        public int DefenseMod => 0;
        public EquipSlot Slot => EquipSlot.Weapon;
        public int UpgradeLevel { get; private set; }
        public Weapon(string name, int attackMod) : base(name, "무기")

        {
            AttackMod = attackMod;
            UpgradeLevel = 0;
        }
        public void Upgrade()
        {
            UpgradeLevel++;
            AttackMod++;
        }
    }

    public class Armor : Item, IEquipable
    {
        public int AttackMod => 0;
        public int DefenseMod { get; private set; }
        public EquipSlot Slot => EquipSlot.Armor;
        public int UpgradeLevel { get; private set; }

        public Armor(string name, int defenseMod) : base(name, "방어구")
        {
            DefenseMod = defenseMod;
            UpgradeLevel = 0;
        }
        public void Upgrade()
        {
            UpgradeLevel++;
            DefenseMod++;
        }
    }
}

