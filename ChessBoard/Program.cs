using ChessBoardModel;

Board.SetPieces(Board.startPos);
//Board.Grid[(int)Board.Coordinates.e5] = 10;
List<Pieces.Move> moves = Pieces.GenerateMoves();
foreach(Pieces.Move move in moves)
{
    Board.Grid[move.targetSquare] = 77;
}
Board.Show();

Console.WriteLine("♚");