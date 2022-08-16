namespace ZavrsniSeminarskiRad.Models.Binding.Interfaces
{
    public interface IItem
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int ItemCategoryId { get; set; }
    }
}
