using ZavrsniSeminarskiRad.Services.Interfaces;

namespace ZavrsniSeminarskiRad.Services.Implementations
{
    public class SharedService : ISharedService
    {

        public List<int>? GetRandomNumberList(int start, int end)
        {
            if (start > end)
            {
                return null;
            }

            if (end == 0)
            {
                return null;
            }
            Random random = new Random();
            List<int> list = new List<int>();
            while (list.Count() < end)
            {
                var randomNumber = random.Next(start, end);
                var find = list.FirstOrDefault(x => x == randomNumber);
                if (find != null)
                {
                    list.Add(randomNumber);
                }
            }
            return list;
        }


    }
}
