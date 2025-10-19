using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{

    public class Character
    {
        public string Name { get; protected set; }
        public int MaxHP { get; protected set; }
        public int HP { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }

        public Character(string name, int maxHP, int attack, int defense)
        {
            Name = name;
            MaxHP = maxHP;  
            HP = maxHP;
            Attack = attack;
            Defense = defense;
                
        }

        public virtual void ShowStatus()
        {
            Console.WriteLine($"{Name} | HP {HP}/{MaxHP} | ATK {Attack} | Defense {Defense}");
        }
        
    }


   
}
