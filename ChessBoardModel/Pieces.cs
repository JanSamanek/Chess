using static ChessBoardModel.Pieces;

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

        public static readonly int[] DirOffsets = { 16, -16, 1, -1, 15, -15, 17, -17 };
        public static readonly int[] KnightOffsets = { 33, 31, 18, 14, -33, -31, -18, -14 };

        public static readonly Dictionary<char, int> SymbolsToPieces = new Dictionary<char, int>()
        {
            ['p'] = Pawn, ['n'] = Knight, ['b'] = Bishop, ['r'] = Rook, 
            ['q'] = Queen, ['k'] = King, ['e'] = Empty, ['o'] = Border
        };
        #endregion

       
    }
}