using System.Diagnostics;
// king moves after attack bug
namespace ChessBoardModel
{
    public static class MoveManager
    {
        static List<Pin>? pins = null;
        static List<int> attackedSquares = new();
        public static Board.Coordinate? En_passant { get; set; }
        public enum CastlingType { KSWhite = 8, QSWhite = 4, KSBlack = 2, QSBlack = 1 };
        public static int Castling { get; set; } = 0;
        static bool CastlingCheck(CastlingType type)
        {
            if((Castling & (int) type) != 0)
            {
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
        public readonly struct Pin
        {
            public readonly int pinnedSquare;
            public readonly int dirOffset;

            public Pin(int pinnedSquare, int dirOffset)
            {
                this.pinnedSquare = pinnedSquare;
                this.dirOffset = dirOffset;
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
        static IEnumerable<Move> SlidingMoves(int originSquare, int pieceType, bool calculatingAttackedSquares)
        {
            int[] dirOffsets;

            if (!calculatingAttackedSquares && pins != null && pins.Exists(x => x.pinnedSquare == originSquare))
            {
                Pin pin = pins.Find(x => x.pinnedSquare == originSquare);
                dirOffsets = new int[2] { pin.dirOffset, -pin.dirOffset };
            }
            else
            {
                //get the coresponding dir offsets for piece
                int startIndex = pieceType == Pieces.Bishop ? 4 : 0;
                int endIndex = pieceType == Pieces.Rook ? 4 : 8;
                dirOffsets = Pieces.DirOffsets[startIndex..endIndex];
            }

            int pieceMoving = Board.Grid[originSquare];
            int colorOfMovingPiece = Pieces.GetPieceColor(pieceMoving);

            foreach (int dirOffset in dirOffsets)
            {
                int targetSquare = originSquare + dirOffset;

                while ((targetSquare & 0x88) == 0)
                {
                    int pieceOnTarget = Board.Grid[targetSquare];
                    if (pieceOnTarget == Pieces.Empty)
                    {
                        yield return new Move(originSquare, targetSquare);
                        targetSquare += dirOffset;
                    }
                    //on target square is a piece of opposite color
                    else if (Pieces.GetPieceColor(pieceOnTarget) != colorOfMovingPiece)
                    {
                        // if calculating attackedSquares, king cannot go in the direction of a sliding attack
                        if(calculatingAttackedSquares && Pieces.GetPieceValue(pieceOnTarget) == Pieces.King)
                        {
                            int squareBehindKing = targetSquare + dirOffset;
                            int pieceBehindKing = Board.Grid[squareBehindKing];
                            if (pieceBehindKing == Pieces.Empty)
                                yield return new Move(originSquare, squareBehindKing);
                        }

                        yield return new Move(originSquare, targetSquare);
                        break;
                    }
                    //on target square is a piece of the same color
                    else if (Pieces.GetPieceColor(pieceOnTarget) == colorOfMovingPiece)
                        break;
                }
            }
        }
        static IEnumerable<Move> KingMoves(int originSquare)
        {
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
                    if (CastlingCheck(CastlingType.KSWhite))
                    {
                        yield return new Move(originSquare,(int) Board.Coordinate.g1, CastlingType.KSWhite);
                        Castling &= (int) ~CastlingType.KSWhite;
                    }
                    if (CastlingCheck(CastlingType.QSWhite))
                    {
                        yield return new Move(originSquare, (int) Board.Coordinate.c1, CastlingType.QSWhite);
                        Castling &= (int) ~CastlingType.QSWhite;
                    }
                }
                else if(Board.SideToMove == Pieces.Black)
                {
                    if (CastlingCheck(CastlingType.KSBlack))
                    {
                        yield return new Move(originSquare, (int)Board.Coordinate.g8, CastlingType.KSBlack);
                        Castling &= (int) ~CastlingType.KSBlack;
                    }
                    if (CastlingCheck(CastlingType.QSBlack))
                    {
                        yield return new Move(originSquare, (int)Board.Coordinate.c8, CastlingType.QSBlack);
                        Castling &= (int) ~CastlingType.QSBlack;
                    }
                }
            }
        }
        static IEnumerable<Move> KnightMoves(int originSquare, bool calculatingAttackedSquares)
        {
            if (!calculatingAttackedSquares && pins != null && pins.Exists(x => x.pinnedSquare == originSquare))
                yield break;

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
        static IEnumerable<Move> PawnMoves(int originSquare, bool calculatingAttackedSquares)
        {
            int pawnColor = Pieces.GetPieceColor(Board.Grid[originSquare]);
            int dir = pawnColor == Pieces.White ? 1 : -1;
            int rank = Board.GetRank(originSquare);
            int targetSquare, pieceOnTargetSquare, dirOffsetAbs;
            int[]? pawnAttacks = new int[2] { 15 * dir, 17 * dir };
            bool foward = true;

            /* Pin Management */
            if (!calculatingAttackedSquares && pins != null && pins.Exists(x => x.pinnedSquare == originSquare))
            {
                Pin pin = pins.Find(x => x.pinnedSquare == originSquare);

                dirOffsetAbs = pin.dirOffset < 0 ? -pin.dirOffset : pin.dirOffset;
                if (dirOffsetAbs != 16)
                {
                    foward = false;
                    pawnAttacks = new int[1] { dirOffsetAbs };
                }
                else
                {
                    foward = true;
                    pawnAttacks = null;
                }
            }

            if (pawnColor == Pieces.White)
            {
                if(!calculatingAttackedSquares && foward && rank == 1 )
                {
                    targetSquare = originSquare + 2 * dir * Pieces.Foward;
                    pieceOnTargetSquare = Board.Grid[targetSquare];
                    if (pieceOnTargetSquare == Pieces.Empty)
                        yield return new Move(originSquare, targetSquare);
                }
                else if(rank == 6)
                {
                    for (int pieceValue = 2; pieceValue <= 5; pieceValue++)
                        foreach (Move move in BasicPawnMoves(originSquare, foward, pawnAttacks, calculatingAttackedSquares, promotionPieceValue: pieceValue))
                            yield return move;
                }
                
                if (rank != 6)
                    foreach (Move move in BasicPawnMoves(originSquare, foward, pawnAttacks, calculatingAttackedSquares))
                        yield return move;
            }
            else if (pawnColor == Pieces.Black)
            {
                if(!calculatingAttackedSquares && foward &&  rank == 6)
                {
                    targetSquare = originSquare + 2 * dir * Pieces.Foward;
                    pieceOnTargetSquare = Board.Grid[targetSquare];
                    if(pieceOnTargetSquare == Pieces.Empty)
                        yield return new Move(originSquare, targetSquare);
                }
                else if(rank == 1)
                {
                    for (int pieceValue = 2; pieceValue <= 5; pieceValue++)
                        foreach (Move move in BasicPawnMoves(originSquare, foward, pawnAttacks, calculatingAttackedSquares, promotionPieceValue: pieceValue))
                            yield return move;
                }
                
                if (rank != 1)
                    foreach (Move move in BasicPawnMoves(originSquare, foward, pawnAttacks, calculatingAttackedSquares))
                        yield return move;
            }
        }
        static IEnumerable<Move> BasicPawnMoves(int originSquare, bool foward, int[]? pawnAttacks, bool calculatingAttackedSquares, int? promotionPieceValue = null)
        {
            int pawnColor = Pieces.GetPieceColor(Board.Grid[originSquare]);
            int dir = pawnColor == Pieces.White ? 1 : -1;

            int targetSquare = originSquare + dir * Pieces.Foward; 
            int pieceOnTargetSquare = Board.Grid[targetSquare];

            if (!calculatingAttackedSquares && pieceOnTargetSquare == Pieces.Empty && foward)
                yield return new Move(originSquare, targetSquare, promotionPieceValue: promotionPieceValue | pawnColor);  // TODO: pawnColor could cause problems

            if(pawnAttacks != null)
            {
                foreach (int attackOffset in pawnAttacks)
                {
                    targetSquare = originSquare + attackOffset * dir;
                    pieceOnTargetSquare = Board.Grid[targetSquare];

                    int colorOfPiece = Pieces.GetPieceColor(pieceOnTargetSquare);
                    if (colorOfPiece == Pieces.GetColorOfOtherSide(pawnColor))
                        yield return new Move(originSquare, targetSquare, promotionPieceValue: promotionPieceValue | pawnColor);
                    else if (En_passant != null && targetSquare == (int) En_passant)
                        yield return new Move(originSquare, targetSquare, en_passant: true);
                }
            }
        }
        static IEnumerable<Move> MovesForSquare(int originSquare, int color, bool calculatingAttackedSquares)
        {
            int piece = Board.Grid[originSquare];

            if (piece == (Pieces.Knight | color))
            {
                return KnightMoves(originSquare, calculatingAttackedSquares);
            }
            else if (piece == (Pieces.Queen | color))
            {
                return SlidingMoves(originSquare, Pieces.Queen, calculatingAttackedSquares);
            }
            else if (piece == (Pieces.Bishop | color))
            {
                return SlidingMoves(originSquare, Pieces.Bishop, calculatingAttackedSquares);
            }
            else if (piece == (Pieces.Rook | color))
            {
                return SlidingMoves(originSquare, Pieces.Rook, calculatingAttackedSquares);
            }   
            else if (piece == (Pieces.Pawn | color))
            {
                return PawnMoves(originSquare, calculatingAttackedSquares);
            }
            else if(piece == (Pieces.King | color))
            {
                return KingMoves(originSquare);
            }
            else
            {
                IEnumerable<Move> empty = Enumerable.Empty<Move>();
                return empty;
            }
        }
        static List<Move> GetMovesForColor(int color, bool calculatingAttackedSquares=false)
        {
            List<Move> moves = new();
            pins = !calculatingAttackedSquares ? GetPins() : null;
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 16; file++)
                {
                    int originSquare = rank * 16 + file;

                    // if square is on chessbaord
                    if ((originSquare & 0x88) == 0)
                        foreach (Move move in MovesForSquare(originSquare, color, calculatingAttackedSquares))
                            moves.Add(move);
                }
            }
            return moves;
        }
        static List<int> GetAttackedSquares()
        {
            List<int> attackedSquares = new();
            foreach(Move attackMove in GetMovesForColor(Board.SideWaiting, calculatingAttackedSquares: true))
                attackedSquares.Add(attackMove.targetSquare);
            return attackedSquares;
        }
        public static List<Move> GetLegalMoves()
        {
            attackedSquares = GetAttackedSquares();
            return GetMovesForColor(Board.SideToMove);
        }
        static List<Pin> GetPins()
        {
            List<Pin> pinnedSquares = new();

            foreach (int offset in Pieces.StraightOffsets)
            {
                WorkoutPin(pinnedSquares, offset, Pieces.Rook);
            }

            foreach (int offset in Pieces.DiagonalOffsets)
            {
                WorkoutPin(pinnedSquares, offset, Pieces.Bishop);
            }
            return pinnedSquares;
        }
        static void WorkoutPin(List<Pin> pinnedSquares, int dirOffset, int pinPiece)
        {
            int king = Board.Grid[Board.KingSquare];
            int colorOfKing = Pieces.GetPieceColor(king);
            int pieceOnSquare, colorOfPiece, targetSquare, pieceOnSquareValue;
            bool isBehindPiece = false;
            int? possiblePinSquare;

            targetSquare = Board.KingSquare + dirOffset;
            possiblePinSquare = null;

            while ((targetSquare & 0x88) == 0)
            {
                pieceOnSquare = Board.Grid[targetSquare];
                pieceOnSquareValue = Pieces.GetPieceValue(pieceOnSquare);
                colorOfPiece = Pieces.GetPieceColor(pieceOnSquare);

                if (possiblePinSquare == null && colorOfPiece == Pieces.GetColorOfOtherSide(colorOfKing))
                    break;
                else if (colorOfPiece == colorOfKing && !isBehindPiece)
                    possiblePinSquare = targetSquare;
                else if (colorOfPiece == colorOfKing && isBehindPiece)
                    break;
                else if (possiblePinSquare != null && colorOfPiece == Pieces.GetColorOfOtherSide(colorOfKing))
                {
                    if (pieceOnSquareValue == Pieces.Queen || pieceOnSquareValue == pinPiece)
                    {
                        pinnedSquares.Add(new Pin((int)possiblePinSquare, dirOffset));
                        break;
                    }
                }

                targetSquare += dirOffset;
            }
        }
    }
}
