namespace photo_add.Model
{
    public class FileModel
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }    
        public string? ContentType { get; set; }
        public Stream? Content { get; set; }

    }
}
