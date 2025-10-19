using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    public static class Combat
    {
        public static void Fight(Player player, Monster enemy)
        {
            Console.WriteLine($"\n===전투 시작===");
            Console.WriteLine($"{player.Name} vs {enemy.Name}");
            Console.WriteLine();

            while (player.HP > 0 && enemy.HP > 0)
            {
                int playerDamage = Math.Max(1, player.GetAttack() - enemy.Defense);//공격력 = 방어력이어도 최소 피해 1은 보장
                enemy.TakeDamage(playerDamage);
                Console.WriteLine($"{player.Name}이 {enemy.Name}을 공격!({playerDamage}만큼 피해)");

                if (enemy.HP <= 0)
                {
                    Console.WriteLine($"{enemy.Name} 처치!");
                    Console.WriteLine($"{player.Name} 승리~");
                    player.GainExp(enemy.ExpReward);

                    return;
                }

                Thread.Sleep(500);//자연스럽게 지연

                int enemyDamage = Math.Max(1, enemy.Attack - player.GetDefense());
                player.TakeDamage(enemyDamage);
                Console.WriteLine($"{enemy.Name}이 반격! ({enemyDamage}만큼 피해)");

                if (player.HP <= 0)
                {
                    Console.WriteLine($"{player.Name}는 쓰러졌다ㅠㅠ");
                    Console.WriteLine($"{enemy.Name}의 승리!ㅡㅡ");

                    player.RecoverAfterDefeat();//HP는 protected setter라서 직접 접근 불가, 메서드 이용
                    Console.WriteLine($"패배 패널티로 HP가 절반으로 회복됨.({player.HP}/{player.MaxHP}");
                    return;

                }

                Thread.Sleep(500);
            }
        }
    }
}
