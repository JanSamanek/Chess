using static ChessBoardModel.Pieces;

namespace ChessBoardModel
{
    public static class Pieces
    {
        #region Pieces Init section
        const int Empty = 0;
        const int Pawn = 1;
        const int Knight = 2;
        const int Bishop = 3;
        const int Rook = 4;
        const int Queen = 5;
        const int King = 6;
        const int Border = 7;

        public const int White = 8;
        public const int Black = 16;

        public static readonly Dictionary<char, int> SymbolsToPieces = new Dictionary<char, int>()
        {
            ['p'] = Pawn, ['n'] = Knight, ['b'] = Bishop, ['r'] = Rook, 
            ['q'] = Queen, ['k'] = King, ['e'] = Empty, ['o'] = Border
        };
        #endregion

        #region Move section
        static readonly int[] DirOffsets = {16, -16, 1, -1, 15, -15, 17, -17 };
        static readonly int[] KnightOffsets = { 33, 31, 18, 14, -33, -31, -18, -14 };
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

        static void GenerateMovesForSquare(int originSquare, List<Move> moveList)
        {
            int piece = Board.Grid[originSquare];

            #region Knight Moves
            if (piece == (Knight | Board.SideToPlay))
            {
                foreach (int offset in KnightOffsets)
                {
                    int targetSquare = originSquare + offset;
                    if ((targetSquare & 0x88) == 0)
                        moveList.Add(new Move(originSquare, targetSquare));
                }
            }
            #endregion

            #region Queen Moves
            if (piece == (Queen | Board.SideToPlay))
            {
                foreach (int offset in DirOffsets)
                {
                    int targetSquare = originSquare + offset;
                    while ((targetSquare & 0x88) == 0)
                    {
                        int squareValue = Board.Grid[targetSquare];
                        if (squareValue == Empty)
                        {
                            moveList.Add(new Move(originSquare, targetSquare));
                            targetSquare += offset;
                        }
                        //on target square is a piece of opposite color
                        else if ((squareValue & 0x18) != Board.SideToPlay)
                        {
                            moveList.Add(new Move(originSquare, targetSquare));
                            break;
                        }
                        //on target square is a piece of the same color
                        else if ((squareValue & 0x18) == Board.SideToPlay)
                            break;
                    }

                }
            }
            #endregion
        }

        public static List<Move> GenerateMovesForBoard()
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
                        Pieces.GenerateMovesForSquare(originSquare, moves);
                    }
                }
            }

            return moves;
        }
    #endregion
    }
}