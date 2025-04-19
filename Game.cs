public enum Treasures { StrenghtGem, IntelligenceGem, AgilityGem };
public class Game
{
    private Hero hero;
    private Map map;
    private ChallengeBST challenges;
    private Stack<Treasures> treasuresStack { get; set; }
    private Stack<int> roomHistory;
    public int currentRoomId = 0;
    Random random = new Random();

    public Game(Map map, ChallengeBST challenges)
    {
        hero = new Hero(5, 5, 5);
        this.map = map;
        this.challenges = challenges;
        treasuresStack = new Stack<Treasures>();
        roomHistory = new Stack<int>();
    }

    public void Play()
    {
        System.Console.WriteLine($"Entering Room {currentRoomId}");
        hero.PrintHeroStatus();
        Room currentRoom = map.RoomData[currentRoomId];

        //consume Health Potion
        if(hero.Health <= 10)
        {
            if(hero.Inventory.Contains(Item.HealthPotion))
            {
                var keepItems = hero.Inventory.Where(i => i == Item.HealthPotion).ToList();
                foreach(var item in keepItems)
                {
                    hero.Inventory = new Queue<Item>();
                    hero.Inventory.Enqueue(item);
                }
                hero.Health += 10;
                System.Console.WriteLine($"Used health Potion from Inventory to restore health: {hero.Health}");
            }
        }

        //treasaure
        if (currentRoom.Treasure)
        {
            System.Console.WriteLine("The current room has a treasure");
            int treasureOrItem = random.Next(2);
            if (treasureOrItem == 0)
            {
                Treasures treasureFound = (Treasures)random.Next(3);
                System.Console.WriteLine($"You have received a treasure: {treasureFound}");
                treasuresStack.Push(treasureFound);
                if (ReceiveUserChoice<string>(new List<string> { "Yes", "No" }, "Do you wish to user the treasure?") == 0)
                {
                    UseTreasure();
                }
            }
            else
            {
                Item itemFound = (Item)random.Next(7);
                System.Console.WriteLine($"You have found an Item {itemFound}");
                hero.AddItem(itemFound);
            }
        }

        //challenge
        Challenge currentRoomChallenge = challenges.FindClosestChallenge(currentRoomId);
        System.Console.WriteLine($"Challenge for this room: {currentRoomChallenge}");

        int DifferenceInStats = 0;
        bool challengePassed = false;
        if (currentRoomChallenge.StatRequired == "Strenght")
        {
            if (hero.Strenght >= currentRoomChallenge.StatRequirement || hero.Inventory.Contains(currentRoomChallenge.ItemRequirement))
                challengePassed = true;
            else
                DifferenceInStats = currentRoomChallenge.StatRequirement - hero.Strenght;
        }
        else if (currentRoomChallenge.StatRequired == "Intelligence")
        {
            if (hero.Intelligence >= currentRoomChallenge.StatRequirement || hero.Inventory.Contains(currentRoomChallenge.ItemRequirement))
                challengePassed = true;
            else
                DifferenceInStats = currentRoomChallenge.StatRequirement - hero.Intelligence;
        }
        else if (currentRoomChallenge.StatRequired == "Agility")
        {
            if (hero.Agility >= currentRoomChallenge.StatRequirement || hero.Inventory.Contains(currentRoomChallenge.ItemRequirement))
                challengePassed = true;
            else
                DifferenceInStats = currentRoomChallenge.StatRequirement - hero.Agility;
        }

        if (challengePassed)
        {
            System.Console.WriteLine("Challenge Completed");
            challenges.Delete(currentRoomChallenge.Difficulty);
        }
        else
        {
            System.Console.WriteLine($"You failed the challenge. you lost {DifferenceInStats} health");
            hero.Health -= DifferenceInStats;
            if (hero.Health < 0) hero.Health = 0;
        }

        //win condition
        if (currentRoom == map.ExitRoom && hero.Health > 0)
        {
            System.Console.WriteLine("Congratulations you completed the maze and spaced succesfully");
            System.Environment.Exit(0);
        }
        CheckLoseConditions(currentRoom);

        //navigation
        var edges = map.Graph[currentRoom];
        map.DisplayPaths(currentRoom);
        var availableRooms = edges.Where(e =>
            (hero.Strenght >= e.RequiredStrenght &&
            hero.Intelligence >= e.RequiredIntelligence &&
            hero.Agility >= e.RequiredAgility) ||
            hero.Inventory.Contains(e.RequiredItem))
            .ToList();

        if (availableRooms.Count > 0)
        {

            int nextRoomId = availableRooms[ReceiveUserChoice(availableRooms.Select(e => $"Room Id: {e.To.Id}").ToList(), "Choose your next room")].To.Id;

            roomHistory.Push(currentRoomId);
            currentRoomId = nextRoomId;
        }
        else
        {
            if(roomHistory.Count > 0)
            {
                int previousRoomId = roomHistory.Pop();
                System.Console.WriteLine($"There are no available. Going back to previous room: {previousRoomId}");
                currentRoomId = previousRoomId;
            }
            else
            {
                System.Console.WriteLine("You cannot go back. Game over");
                System.Environment.Exit(0);
            }

        }


    }

    public void UseTreasure()
    {
        if (treasuresStack.Count > 0)
        {
            Treasures treasure = treasuresStack.Pop();
            if (treasure == Treasures.StrenghtGem)
            {
                hero.Strenght += 10;
                System.Console.WriteLine("Used the Strenght Gem, you gain +10 Strenght");
            }
            if (treasure == Treasures.IntelligenceGem)
            {
                hero.Intelligence += 10;
                System.Console.WriteLine("Used the Intelligence Gem, you gain +10 Intelligence");
            }
            if (treasure == Treasures.AgilityGem)
            {
                hero.Agility += 10;
                System.Console.WriteLine("Used the Agility Gem, you gain +10 Agility");
            }
        }
        else
        {
            System.Console.WriteLine("You have no treasures available");
        }
    }

    public int ReceiveUserChoice<T>(List<T> options, string prompt)
    {
        int lines = 1 + options.Count + 1;
        int startLine;
        while (true)
        {
            startLine = Console.CursorTop;
            System.Console.WriteLine(prompt);
            for (int i = 0; i < options.Count; i++)
                System.Console.WriteLine($"[{i}]. {options[i]}");

            System.Console.Write("> ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int index) && index >= 0 && index < options.Count)
            {
                return index;
            }

            for (int i = 0; i < lines; i++)
            {
                Console.SetCursorPosition(0, startLine + i);
                System.Console.Write(new string(' ', Console.BufferWidth));
            }
            Console.SetCursorPosition(0, startLine);
        }
    }
    public void CheckLoseConditions(Room currentRoom)
    {
        bool lost = false;
        if (hero.Health <= 0)
        {
            System.Console.WriteLine("You have lost all you health");
            lost = true;
        }
        if (challenges.Root == null)
        {
            System.Console.WriteLine("You have no more challenges left");
            lost = true;
        }

        if (lost)
        {
            var path = map.FindShortestPathFromNode(currentRoom, map.ExitRoom);
            if (path == null)
            {
                System.Console.WriteLine("There is no path to exit from node");
            }
            else
            {
                System.Console.WriteLine("Path from currentRoom to the end");
                System.Console.WriteLine(string.Join("-> ", path.Select(r => r.Id).ToList()));
            }
            System.Environment.Exit(0);
        }
    }
}