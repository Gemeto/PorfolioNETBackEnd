using FluentValidation;
using PorfolioNETBackEnd.Models.Interfaces;

namespace PorfolioNETBackEnd.Models;

public partial class Image : IFileModel
{
    public int Id { get; set; }

    public string Path { get; set; } = null!;

    public virtual ICollection<JobExperience> JobExperiences { get; set; } = new List<JobExperience>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

    public class ImageValidator : AbstractValidator<Image>
{
    public ImageValidator()
    {
        RuleFor(x => x.Id).NotNull();
        RuleFor(x => x.Path).NotEmpty();

    }
}

