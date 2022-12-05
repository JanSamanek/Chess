namespace ChessBoardModel
{

    public static class FenParser
    {
        public static void parse(string fen)
        {
            string fenBoard = fen.Split(" ")[0];

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
