using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    interface IPrintable
    {
        public void Print();
    }

    abstract class Item
    {
        public bool used;
        public abstract bool Use();
    }

    class EquipItem : Item, IPrintable
    {
        public void Print()
        {
            Console.WriteLine("EquipItme");
        }

        public override bool Use()
        {
            return true;
        }
    }

    class UseItem : Item, IPrintable
    {
        public void Print()
        {
            Console.WriteLine("EquipItme");
        }

        public override bool Use()
        {
            return true;
        }
    }
}
