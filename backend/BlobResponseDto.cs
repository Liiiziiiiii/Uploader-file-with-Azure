using photo_add.Model;
using System.Reflection.Metadata;

namespace photo_add
{
    public class BlobResponseDto
    {
        public BlobResponseDto()
        { 
            Blob = new FileModel();
        }

        public string? Status { get; set; }
        public bool Error { get; set; }

        public FileModel Blob { get; set; }
    }
}
