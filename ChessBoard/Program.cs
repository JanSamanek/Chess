using ChessBoardModel;

string position = "4q1R1/8/8/8/8/8/8/4K2R w Ke - 0 1";
Board.SetPieces(position);
IEnumerable<MoveManager.Move> moves = MoveManager.GetLegalMoves();
Board.Show();
Thread.Sleep(1000);
Console.Clear();

foreach (MoveManager.Move move in moves)
{
    MoveManager.MakeMove(move);
    Board.Show();
    Thread.Sleep(1000);
    Board.SetPieces(position);
    Console.Clear();
}