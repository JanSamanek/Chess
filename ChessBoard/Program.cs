using ChessBoardModel;

Board.setEmpty();
Board.setPieces(Board.startPos);
Board.Grid[(int) Board.Coordinates.d5] = 11;
Board.Grid[51] = 11;
Board.show();