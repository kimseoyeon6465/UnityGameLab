using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{

    class Character
    {
        // Status ( HP, MP, Atk, Def ...) 능력치
        // 공격 기능, 이동 기능, 방어 기능, 

        public virtual void Move()
        {
            Console.WriteLine("Character Move() : 두발 걷기");
        }

        public void Print()
        {
            Console.WriteLine("Character");
        }
    }


    class Player : Character
    {
        // 클래스 -> 저장공간 과 기능 집합
    }

    class Monster : Character
    {
        public override void Move()
        {
            Console.WriteLine("Monster Move() : 네발 걷기");
        }
    }
}
