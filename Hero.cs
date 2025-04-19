public class Hero
{
    public int Strenght {get;set;}
    public int Agility {get;set;}
    public int Intelligence {get;set;}
    public int Health {get;set;}
    public Queue<Item> Inventory {get;set;} 
    public int InventoryMaxSize {get; set;}

    public Hero(int strenght, int agility, int intelligence)
    {
        Strenght = strenght;
        Agility = agility;
        Intelligence = intelligence;
        Health = 20;

        Inventory = new Queue<Item>();
        Inventory.Enqueue(Item.HealthPotion);
        Inventory.Enqueue(Item.Sword);

        InventoryMaxSize = 5;
    }

    public void AddItem(Item item)
    {
        if(Inventory.Count >= InventoryMaxSize)
        {
            Item discardedItem = Inventory.Dequeue();
            System.Console.WriteLine($"Inventory Full, Remove oldest item: {discardedItem}");
        }
        Inventory.Enqueue(item);
        System.Console.WriteLine($"{item} added to the inventory");
    }

    public void PrintHeroStatus()
    {
        System.Console.WriteLine($"Hero has: Health {Health} / Strenght {Strenght} / Agility {Agility} / Intelligence {Intelligence}");
        System.Console.WriteLine($"Inventory: {string.Join("; ", Inventory)}");
    }

}

public enum Item {Sword, Lockpick, WisdomBook, Teleport, SpeedBoots, Shield, HealthPotion, None}