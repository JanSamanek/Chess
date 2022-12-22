using ChessBoardModel;

string position = "3r4/8/2q5/2p4b/r1p5/2P2P2/3R4/3K4 w - - 0 1";
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