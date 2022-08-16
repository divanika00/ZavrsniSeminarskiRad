namespace ZavrsniSeminarskiRad.Services.Interfaces
{
    public interface ISharedService
    {
        List<int>? GetRandomNumberList(int start, int end);
    }
}