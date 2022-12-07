using ChessBoardModel;

Board.SetPieces("8/1k2b3/4R3/8/4Q1n1/8/8/8 w - - 0 1");
Board.Grid[(int)Board.Coordinates.a1] = 10;
List<Pieces.Move> moves = Pieces.GenerateMovesForBoard();
foreach(Pieces.Move move in moves)
{
    Board.Grid[move.targetSquare] = 77;
}
Board.Show();

Console.WriteLine("♚");