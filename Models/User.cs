using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace PorfolioNETBackEnd.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Surname { get; set; }

    public string? Description { get; set; }

    [ForeignKey("MainImage")]
    public int? MainImageId { get; set; }

    public virtual Image? MainImage { get; set; }

    public virtual ICollection<JobExperience> JobExperiences { get; set; } = new List<JobExperience>();

}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Id).NotNull();
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();

    }
}