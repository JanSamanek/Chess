using ChessBoardModel;

Board.SetPieces("8/8/8/3Qb3/3K4/8/8/8 w - - 0 1");
//Board.Grid[(int)Board.Coordinates.a1] = 10;
List<MoveManager.Move> moves = MoveManager.GenerateMovesForBoard();
foreach(MoveManager.Move move in moves)
{
    MoveManager.MakeMove(move);
    Board.Show();
    Thread.Sleep(1000);
    Board.SetPieces("8/8/8/3Qb3/3K4/8/8/8 w - - 0 1");
    Console.Clear();
}