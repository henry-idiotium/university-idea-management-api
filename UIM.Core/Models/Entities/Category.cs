namespace UIM.Core.Models.Entities;

public class Category : Entity
{
    public string Name { get; set; } = default!;

    public virtual ICollection<Idea>? Ideas { get; set; }
}