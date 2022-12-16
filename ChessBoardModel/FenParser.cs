namespace ChessBoardModel
{

    public static class FenParser
    {
        public static void Parse(string fen)
        {
            string[] fenSplit = fen.Split(" ");
            string fenBoard = fenSplit[0];
            string sideToMove = fenSplit[1];
            string Castling = fenSplit[2];

            Board.SideToMove = sideToMove == "w" ? Pieces.White: Pieces.Black;
            Board.SideWaiting = Board.SideToMove ^ 0x18;

            foreach(char castlingType in Castling)
            {
                switch(castlingType)
                {
                    case 'K': MoveManager.Castling |= 0b1000; break;
                    case 'Q': MoveManager.Castling |= 0b0100; break;
                    case 'k': MoveManager.Castling |= 0b0010; break;
                    case 'q': MoveManager.Castling |= 0b0001; break;
                }
            }

            int rank = 7;
            int file = 0;

            foreach (char value in fenBoard)
            {
                if(value == '/')
                {
                    file = 0;
                    rank--;
                }
                else
                {
                    if(char.IsDigit(value))
                    {
                        file += (int) char.GetNumericValue(value);
                    }
                    else
                    {
                        int pieceType = Pieces.SymbolsToPieces[char.ToLower(value)];
                        int pieceColor = char.IsUpper(value) ? Pieces.White : Pieces.Black;
                        int square = rank * 16 + file;
                        Board.Grid[square] = pieceType | pieceColor;
                        file++;
                    }
                }
            }
        }
    }
}
