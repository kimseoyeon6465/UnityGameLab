using System.Collections.Generic;

namespace ConsoleRPG
{
    public class Inventory//데이터 담당
    {
        private List<Item> items = new List<Item>();
        public void Add(Item item)
        {
            items.Add(item);
        }
        public void Remove(Item item)
        {
            items.Remove(item);
        }

        public IReadOnlyList<Item> GetItems()//추가/삭제 하지 않고 just 읽기만 함.
        {
            return items.AsReadOnly();
        }

        public void ShowAll()//내가 원하는 방식으로 print
        {
            if (items.Count == 0)
            {
                Console.WriteLine("인벤토리가 비었습니다.");
                return;
            }
            for (int i = 0; i < items.Count; i++)
            {
                string displayName = items[i].Name;
                if (items[i] is Weapon w && w.UpgradeLevel > 0)
                    displayName += $" +{w.UpgradeLevel}";
                else if (items[i] is Armor a && a.UpgradeLevel > 0)
                    displayName += $" +{a.UpgradeLevel}";
                Console.WriteLine($"{i + 1}.{items[i].Name}");
            }
        }
    }
}