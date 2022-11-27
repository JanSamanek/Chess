namespace ChessBoardModel
{
    public static class Pieces
    {
        public class Pawn
        {
            public const int value = 1;

        }
        public class Knigth
        {
            public const int value = 2;
            public static int[] knight_offsets = { 33, 31, 18, 14, -33, -31, -18, -14 };

            public static IEnumerable<int> getAttackedSquares(int square)
            {
                foreach(int offset in knight_offsets)
                {
                    int attackedSquare = square + offset;
                    if((attackedSquare & 0x88) == 0)
                    {
                        yield return attackedSquare;
                    }
                }
            }
        }
        public class Bishop
        {
            public const int value = 3;
            public static int[] bishop_offsets = { 15, 17, -15, -17 };
        }
        public class Rook
        {
            public const int value = 4;
            public static int[] rook_offsets = { 16, -16, 1, -1 };
        }
        public class Queen
        {
            public const int value = 5;
        }
        public class King
        {
            public const int value = 6;
            public static int[] king_offsets = { 16, -16, 1, -1, 15, 17, -15, -17 };
        }

        public const int Border = 7;
        public const int Empty = 0;
        public const int White = 8;
        public const int Black = 16;

        public static Dictionary<char, int> piecesToSymbols = new Dictionary<char, int>()
        {
            ['p'] = Pawn.value,
            ['n'] = Knigth.value,
            ['b'] = Bishop.value,
            ['r'] = Rook.value,
            ['q'] = Queen.value,
            ['k'] = King.value,
            ['e'] = Empty,
            ['o'] = Border,
        };
    }
}