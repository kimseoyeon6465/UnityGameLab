namespace ConsoleRPG
{
    internal class Potion : Item, IUsable
    {
        private int healAmount;

        public Potion(string name, int healAmount) : base(name, "회복아이템")
        {
            this.healAmount = healAmount;
        }
        public void UseOn(Object target)
        {
            if(target is IHasHealth h)
            {
                h.Heal(healAmount);
                Console.WriteLine($"{Name}을 사용했습니다. HP가 {healAmount} 만큼 회복됩니다.");
            }
            else
            {
                Console.WriteLine("이 대상에게는 사용할 수 없습니다.");
            }
        }
    }
}