using System.Text;
public class Map
{
    public class Edge
    {
        public Room To { get; set; }
        public int RequiredStrenght { get; }
        public int RequiredAgility { get; }
        public int RequiredIntelligence { get; }
        public Item RequiredItem { get; }

        public Edge(Room to, int requiredStrenght, int requiredAgility, int requiredIntelligence, Item requiredItem)
        {
            To = to;
            RequiredStrenght = requiredStrenght;
            RequiredAgility = requiredAgility;
            RequiredIntelligence = requiredIntelligence;
            RequiredItem = requiredItem;
        }

    }
    public record Room(int Id, bool Treasure = false);

    public Dictionary<Room, List<Edge>> Graph { get; set; }
    public Dictionary<int, Room> RoomData { get; set; }
    public Room ExitRoom { get; set; }

    public Map()
    {
        Graph = new Dictionary<Room, List<Edge>>();
        RoomData = new Dictionary<int, Room>();
        GenerateRandomMap(15);

    }

    public void AddRoom(Room room)
    {
        if (Graph.ContainsKey(room))
        {
            System.Console.WriteLine($"{room} already exists");
            return;
        }

        Graph[room] = new List<Edge>();
        RoomData[room.Id] = room;
        System.Console.WriteLine($"{room} has been added");
    }

    public void RemoveRoom(Room room)
    {
        if (!Graph.ContainsKey(room))
        {
            System.Console.WriteLine($"{room} does not exist");
            return;
        }

        Graph.Remove(room);
        RoomData.Remove(room.Id);
        foreach (var roomInGraph in Graph.Keys)
        {
            Graph[roomInGraph].RemoveAll(e => e.To == room);
        }
        System.Console.WriteLine($"{room} removed from graph");
    }

    public void AddPath(Room from, Edge to)
    {
        if (!Graph.ContainsKey(from) || !Graph.ContainsKey(to.To))
        {
            System.Console.WriteLine($"One or both rooms do not exist");
            return;
        }
        if (Graph[from].FirstOrDefault(e => e.To == to.To) is not null)
        {
            System.Console.WriteLine($"There is already a path between {from} and {to.To}");
            return;
        }
        Graph[from].Add(to);
        System.Console.WriteLine($"Path added from {from} to {to.To}");
    }

    public void RemovePath(Room from, Room to)
    {
        if (!Graph.ContainsKey(from) || !Graph.ContainsKey(to))
        {
            System.Console.WriteLine($"One or both rooms do not exist");
            return;
        }
        if (Graph[from].FirstOrDefault(e => e.To == to) is null)
        {
            System.Console.WriteLine($"There is no path between {from} and {to}");
            return;
        }

        Graph[from].RemoveAt(Graph[from].FindIndex(e => e.To == to));
        System.Console.WriteLine($"Path removed from {from} to {to}");
    }

    public void DisplayPaths(Room room)
    {
        if (!Graph.ContainsKey(room))
        {
            System.Console.WriteLine($"{room} does not exist.");
            return;
        }
        if (Graph[room].Count == 0)
        {
            System.Console.WriteLine($"{room} has no paths.");
            return;
        }

        List<string> paths = new List<string>();
        foreach (var edge in Graph[room])
        {
            paths.Add($"To: {edge.To}, strenght: {edge.RequiredStrenght}, agility: {edge.RequiredAgility}, intelligence: {edge.RequiredIntelligence}, item: {edge.RequiredItem}");
        }
        System.Console.WriteLine($"PATHS FROM ROOM {room}");
        System.Console.WriteLine($"{string.Join("\n", paths)}");
        System.Console.WriteLine();
    }

    public void displayWholeMap()
    {
        foreach (var room in RoomData.Values)
        {
            DisplayPaths(room);
        }
        int edgeCount = 0;
        foreach (var edgeList in Graph.Values)
        {
            edgeCount += edgeList.Count;
        }
        System.Console.WriteLine($"Start: {RoomData[0]}");
        System.Console.WriteLine($"Finish: {ExitRoom}");
        System.Console.WriteLine($"Number Of Edges: {edgeCount}");

    }

    public void GenerateRandomMap(int mapSize)
    {
        Random random = new Random();
        List<int> pathOptions = new List<int>();

        //generate all rooms
        for (int i = 0; i < mapSize; i++)
        {
            AddRoom(new Room(i, random.Next(1, 11) == 1 ? true : false)); //10% of having treasure
            pathOptions.Add(i);
        }

        //generate a random path from 0 to a random room and that is the path to the exit
        List<int> path = new List<int>();
        int pathDistance = random.Next(10, 13);

        path.Add(0);
        pathOptions.Remove(0);

        for (int i = 0; i < pathDistance; i++)
        {
            int nextRoom = random.Next(1, pathOptions.Count);
            path.Add(pathOptions[nextRoom]);
            pathOptions.Remove(pathOptions[nextRoom]);
        }

        ExitRoom = RoomData[path.Last()];
        for (int i = 1; i < path.Count; i++)
        {
            AddPath(RoomData[path[i - 1]], new Edge(RoomData[path[i]], 0, 0, 0, Item.None));
        }
        System.Console.WriteLine();

        //generate extra rooms
        foreach (var room in RoomData.Values)
        {
            int randomNumberOfEdges = random.Next(1, 3);
            for (int i = 0; i < randomNumberOfEdges; i++)
            {
                List<int> roomsAvailableToAdd = RoomData.Keys.Where(id => id != room.Id && ExitRoom.Id != id && Graph[room].All(e => e.To.Id != id)).ToList();
                int randomRoom = random.Next(1, roomsAvailableToAdd.Count);
                AddPath(room, new Edge(RoomData[roomsAvailableToAdd[randomRoom]], 0, 0, 0, Item.None));
                roomsAvailableToAdd.Remove(roomsAvailableToAdd[randomRoom]);
            }
        }
    }
}
