using ChessBoardModel;

Board.SetPieces(Board.startPos);
//Board.Grid[(int)Board.Coordinates.a1] = 10;
List<Board.Move> moves = Board.GenerateMovesForBoard();
foreach(Board.Move move in moves)
{
    Board.Grid[move.targetSquare] = Board.Grid[move.originSquare];
    Board.Grid[move.originSquare] = Pieces.Empty;
    Board.Show();
    Thread.Sleep(1000);
    Board.SetPieces(Board.startPos);
    Console.Clear();
}