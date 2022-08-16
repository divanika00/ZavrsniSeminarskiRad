using System.Net.Mime;
using ZavrsniSeminarskiRad.Models.Base;

namespace ZavrsniSeminarskiRad.Models.ViewModels
{
    public class FileSaveViewModel : FileSaveBase
    {
        public int Id { get; set; }
    }


    public class FileSaveExpendedViewModel : FileSaveBase
    {
        public int Id { get; set; }
        public string Base64 { get; set; }
        public FileStream FileStream { get; set; }
        public ContentDisposition ContentDisposition { get; set; }
    }
}
