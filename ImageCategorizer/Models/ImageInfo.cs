namespace ImageCategorizer.Models
{
    public class ImageInfo
    {
        public string PreviewImage { set; get; }
        public string[] SerieNames { init; get; }
        public string[] Characters { init; get; }
        public string SourceName { init; get; }
        public string Artist { init; get; }
        public string SourceUrl { init; get; }
        public int Rating { init; get; }
        public string[] RatingTags { init; get; }
    }
}
