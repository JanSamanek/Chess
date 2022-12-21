namespace ChessBoardModel
{
    public static class MoveManager
    {
        public static Board.Coordinate? En_passant { get; set; }
        public enum CastlingType { KSWhite = 8, QSWhite = 4, KSBlack = 2, QSBlack = 1 };
        public static int Castling { get; set; } = 0b1111;
        static bool CastlingCheck(CastlingType type, List<int> attackedSquares)
        {
            if((Castling & (int) type) != 0)
            {
                List<int> AttackedSquares = attackedSquares;
                switch (type)
                {
                    case CastlingType.KSWhite:
                        if (Board.Grid[(int) Board.Coordinate.f1] == Pieces.Empty && Board.Grid[(int) Board.Coordinate.g1] == Pieces.Empty)
                        {
                            if(!AttackedSquares.Contains((int) Board.Coordinate.f1) && !AttackedSquares.Contains((int)Board.Coordinate.g1))
                            return true;
                        }
                        break;

                    case CastlingType.QSWhite:
                        if (Board.Grid[(int)Board.Coordinate.d1] == Pieces.Empty && Board.Grid[(int)Board.Coordinate.c1] == Pieces.Empty)
                        {
                            if (!AttackedSquares.Contains((int)Board.Coordinate.d1) && !AttackedSquares.Contains((int)Board.Coordinate.c1))
                                return true;
                        }
                        break;
                        
                    case CastlingType.KSBlack:
                        if (Board.Grid[(int)Board.Coordinate.f8] == Pieces.Empty && Board.Grid[(int)Board.Coordinate.g8] == Pieces.Empty)
                        {
                            if (!AttackedSquares.Contains((int)Board.Coordinate.f8) && !AttackedSquares.Contains((int)Board.Coordinate.g8))
                                return true;
                        }
                        break; 

                    case CastlingType.QSBlack:
                        if (Board.Grid[(int)Board.Coordinate.d8] == Pieces.Empty && Board.Grid[(int)Board.Coordinate.c8] == Pieces.Empty)
                        {
                            if (!AttackedSquares.Contains((int)Board.Coordinate.d8) && !AttackedSquares.Contains((int)Board.Coordinate.d8))
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
            public readonly CastlingType? castling;
            public readonly int? promotionPieceValue;
            public readonly bool? en_passant;
            public Move(int originSquare, int targetSquare, CastlingType? castling = null, int? promotionPieceValue = null, bool? en_passant = null)
            {
                this.originSquare = originSquare;
                this.targetSquare = targetSquare;
                this.castling = castling;
                this.promotionPieceValue = promotionPieceValue;
                this.en_passant = en_passant;
            }
        }
        public static void MakeMove(Move move)
        {
            switch (move.castling)
            {
                case null:
                    if(move.promotionPieceValue != null)
                    {
                        Board.Grid[move.targetSquare] = (int) move.promotionPieceValue;
                        Board.Grid[move.originSquare] = Pieces.Empty;
                    }
                    else if(move.en_passant != null)
                    {
                        Board.Grid[move.targetSquare] = Board.Grid[move.originSquare];
                        Board.Grid[move.originSquare] = Pieces.Empty;
                        int squareBehind = Board.SideToMove == Pieces.White ? -16 : 16;
                        Board.Grid[move.targetSquare + squareBehind] = Pieces.Empty;
                    }
                    else
                    {
                        Board.Grid[move.targetSquare] = Board.Grid[move.originSquare];
                        Board.Grid[move.originSquare] = Pieces.Empty;
                    }
                    break;

                case CastlingType.KSWhite:
                    Board.Grid[move.targetSquare] = Board.Grid[move.originSquare];
                    Board.Grid[move.originSquare] = Pieces.Empty;
                    Board.Grid[(int)Board.Coordinate.h1] = Pieces.Empty;
                    Board.Grid[(int)Board.Coordinate.f1] = Pieces.Rook | Pieces.White;
                    break;

                case CastlingType.QSWhite:
                    Board.Grid[move.targetSquare] = Board.Grid[move.originSquare];
                    Board.Grid[move.originSquare] = Pieces.Empty;
                    Board.Grid[(int)Board.Coordinate.a1] = Pieces.Empty;
                    Board.Grid[(int)Board.Coordinate.d1] = Pieces.Rook | Pieces.White;
                    break;

                case CastlingType.KSBlack:
                    Board.Grid[move.targetSquare] = Board.Grid[move.originSquare];
                    Board.Grid[move.originSquare] = Pieces.Empty;
                    Board.Grid[(int)Board.Coordinate.h8] = Pieces.Empty;
                    Board.Grid[(int)Board.Coordinate.f8] = Pieces.Rook | Pieces.Black;
                    break;

                case CastlingType.QSBlack:
                    Board.Grid[move.targetSquare] = Board.Grid[move.originSquare];
                    Board.Grid[move.originSquare] = Pieces.Empty;
                    Board.Grid[(int)Board.Coordinate.a8] = Pieces.Empty;
                    Board.Grid[(int)Board.Coordinate.d8] = Pieces.Rook | Pieces.Black;
                    break;
            }

        }
        static IEnumerable<Move> GenerateSlidingMoves(int originSquare, int pieceType)
        {
            //get the coresponding dir offsets for piece
            int startIndex = pieceType == Pieces.Bishop ? 4 : 0;
            int endIndex = pieceType == Pieces.Rook ? 4 : 8;
            int pieceMoving = Board.Grid[originSquare];
            int colorOfMovingPiece = Pieces.GetPieceColor(pieceMoving);

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
                    else if (Pieces.GetPieceColor(pieceOnSquare) != colorOfMovingPiece)
                    {
                        yield return new Move(originSquare, targetSquare);
                        break;
                    }
                    //on target square is a piece of the same color
                    else if (Pieces.GetPieceColor(pieceOnSquare) == colorOfMovingPiece)
                        break;
                }
            }
        }
        static IEnumerable<Move> GenerateKingMoves(int originSquare)
        {
            // to able to use function in GetAttackedSquares and not care if squares are attacked or not
            List<int> attackedSquares;
            if (Pieces.GetPieceColor(originSquare) == Board.SideToMove)
                 attackedSquares = GetAttackedSquares();
            else
                attackedSquares = new List<int>();

            int colorOfKing = Pieces.GetPieceColor(Board.Grid[originSquare]);
            foreach (int offset in Pieces.DirOffsets)
            {
                int targetSquare = originSquare + offset;
                if ((targetSquare & 0x88) == 0 && !attackedSquares.Contains(targetSquare))
                {
                    int pieceOnSquare = Board.Grid[targetSquare];
                    if(Pieces.GetPieceColor(pieceOnSquare) != colorOfKing)
                        yield return new Move(originSquare, targetSquare);
                }
            }

            if (!attackedSquares.Contains(originSquare))
            {
                if(Board.SideToMove == Pieces.White)
                {
                    if (CastlingCheck(CastlingType.KSWhite, attackedSquares))
                    {
                        yield return new Move(originSquare,(int) Board.Coordinate.g1, CastlingType.KSWhite);
                        Castling &= (int) ~CastlingType.KSWhite;
                    }
                    if (CastlingCheck(CastlingType.QSWhite, attackedSquares))
                    {
                        yield return new Move(originSquare, (int) Board.Coordinate.c1, CastlingType.QSWhite);
                        Castling &= (int) ~CastlingType.QSWhite;
                    }
                }
                else if(Board.SideToMove == Pieces.Black)
                {
                    if (CastlingCheck(CastlingType.KSBlack, attackedSquares))
                    {
                        yield return new Move(originSquare, (int)Board.Coordinate.g8, CastlingType.KSBlack);
                        Castling &= (int) ~CastlingType.KSBlack;
                    }
                    if (CastlingCheck(CastlingType.QSBlack, attackedSquares))
                    {
                        yield return new Move(originSquare, (int)Board.Coordinate.c8, CastlingType.QSBlack);
                        Castling &= (int) ~CastlingType.QSBlack;
                    }
                }
            }
        }
        static IEnumerable<Move> GenerateKnightMoves(int originSquare)
        {
            int colorOfMovingPiece = Pieces.GetPieceColor(Board.Grid[originSquare]);
            foreach (int offset in Pieces.KnightOffsets)
            {
                int targetSquare = originSquare + offset;
                //if target square is on chessboard and is not a friendly piece
                if ((targetSquare & 0x88) == 0)
                {
                    int pieceOnSquare = Board.Grid[targetSquare];
                    if (Pieces.GetPieceColor(pieceOnSquare) != colorOfMovingPiece)
                        yield return new Move(originSquare, targetSquare);
                }
            }
        }
        static IEnumerable<Move> GeneratePawnMoves(int originSquare)
        {
            int pawnColor = Pieces.GetPieceColor(Board.Grid[originSquare]);
            int dir = pawnColor == Pieces.White ? 1 : -1;
            int rank = Board.GetRank(originSquare);

            int targetSquare, pieceOnTargetSquare;

            if (pawnColor == Pieces.White && rank == 1)
            {
                targetSquare = originSquare + 2 * dir * Pieces.Foward;
                pieceOnTargetSquare = Board.Grid[targetSquare];
                if (pieceOnTargetSquare == Pieces.Empty)
                    yield return new Move(originSquare, targetSquare);
            }
            else if (pawnColor == Pieces.Black && rank == 6)
            {
                targetSquare = originSquare + 2 * dir * Pieces.Foward;
                pieceOnTargetSquare = Board.Grid[targetSquare];
                if(pieceOnTargetSquare == Pieces.Empty)
                    yield return new Move(originSquare, targetSquare);
            }

            if (rank == 6 && pawnColor == Pieces.White)
                for (int pieceValue = 2; pieceValue <= 5; pieceValue++)
                    foreach(Move move in PawnMoves(originSquare, pieceValue))
                        yield return move;

            else if (rank == 1 && pawnColor == Pieces.Black)
                for (int pieceValue = 2; pieceValue <= 5; pieceValue++)
                    foreach (Move move in PawnMoves(originSquare, pieceValue))
                        yield return move;

            else
                foreach(Move move in PawnMoves(originSquare))
                    yield return move;
        }
        static IEnumerable<Move> PawnMoves(int originSquare, int? promotionPieceValue = null)
        {
            int pawnColor = Pieces.GetPieceColor(Board.Grid[originSquare]);
            int dir = pawnColor == Pieces.White ? 1 : -1;

            int targetSquare = originSquare + dir * Pieces.Foward; 
            int pieceOnTargetSquare = Board.Grid[targetSquare];

            if (pieceOnTargetSquare == Pieces.Empty)
                yield return new Move(originSquare, targetSquare, promotionPieceValue: promotionPieceValue | pawnColor);

            foreach (int attackOffset in Pieces.PawnAttacks)
            {
                targetSquare = originSquare + attackOffset * dir;
                pieceOnTargetSquare = Board.Grid[targetSquare];

                int colorOfPiece = Pieces.GetPieceColor(pieceOnTargetSquare);
                if (colorOfPiece == Pieces.GetColorOfOposingSide(pawnColor))
                    yield return new Move(originSquare, targetSquare, promotionPieceValue: promotionPieceValue | pawnColor);
                else if (En_passant != null && targetSquare == (int) En_passant)
                    yield return new Move(originSquare, targetSquare, promotionPieceValue: promotionPieceValue | pawnColor, en_passant: true);
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
                        foreach (Move move in GenerateMovesForSquare(originSquare, color))
                            moves.Add(move);
                }
            }
            return moves;
        }
        public static List<int> GetAttackedSquares()
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
