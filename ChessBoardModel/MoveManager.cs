namespace ChessBoardModel
{
    public static class MoveManager
    {
        enum CastlingType { KSWhite = 8, QSWhite = 4, KSBlack = 2, QSBlack = 1 };
        static int Castling = 0b1111;
        
        static bool IsCastlingLegal(CastlingType type)
        {
            if((Castling & (int) type) != 0)
            {
                List<int> attackedSquares = GetAttackedSquares();   // is it possible to calc attacked squares only once?
                switch (type)
                {
                    case CastlingType.KSWhite:
                        if (Board.Grid[(int) Board.Coordinate.f1] == Pieces.Empty && Board.Grid[(int) Board.Coordinate.g1] == Pieces.Empty)
                        {
                            if(!attackedSquares.Contains((int) Board.Coordinate.f1) && !attackedSquares.Contains((int)Board.Coordinate.g1))
                            return true;
                        }
                        break;

                    case CastlingType.QSWhite:
                        if (Board.Grid[(int)Board.Coordinate.d1] == Pieces.Empty && Board.Grid[(int)Board.Coordinate.c1] == Pieces.Empty)
                        {
                            if (!attackedSquares.Contains((int)Board.Coordinate.d1) && !attackedSquares.Contains((int)Board.Coordinate.c1))
                                return true;
                        }
                        break;
                        
                    case CastlingType.KSBlack:
                        if (Board.Grid[(int)Board.Coordinate.f8] == Pieces.Empty && Board.Grid[(int)Board.Coordinate.g8] == Pieces.Empty)
                        {
                            if (!attackedSquares.Contains((int)Board.Coordinate.f8) && !attackedSquares.Contains((int)Board.Coordinate.g8))
                                return true;
                        }
                        break; 

                    case CastlingType.QSBlack:
                        if (Board.Grid[(int)Board.Coordinate.d8] == Pieces.Empty && Board.Grid[(int)Board.Coordinate.c8] == Pieces.Empty)
                        {
                            if (!attackedSquares.Contains((int)Board.Coordinate.d8) && !attackedSquares.Contains((int)Board.Coordinate.d8))
                                return true;
                        }
                        break;
                }
            }
            return false;
        }
        public readonly struct Move
        {
            public readonly int originSquare;
            public readonly int targetSquare;

            public Move(int originSquare, int targetSquare)
            {
                this.originSquare = originSquare;
                this.targetSquare = targetSquare;
            }
        }
        public static void MakeMove(Move move)
        {
            Board.Grid[move.targetSquare] = Board.Grid[move.originSquare];
            Board.Grid[move.originSquare] = Pieces.Empty;
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
                    int pieceOnSquare = Board.Grid[targetSquare];
                    if (pieceOnSquare == Pieces.Empty)
                    {
                        yield return new Move(originSquare, targetSquare);
                        targetSquare += dirOffset;
                    }
                    //on target square is a piece of opposite color
                    else if (Pieces.GetPieceColor(pieceOnSquare) != Board.SideToMove)
                    {
                        yield return new Move(originSquare, targetSquare);
                        break;
                    }
                    //on target square is a piece of the same color
                    else if (Pieces.GetPieceColor(pieceOnSquare) == Board.SideToMove)
                        break;
                }
            }
        }
        static IEnumerable<Move> GenerateKingMoves(int originSquare)
        {
            List<int> attackedSquares = GetAttackedSquares();
            foreach(int offset in Pieces.DirOffsets)
            {
                int targetSquare = originSquare + offset;
                if ((targetSquare & 0x88) == 0 && !attackedSquares.Contains(targetSquare))
                {
                    int pieceOnSquare = Board.Grid[targetSquare];
                    if(Pieces.GetPieceColor(pieceOnSquare) != Board.SideToMove)
                    {
                        yield return new Move(originSquare, targetSquare);
                    }
                }
            }
        }
        static IEnumerable<Move> GenerateKnightMoves(int originSquare)
        {
            foreach (int offset in Pieces.KnightOffsets)
            {
                int targetSquare = originSquare + offset;
                //if target square is on chessboard and is not a friendly piece
                if ((targetSquare & 0x88) == 0)
                {
                    int pieceOnSquare = Board.Grid[targetSquare];
                    if (Pieces.GetPieceColor(pieceOnSquare) != Board.SideToMove)
                        yield return new Move(originSquare, targetSquare);
                }
            }
        }
        static IEnumerable<Move> GeneratePawnMoves(int originSquare)
        {
            int dir = Board.SideToMove == Pieces.White ? 1 : -1;
            int pawnColor = Pieces.GetPieceColor(Board.Grid[originSquare]);
            int rank = Board.GetRank(originSquare);

            int targetSquare = originSquare + 2 * dir * Pieces.Foward;
            int pieceOnTargetSquare = Board.Grid[targetSquare];

            if (pawnColor == Pieces.White && rank == 1 && pieceOnTargetSquare == Pieces.Empty)
            {
                yield return new Move(originSquare, targetSquare);
            }
            else if (pawnColor == Pieces.Black && rank == 6 && pieceOnTargetSquare == Pieces.Empty)
            {
                yield return new Move(originSquare, targetSquare);
            }

            targetSquare = originSquare + dir * Pieces.Foward;
            pieceOnTargetSquare = Board.Grid[targetSquare];

            if (pieceOnTargetSquare == Pieces.Empty)
                yield return new Move(originSquare, targetSquare);

            foreach (int attackOffset in Pieces.PawnAttacks)
            {
                targetSquare = originSquare + attackOffset * dir;
                pieceOnTargetSquare = Board.Grid[targetSquare];

                int colorOfPiece = Pieces.GetPieceColor(pieceOnTargetSquare);
                if (colorOfPiece == Board.SideWaiting)
                {
                    yield return new Move(originSquare, targetSquare);
                }
            }
        }
        static IEnumerable<Move> GenerateMovesForSquare(int originSquare, int color)
        {
            int piece = Board.Grid[originSquare];

            if (piece == (Pieces.Knight | color))
            {
                return GenerateKnightMoves(originSquare);
            }
            else if (piece == (Pieces.Queen | color))
            {
                return GenerateSlidingMoves(originSquare, Pieces.Queen);
            }
            else if (piece == (Pieces.Bishop | color))
            {
                return GenerateSlidingMoves(originSquare, Pieces.Bishop);
            }
            else if (piece == (Pieces.Rook | color))
            {
                return GenerateSlidingMoves(originSquare, Pieces.Rook);
            }   
            else if (piece == (Pieces.Pawn | color))
            {
                return GeneratePawnMoves(originSquare);
            }
            else if(piece == (Pieces.King | color))
            {
                return GenerateKingMoves(originSquare);
            }
            else
            {
                IEnumerable<Move> empty = Enumerable.Empty<Move>();
                return empty;
            }
        }
        public static List<Move> GetMovesForBoard(int color)
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
                        foreach (Move move in GenerateMovesForSquare(originSquare, color))
                            moves.Add(move);
                    }
                }
            }
            return moves;
        }
        static List<int> GetAttackedSquares()
        {
            List<int> attackedSquares = new List<int>();
            foreach(Move move in GetMovesForBoard(Board.SideWaiting))
            {
                attackedSquares.Add(move.targetSquare);
            }
            return attackedSquares;
        }
        public static List<Move> GetLegalMoves()
        {
            return GetMovesForBoard(Board.SideToMove);
        }
    }
}
