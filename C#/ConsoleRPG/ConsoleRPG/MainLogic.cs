using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class MainLogic
    {
        // 참조 형태
        Player player;
        Monster monster;

        public void Run()
        {
            Console.WriteLine("게임 시작");

            // 참조형태 = 실형태
            player = new Player();
            monster = new Monster();

            player.Move();
            monster.Move();

            List<Character> characters = new List<Character>();
            characters.Add(monster);
            characters.Add(player);
            // characters[0].Print();
            // Item item = new Item();

            // is as
            if (player is Character)
            {
                Character c =  (Character)player;
                c = monster as Character;
            }

        }
    }
}
