public class Challenge
{
    public string Name { get; set; }
    public int Difficulty { get; set; }
    public Item ItemRequirement { get; set; }
    public int StatRequirement { get; set; }
    public string StatRequired { get; set; }

    public Challenge(string name, int difficulty, Item itemRequirement, int statRequirement, string statRequired)
    {
        Name = name;
        Difficulty = difficulty;
        ItemRequirement = itemRequirement;
        StatRequirement = statRequirement;
        StatRequired = statRequired;
    }

    public override string ToString()
    {
        return $"Challenge Name: {Name} / Difficulty: {Difficulty} / Requirements: {ItemRequirement} or {StatRequired} >= {StatRequirement}";
    }
}


public class ChallengeBST
{
    public class Node
    {
        public Challenge Data { get; set; }
        public Node? Left { get; set; } = null;
        public Node? Right { get; set; } = null;
        public int Height {get;set;}

        public Node(Challenge data)
        {
            Data = data;
            Height = 1;
        }

    }

    public Node? Root = null; 
    public ChallengeBST()
    {
        AddChallenges();
    }

    public void Insert(Challenge challenge)
    {
        Root = InsertLogic(Root, challenge);
        System.Console.WriteLine($"Challenge added {challenge}");
    }
    private Node InsertLogic(Node node, Challenge challenge)
    {
        if (node == null)
            return new Node(challenge);
        if (challenge.Difficulty < node.Data.Difficulty)
            node.Left = InsertLogic(node.Left, challenge);
        else
            node.Right = InsertLogic(node.Right, challenge);

        //ballancing
        UpdateHeight(node);
        return BalanceTree(node);
    }

    public void Delete(int challengeDifficulty)
    {
        Root = DeleteLogic(Root, challengeDifficulty);
    }
    public Node DeleteLogic(Node node, int challengeDifficulty)
    {
        if (node == null)
            return node;
        if (challengeDifficulty < node.Data.Difficulty)
            node.Left = DeleteLogic(node.Left, challengeDifficulty);
        else if (challengeDifficulty > node.Data.Difficulty)
            node.Right = DeleteLogic(node.Right, challengeDifficulty);
        else
        {
            //no children
            if (node.Left == null && node.Right == null)
                return null;

            //1 child
            if (node.Left == null)
                return node.Right;
            else if (node.Right == null)
                return node.Left;

            //2 children
            else
            {
                node.Data = MaxValue(Root.Left);
                node.Left = DeleteLogic(node.Left, node.Data.Difficulty);
            }
            //balancing

        }

        UpdateHeight(node);
        return BalanceTree(node);
    }

    private Challenge MinValue(Node node)
    {
        Challenge minDifChallenge = node.Data;
        while (node.Left != null)
        {
            minDifChallenge = node.Left.Data;
            node = node.Left;
        }
        return minDifChallenge;
    }

    private Challenge MaxValue(Node node)
    {
        Challenge maxDifChallenge = node.Data;
        while (node.Right != null)
        {
            maxDifChallenge = node.Right.Data;
            node = node.Right;
        }
        return maxDifChallenge;
    }
    private void UpdateHeight(Node node)
    {
        node.Height = 1 + Math.Max(node.Right?.Height ?? 0, node.Left?.Height ?? 0);
    }

    private int balancingFactor(Node node)
    {
        if(node == null)
            return 0;
        else
            return (node.Left?.Height ?? 0) - (node.Right?.Height ?? 0);
    }
    private Node BalanceTree(Node node)
    {
        //AVL tree to self balance the tree. I selected this method because it is not that hard to implement and rotations are O(1)
        int balanceFactor = balancingFactor(node);

        //LL
        if(balanceFactor > 1 && balancingFactor(node.Left) >= 0)
        {
            return RotateRight(node);
        }
        //LR
        if(balanceFactor > 1 && balancingFactor(node.Left) < 0)
        {
            node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }
        //RR
        if(balanceFactor < -1 && balancingFactor(node.Right) <= 0)
        {
            return RotateLeft(node);
        }
        //RL
        if(balanceFactor < -1 && balancingFactor(node.Right) > 0)
        {
            node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }

        return node;
    }

    private Node RotateRight(Node A)
    {
        Node B = A.Left;
        Node? Br = B.Right;

        B.Right = A;
        A.Left = Br;

        UpdateHeight(A);
        UpdateHeight(B);

        return B;
    }

    private Node RotateLeft(Node B)
    {
        Node A = B.Right;
        Node? Al = A.Left;

        A.Left = B;
        B.Right = Al;

        UpdateHeight(B);
        UpdateHeight(A);

        return A;
    }

    public void DisplayChallenges()
    {
        System.Console.WriteLine("Display Challenges");
        InOrderTraversal(Root);
        System.Console.WriteLine();

    }

    public void InOrderTraversal(Node node)
    {
        if (node != null)
        {
            InOrderTraversal(node.Left);
            System.Console.WriteLine(node.Data);
            InOrderTraversal(node.Right);
        }
    }

    public Challenge FindClosestChallenge(int roomNumber)
    {
        Challenge? searchedChallenge = Search(roomNumber);
        if (searchedChallenge != null)
        {
            return searchedChallenge;
        }

        Node currentNode = Root;
        int BestChallengeDifference = int.MaxValue;
        while (currentNode != null)
        {
            if ((Math.Abs(currentNode.Data.Difficulty) - roomNumber) < BestChallengeDifference)
            {
                BestChallengeDifference = Math.Abs(currentNode.Data.Difficulty) - roomNumber;
                searchedChallenge = currentNode.Data;

            }

            if (roomNumber < currentNode.Data.Difficulty)
                currentNode = currentNode.Left;
            else if (roomNumber > currentNode.Data.Difficulty)
                currentNode = currentNode.Right;
            else
                break;
        }
        return searchedChallenge;
    }

    public Challenge? Search(int searchDifficulty)
    {
        Node searchNode = SearchLogic(Root, searchDifficulty);
        if (searchNode == null)
            return null;
        else
            return searchNode.Data;
    }
    public Node SearchLogic(Node node, int searchDifficulty)
    {
        if (node == null)
            return null;
        if (node.Data.Difficulty == searchDifficulty)
            return node;
        else if (searchDifficulty < node.Data.Difficulty)
            return SearchLogic(node.Left, searchDifficulty);
        else
            return SearchLogic(node.Right, searchDifficulty);
    }

    private void AddChallenges()
    {
        Insert(new Challenge("Puzzle", 2, Item.WisdomBook, 4, "Intelligence"));
        Insert(new Challenge("Puzzle", 3, Item.Lockpick, 6, "Intelligence"));
        Insert(new Challenge("Trap", 5, Item.SpeedBoots, 4, "Agility"));
        Insert(new Challenge("Combat", 7, Item.Sword, 5, "Strenght"));
        Insert(new Challenge("Trap", 8, Item.Teleport, 5, "Agility"));
        
        Insert(new Challenge("Combat", 9, Item.Shield, 7, "Strenght"));
        Insert(new Challenge("Trap", 10, Item.SpeedBoots, 8, "Agility"));
        Insert(new Challenge("Combat", 11, Item.Sword, 7, "Strenght"));
        Insert(new Challenge("Puzzle", 12, Item.Lockpick, 9, "Intelligence"));
        Insert(new Challenge("Combat", 13, Item.Shield, 9, "Strenght"));
        
        Insert(new Challenge("Trap", 14, Item.Teleport, 10, "Agility"));
        Insert(new Challenge("Trap", 15, Item.Teleport, 12, "Agility"));
        Insert(new Challenge("Puzzle", 17, Item.Lockpick, 10, "Intelligence"));
        Insert(new Challenge("Puzzle", 18, Item.WisdomBook, 12, "Intelligence"));
        Insert(new Challenge("Combat", 20, Item.Shield, 15, "Strenght"));
    }
}