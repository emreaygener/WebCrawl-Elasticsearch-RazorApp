namespace AqvaTaskRazorApp
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public string Link { get; set; }
    }
    public class Detail
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string[] Article { get; set; }
    }
    public class NewsDetail
    {
        public string Title { get; set; }
        public string Img { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string[] Article { get; set; }
    }
}