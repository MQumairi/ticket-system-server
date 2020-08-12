namespace API.Models
{
    public class Attachment : Photo
    {
        //Relationship with Post entity
        public Post post { get; set; }
    }
}