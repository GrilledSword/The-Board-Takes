namespace BoardTakes.Core
{
    /// <summary>
    /// Standard chess ranks. Values match classical pawn-units so later
    /// ledger code can reason about "a queen costs more than a knight"
    /// without a second lookup table.
    /// </summary>
    public enum PieceKind
    {
        None = 0,
        Pawn = 1,
        Knight = 3,
        Bishop = 3 + 100, // distinct from knight; value still 3 via ValueOf
        Rook = 5,
        Queen = 9,
        King = 1000
    }

    public static class PieceKindMath
    {
        public static int ValueOf(PieceKind kind) => kind switch
        {
            PieceKind.Pawn => 1,
            PieceKind.Knight => 3,
            PieceKind.Bishop => 3,
            PieceKind.Rook => 5,
            PieceKind.Queen => 9,
            PieceKind.King => 1000,
            _ => 0
        };
    }
}
