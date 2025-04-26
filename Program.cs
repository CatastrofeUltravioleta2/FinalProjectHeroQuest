public class Program
{
    static void Main()
    {
        Map map = new Map();
        ChallengeBST challenges = new ChallengeBST();

        Game game = new Game(map, challenges);
        //map.displayWholeMap();
        System.Console.WriteLine("Do you wish to see the path to the exit? (press 1 for yes)");
        string? userInput = Console.ReadLine();
        if (userInput == "1")
        {
            System.Console.WriteLine("ShortestPath");
            foreach (var i in map.FindShortestPathFromNode(map.RoomData[0], map.ExitRoom))
            {
                System.Console.WriteLine(i);
            }
        }
        System.Console.WriteLine();
        while (true)
        {
            game.Play();
        }
    }
}

