using ChessBoardModel;

Board.SetPieces("8/3p2p1/2R1P1R1/8/8/8/8/8 w - - 0 1");
//Board.Grid[(int)Board.Coordinates.a1] = 10;
List<Board.Move> moves = Board.GenerateMovesForBoard();
foreach(Board.Move move in moves)
{
    Board.Grid[move.targetSquare] = 77;
}
Board.Show();