using System;
using System.Collections.Generic;

namespace HW3
{
    // Nhân viên thu ngân
    public class Employee
    {
        public string Name { get; set; }

        public Employee(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"Clerk: {Name}";
        }
    }

    // Mặt hàng
    public class Item
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; } // số tiền giảm

        public Item(string name, double price, double discount = 0.0)
        {
            Name = name;
            Price = price;
            Discount = discount;
        }

        public double GetPrice()
        {
            return Price;
        }

        public double GetDiscount()
        {
            return Discount;
        }

        public override string ToString()
        {
            return $"{Name} - Price: {Price:C}, Discount: {Discount:C}";
        }
    }

    // Dòng hóa đơn (BillLine - phần mở rộng)
    public class BillLine
    {
        public Item Item { get; private set; }
        public int Quantity { get; private set; }

        public BillLine(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public void SetQuantity(int q)
        {
            Quantity = q;
        }

        public int GetQuantity()
        {
            return Quantity;
        }

        public void SetItem(Item i)
        {
            Item = i;
        }

        public Item GetItem()
        {
            return Item;
        }

        public double GetLineTotal(bool preferred)
        {
            if (preferred)
                return (Item.Price - Item.Discount) * Quantity;
            else
                return Item.Price * Quantity;
        }

        public double GetLineDiscount()
        {
            return Item.Discount * Quantity;
        }

        public override string ToString()
        {
            return $"{Item.Name} x{Quantity}  =>  {(Item.Price * Quantity):C} (Discount: {(Item.Discount * Quantity):C})";
        }
    }

    // Hóa đơn
    public class GroceryBill
    {
        protected Employee Clerk;
        protected List<BillLine> BillLines;

        public GroceryBill(Employee clerk)
        {
            Clerk = clerk;
            BillLines = new List<BillLine>();
        }

        public virtual void Add(BillLine line)
        {
            BillLines.Add(line);
        }

        public virtual double GetTotal()
        {
            double total = 0.0;
            foreach (var line in BillLines)
            {
                total += line.Item.Price * line.Quantity; // tính giá gốc
            }
            return total;
        }

        public virtual void PrintReceipt()
        {
            Console.WriteLine("=== Receipt ===");
            Console.WriteLine(Clerk.ToString());
            foreach (var line in BillLines)
            {
                Console.WriteLine(line.ToString());
            }
            Console.WriteLine($"Total: {GetTotal():C}");
        }
    }

    // Hóa đơn có giảm giá
    public class DiscountBill : GroceryBill
    {
        private bool Preferred; // khách hàng thân thiết?

        public DiscountBill(Employee clerk, bool preferred) : base(clerk)
        {
            Preferred = preferred;
        }

        public override double GetTotal()
        {
            double total = 0.0;
            foreach (var line in BillLines)
            {
                if (Preferred)
                    total += (line.Item.Price - line.Item.Discount) * line.Quantity;
                else
                    total += line.Item.Price * line.Quantity;
            }
            return total;
        }

        public int GetDiscountCount()
        {
            if (!Preferred) return 0;
            int count = 0;
            foreach (var line in BillLines)
            {
                if (line.Item.Discount > 0)
                    count += line.Quantity;
            }
            return count;
        }

        public double GetDiscountAmount()
        {
            if (!Preferred) return 0.0;
            double sum = 0.0;
            foreach (var line in BillLines)
            {
                sum += line.GetLineDiscount();
            }
            return sum;
        }

        public double GetDiscountPercent()
        {
            if (!Preferred) return 0.0;
            double original = base.GetTotal();
            if (original == 0) return 0.0;
            return (GetDiscountAmount() / original) * 100.0;
        }

        public override void PrintReceipt()
        {
            Console.WriteLine("=== Discount Receipt ===");
            Console.WriteLine(Clerk.ToString());
            foreach (var line in BillLines)
            {
                Console.WriteLine(line.ToString());
            }
            Console.WriteLine($"Original Total: {base.GetTotal():C}");
            Console.WriteLine($"Discount: {GetDiscountAmount():C}");
            Console.WriteLine($"Total after discount: {GetTotal():C}");
            Console.WriteLine($"Discount items: {GetDiscountCount()}");
            Console.WriteLine($"Discount percent: {GetDiscountPercent():F2}%");
        }
    }

    // Demo
    public class Program
    {
        public static void Main(string[] args)
        {
            Employee emp = new Employee("Alice");

            Item candy = new Item("Candy Bar", 1.35, 0.25);
            Item milk = new Item("Milk", 2.00, 0.0);
            Item chips = new Item("Potato Chips", 3.50, 0.50);

            // Tạo hóa đơn thường
            GroceryBill bill = new GroceryBill(emp);
            bill.Add(new BillLine(candy, 2));
            bill.Add(new BillLine(milk, 1));
            bill.Add(new BillLine(chips, 3));
            bill.PrintReceipt();

            Console.WriteLine();

            // Tạo hóa đơn có giảm giá cho khách thân thiết
            DiscountBill dbill = new DiscountBill(emp, true);
            dbill.Add(new BillLine(candy, 2));
            dbill.Add(new BillLine(milk, 1));
            dbill.Add(new BillLine(chips, 3));
            dbill.PrintReceipt();
        }
    }
}

