using ChessBoardModel;

string position = "3r4/8/2q5/2p4b/r1pP4/5P2/2R5/3K4 w - - 0 1";
Board.SetPieces(position);
IEnumerable<MoveManager.Move> moves = MoveManager.GetLegalMoves();
Board.Show();
MoveManager.GetPinnedSquares();
Thread.Sleep(10000);
Console.Clear();

foreach (MoveManager.Move move in moves)
{
    MoveManager.MakeMove(move);
    Board.Show();
    Thread.Sleep(1000);
    Board.SetPieces(position);
    Console.Clear();
}

// have to add kingsquare, castling and en_passant, what happens when turn ends???