using ChessBoardModel;

string position = "8/8/5B2/q3p3/1P1k3b/8/5P2/4K3 b - - 0 1";
Board.SetPieces(position);
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

// have to add GetPin, kingsquare, castling and en_passant => what happens when turn ends???