public class Program
{
    static void Main()
    {
        Map map = new Map();
        ChallengeBST challenges = new ChallengeBST();
        challenges.DisplayTree();

        Game game = new Game(map, challenges);
        map.displayWholeMap();
        System.Console.WriteLine();
        while(true)
        {
            game.Play();
        }
    }
}

