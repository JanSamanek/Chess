﻿using System.CodeDom.Compiler;

namespace ChessBoardModel
{
    public static class Board
    {
        public static int[] Grid = new int[128];
        public const string startPos = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        public static int SideToPlay = Pieces.Black;  //TODO: make sides to play not static
        public static int SideWaiting = SideToPlay^0x18;
        public enum Coordinates
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
                        Grid[square] = Pieces.SymbolsToPieces['e'];
                    }
                    else
                    {
                        Grid[square] = Pieces.SymbolsToPieces['o'];
                    }
                }
            }
        }
        public static void SetPieces(string fen)
        {
            Board.SetEmpty();
            FenParser.parse(fen);
        }
        public static void Show()
        {
            for (int rank = 7; rank >= 0; rank--)
            {
                for (int file = 0; file < 16; file++)
                {
                    int square = rank * 16 + file;

                    // if square is on chessbaord
                    if ((square & 0x88) == 0)
                    {
                        Console.Write($"{Grid[square], -3}");
                    }
                }
                Console.Write("\n");
            }
        }

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
        static IEnumerable<Move> GenerateSlidingMoves(int originSquare, int piece)
        {
            //get the coresponding dir offsets for piece
            int startIndex = piece == Pieces.Bishop ? 4 : 0;
            int endIndex = piece == Pieces.Rook ? 4 : 8;
                    
            foreach (int dirOffset in Pieces.DirOffsets[startIndex..endIndex])
            {
                int targetSquare = originSquare + dirOffset;
                while ((targetSquare & 0x88) == 0)
                {
                    int pieceOnSquare = Grid[targetSquare];
                    if (pieceOnSquare == Pieces.Empty)
                    {
                        yield return new Move(originSquare, targetSquare);
                        targetSquare += dirOffset;
                    }
                    //on target square is a piece of opposite color
                    else if (Pieces.getPieceColor(pieceOnSquare) != SideToPlay)
                    {
                        yield return new Move(originSquare, targetSquare);
                        break;
                    }
                    //on target square is a piece of the same color
                    else if (Pieces.getPieceColor(pieceOnSquare) == SideToPlay)
                        break;
                }
            }
        }
        static IEnumerable<Move> GenerateKnightMoves(int originSquare)
        {
            foreach (int offset in Pieces.KnightOffsets)
            {
                int targetSquare = originSquare + offset;
                if ((targetSquare & 0x88) == 0)
                    yield return new Move(originSquare, targetSquare);
            }
        }
        static IEnumerable<Move> GeneratePawnMoves(int originSquare)
        {
            int dir = SideToPlay == Pieces.White ? 1 : -1;

            int targetSquare = originSquare + dir * Pieces.Foward;
            int pieceOnAttackSquare = Grid[targetSquare];

            if (pieceOnAttackSquare == Pieces.Empty)
                yield return new Move(originSquare, originSquare + dir * Pieces.Foward);

            foreach(int attackOffset in Pieces.PawnAttacks)
            {
                targetSquare = originSquare + attackOffset * dir;
                pieceOnAttackSquare= Grid[targetSquare];

                int colorOfPiece = Pieces.getPieceColor(pieceOnAttackSquare);
                if (colorOfPiece == SideWaiting)
                {
                    yield return new Move(originSquare, targetSquare);
                }
            }
        }
        static IEnumerable<Move> GenerateMovesForSquare(int originSquare)
        {
            int piece = Board.Grid[originSquare];

            if (piece == (Pieces.Knight | Board.SideToPlay))
            {
                return GenerateKnightMoves(originSquare);
            }
            else if (piece == (Pieces.Queen | Board.SideToPlay))
            {
                return GenerateSlidingMoves(originSquare, Pieces.Queen);
            }
            else if(piece == (Pieces.Bishop| Board.SideToPlay))
            {
                return GenerateSlidingMoves(originSquare, Pieces.Bishop);
            }
            else if(piece ==(Pieces.Rook | Board.SideToPlay))
            {
                return GenerateSlidingMoves(originSquare, Pieces.Rook);
            }
            else if(piece == (Pieces.Pawn | Board.SideToPlay))
            {
                return GeneratePawnMoves(originSquare);
            }
            else
            {
                IEnumerable<Move> empty = Enumerable.Empty<Move>();
                return empty;
            }
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
                        foreach(Move move in GenerateMovesForSquare(originSquare))
                            moves.Add(move);
                    }
                }
            }
            return moves;
        }
        #endregion
    }
}
