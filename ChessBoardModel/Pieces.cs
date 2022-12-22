namespace ChessBoardModel
{
    public static class Pieces
    {
        #region Pieces Init section
        public const int Empty = 0, Pawn = 1, Knight = 2, Bishop = 3, Rook = 4, Queen = 5, King = 6, Border = 7;
        public const int White = 8, Black = 16;

        public static readonly int[] DirOffsets = { 16, -16, 1, -1, 15, -15, 17, -17 };
        public static readonly int[] StraightOffsets = { 16, -16, 1, -1 };
        public static readonly int[] DiagonalOffsets = { 15, -15, 17, -17 };
        public static readonly int[] KnightOffsets = { 33, 31, 18, 14, -33, -31, -18, -14 };
        public static readonly int[] PawnAttacks = { 15, 17 };
        public static readonly int Foward = 16;

        public static readonly string[] unicodeWhitePieces = { ".", "♙", "♘", "♗", "♖", "♕", "♔" };
        public static readonly string[] unicodeBlackPieces = { ".", "♟", "♞", "♝", "♜", "♛", "♚" };

        public static readonly Dictionary<char, int> SymbolsToPieces = new Dictionary<char, int>()
        {
            ['p'] = Pawn, ['n'] = Knight, ['b'] = Bishop, ['r'] = Rook, 
            ['q'] = Queen, ['k'] = King, ['e'] = Empty, ['o'] = Border
        };
        #endregion
        public static int GetPieceColor(int piece) => piece & 0x18;
        public static int GetPieceValue(int piece) => piece & 0x7;
        public static int GetColorOfOtherSide(int color) => color ^ 0x18;
    }
}