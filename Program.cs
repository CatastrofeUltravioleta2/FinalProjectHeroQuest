public class Program
{
    static void Main()
    {
        Map map = new Map();
        ChallengeBST challenges = new ChallengeBST();

        Game game = new Game(map, challenges);
        map.displayWholeMap();
        System.Console.WriteLine("ShortestPath");
        foreach(var i in map.FindShortestPathFromNode(map.RoomData[0], map.ExitRoom))
        {
            System.Console.WriteLine(i);
        }
        System.Console.WriteLine();
        while(true)
        {
            game.Play();
        }
    }
}

