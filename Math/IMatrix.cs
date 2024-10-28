
public interface IMatrix
{
    public int RowsCount { get; }
    public int ColumnsCount { get; }
    public List<List<double>> Rows { get; }
}