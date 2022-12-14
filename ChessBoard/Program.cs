using ChessBoardModel;

Board.SetPieces("8/8/3r1r2/3R4/4K3/8/8/8 w - - 0 1");
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
    Board.SetPieces("8/8/3r1r2/3R4/4K3/8/8/8 w - - 0 1");
    Console.Clear();
}