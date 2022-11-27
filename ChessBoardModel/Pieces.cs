namespace ChessBoardModel
{
    public static class Pieces
    {
        public const int Empty = 0;
        public const int Pawn = 1;
        public const int Knigth = 2;
        public const int Bishop = 3;
        public const int Rook = 4;
        public const int Queen = 5;
        public const int King = 6;
        public const int Border = 7;

        public const int White = 8;
        public const int Black = 16;

        public static int[] knight_offsets = { 33, 31, 18, 14, -33, -31, -18, -14 };
        public static int[] bishop_offsets = { 15, 17, -15, -17 };
        public static int[] rook_offsets = { 16, -16, 1, -1 };
        public static int[] king_offsets = { 16, -16, 1, -1, 15, 17, -15, -17 };

        public static Dictionary<char, int> piecesToSymbols = new Dictionary<char, int>()
        {
            ['p'] = Pawn,
            ['n'] = Knigth,
            ['b'] = Bishop,
            ['r'] = Rook,
            ['q'] = Queen,
            ['k'] = King,
            ['e'] = Empty,
            ['o'] = Border,
        };
    }
}