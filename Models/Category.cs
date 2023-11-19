using FluentValidation;

namespace PorfolioNETBackEnd.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ParentId { get; set; }

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual ICollection<JobExperience> JobExperiences { get; set; } = new List<JobExperience>();

    public virtual Category? Parent { get; set; }
}
    public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Id).NotNull();
        RuleFor(x => x.Name).NotEmpty();

    }
}
