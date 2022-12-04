namespace ChessBoardModel
{
    public static class Pieces
    {
        #region Pieces Init section
        public const int Empty = 0;
        public const int Pawn = 1;
        public const int Knight = 2;
        public const int Bishop = 3;
        public const int Rook = 4;
        public const int Queen = 5;
        public const int King = 6;
        public const int Border = 7;

        public const int White = 8;
        public const int Black = 16;

        public static Dictionary<char, int> piecesToSymbols = new Dictionary<char, int>()
        {
            ['p'] = Pawn,
            ['n'] = Knight,
            ['b'] = Bishop,
            ['r'] = Rook,
            ['q'] = Queen,
            ['k'] = King,
            ['e'] = Empty,
            ['o'] = Border,
        };
        #endregion

        #region Move section
        public struct Move
        {
            public readonly int originSquare;
            public readonly int targetSquare;

            public Move(int originSquare, int targetSquare)
            {
                this.originSquare = originSquare;
                this.targetSquare = targetSquare;
            }
        }

        public static List<Move> GenerateMoves()
        {
            List<Move> moves = new List<Move>();

            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 16; file++)
                {
                    int originSquare = rank * 16 + file;

                    // if square is on chessbaord
                    if ((originSquare & 0x88) == 0)
                    {
                        int piece = Board.Grid[originSquare];

                        if(piece == (Knight | Board.SideToPlay))
                        {
                            Console.WriteLine("Knight " + originSquare);
                        }
                    }
                }
            }

            return moves;
        }

        #endregion

    }
}