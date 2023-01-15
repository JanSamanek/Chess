using ChessBoardModel;

string position = "4r3/8/8/8/8/4K3/8/8 w - - 0 1";
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

// have to add GetPin, kingsquare, castling and en_passant, clear moveList => what happens when turn ends???