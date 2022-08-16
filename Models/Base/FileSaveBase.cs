namespace ZavrsniSeminarskiRad.Models.Base
{
    public abstract class FileSaveBase
    {
        public string? PhysicalPath { get; set; }
        public string? DownloadUrl { get; set; }
        public string? FileExtension { get; set; }
        public string? FileName { get; set; }
    }
}
