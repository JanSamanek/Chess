namespace ChessBoardModel
{
    public static class Pieces
    {

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
    }
}