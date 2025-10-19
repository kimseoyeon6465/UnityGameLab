using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    public class Monster : Character
    {

        public int ExpReward { get; private set; }// 보상 경험치
        public Monster(string name, int level) : base($"{name} Lv{level}", 30+(level*10), 5+(level*2), 2+level)
        {
            ExpReward = 10 * level;
        }
        public override void ShowStatus()
        {
            Console.WriteLine($"몬스터: {Name} | HP {HP}/{MaxHP} | ATK {Attack} | DEF {Defense}");
        }
        public void TakeDamage(int damage)
        {
            HP-=damage;
            if (HP < 0)HP = 0;
            Console.WriteLine($"{Name}이 {damage}만큼 피해를 입었다!~ (HP {HP}/{MaxHP})");
        }
    }
}
