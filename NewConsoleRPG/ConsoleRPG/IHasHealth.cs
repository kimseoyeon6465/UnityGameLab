using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    public interface IHasHealth//펫, 방어벽, 포탑 같은 건 '살았는지/죽었는 지' 만 봄, attack, defense를 가질 필요 없음. 이럴때 IHasHealth만 상속하면 됨.
    {
        int HP { get; }
        int MaxHP {  get; }

        void Heal(int amount);//HP 회복용
    }
}
