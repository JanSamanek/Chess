using ChessBoardModel;

string position = Board.startPos;
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

// have to add kingsquare, castling and en_passant, what happens when turn ends???