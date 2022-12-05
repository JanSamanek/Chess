using ChessBoardModel;

Board.SetPieces("8/1k2b3/4R3/8/4Q1n1/8/8/8 w - - 0 1");
//Board.Grid[(int)Board.Coordinates.e5] = 13;
List<Pieces.Move> moves = Pieces.GenerateMoves();
foreach(Pieces.Move move in moves)
{
    Board.Grid[move.targetSquare] = 77;
}
Board.Show();

Console.WriteLine("♚");