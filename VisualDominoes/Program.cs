using DominoEngine;

Console.Clear();
Game DominoGame;
int selection;
do{
    PrintMenu();
    selection = int.Parse(Console.ReadLine());
}while(selection != 1 && selection != 2);
if(selection != 2)
{ 
    DominoGame = new Game();
    System.Console.WriteLine("How many player dou you want to play with?");
    int n =int.Parse(Console.ReadLine());
    List<Player<Number>> Players = new List<Player<Number>>();
    for(int i = 0; i < n; i++)
    {
        int P = i+1;
        System.Console.WriteLine("Say the name of player "+ P);
        Players.Add(new HumanPlayer<Number>(i, Console.ReadLine()));
    }
    DominoGame.AddPlayers(Players);
    DominoGame.GiveChips();
    while(true)
    {
        PrintGame(DominoGame);
        DominoGame.CurrentTurn();
        if(DominoGame.EndGame()) break;
        Console.ReadKey();
    }
    Console.Clear();
    Console.WriteLine("the winer is: "+DominoGame.Winner.Name);
}


static void PrintGame(Game DominoGame)
{
    Console.Clear();
    System.Console.WriteLine("Turn: "+DominoGame.CurrentPlayer.Name);
    System.Console.WriteLine();
    PrintTable(DominoGame.table); 
    System.Console.WriteLine("Hand:");
    PrintHand(DominoGame.CurrentPlayer.GetHand());

}
static void PrintMenu()
{
    System.Console.WriteLine("Welcom to Pendavid Dominos");
    System.Console.WriteLine("Chouse the Kind of game you wonna lay");
    System.Console.WriteLine("1: Clasic domino");
    System.Console.WriteLine("2: Quit");
}
static void PrintTable(Board<Number> table)
{
    List<Number> Table = table.GetBoard().ToList();
    for(int i = 0; i<Table.Count; i=i+2)
    {
        System.Console.Write("["+Table[i].number+"|"+Table[i+1].number+"]");
    }
    System.Console.WriteLine();
    
}
static void PrintHand(List<Chip<Number>> Hand)
{
    for(int i=0; i<Hand.Count; i++)
    {
        System.Console.Write("["+Hand[i].LinkL.number+"] ");
    }
    System.Console.WriteLine();
    for(int i = 0; i < Hand.Count; i++)    
    {    
        System.Console.Write("["+Hand[i].LinkR.number+"] ");  
    }
    System.Console.WriteLine();
}