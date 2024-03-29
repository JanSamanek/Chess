﻿namespace ChessBoardModel
{
    public static class Board
    {
        public static int[] Grid { get; set; } = new int[128];
        public static int KingSquare { get; set; }
        public const string startPos = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        public static int SideToMove { get; set; }
        public static int SideWaiting { get; set; }
        public enum Coordinate
        {
            a1, b1, c1, d1, e1, f1, g1, h1, i1, j1, k1, l1, m1, n1, o1, p1,
            a2, b2, c2, d2, e2, f2, g2, h2, i2, j2, k2, l2, m2, n2, o2, p2,
            a3, b3, c3, d3, e3, f3, g3, h3, i3, j3, k3, l3, m3, n3, o3, p3,
            a4, b4, c4, d4, e4, f4, g4, h4, i4, j4, k4, l4, m4, n4, o4, p4,
            a5, b5, c5, d5, e5, f5, g5, h5, i5, j5, k5, l5, m5, n5, o5, p5,
            a6, b6, c6, d6, e6, f6, g6, h6, i6, j6, k6, l6, m6, n6, o6, p6,
            a7, b7, c7, d7, e7, f7, g7, h7, i7, j7, k7, l7, m7, n7, o7, p7,
            a8, b8, c8, d8, e8, f8, g8, h8, i8, j8, k8, l8, m8, n8, o8, p8,
        };
        public static int GetRank(int square) => square / 16;
        static void SetEmpty()
        {
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 16; file++)
                {
                    int square = rank * 16 + file;

                    // if square is on chessbaord
                    if ((square & 0x88) == 0)
                    {
                        Grid[square] = Pieces.Empty;
                    }
                    else
                    {
                        Grid[square] = Pieces.Border;
                    }
                }
            }
        }
        public static void SetPieces(string fen)
        {
            Board.SetEmpty();
            FenParser.Parse(fen);
        }
        public static void Show()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            for (int rank = 7; rank >= 0; rank--)
            {
                for (int file = 0; file < 16; file++)
                {
                    int square = rank * 16 + file;

                    // if square is on chessbaord
                    if ((square & 0x88) == 0)
                    {
                        int piece = Grid[square];
                        int pieceValue = Pieces.GetPieceValue(piece);
                        string[] unicode = Pieces.GetPieceColor(piece) == Pieces.White ? Pieces.unicodeWhitePieces : Pieces.unicodeBlackPieces;
                        Console.Write($"{unicode[pieceValue], -3}");
                    }
                }
                Console.Write("\n");
            }
        }
        public static Coordinate? ParseCoordinate(string coordinate) => coordinate == "-" ? null : Enum.Parse<Coordinate>(coordinate);
    }
}
