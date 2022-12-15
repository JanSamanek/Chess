using ChessBoardModel;
string position = "3r4/8/8/8/8/8/8/4K1R1 w - - 0 1";
Board.SetPieces(position);
//Board.Grid[(int)Board.Coordinates.a1] = 10;
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