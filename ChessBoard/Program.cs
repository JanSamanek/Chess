using ChessBoardModel;
string position = "5r2/8/8/8/8/8/8/4K2R w - - 0 1";
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