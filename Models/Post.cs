using System.ComponentModel.DataAnnotations;

namespace ProjectMiniBlog.Models;

public class Post
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [MaxLength(256)]
    public string Description { get; set; }
    public DateTime DateCreated { get; set; }
    public string? OwnerId { get; set; }
    public virtual ICollection<PostTag>? PostTags { get; set; }
}

