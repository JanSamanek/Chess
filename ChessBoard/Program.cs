using ChessBoardModel;

Board.SetPieces(Board.startPos);
//Board.Grid[(int)Board.Coordinates.a1] = 10;
List<MoveManager.Move> moves = MoveManager.GenerateMovesForBoard();
foreach(MoveManager.Move move in moves)
{
    MoveManager.MakeMove(move);
    Board.Show();
    Thread.Sleep(1000);
    Board.SetPieces(Board.startPos);
    Console.Clear();
}
