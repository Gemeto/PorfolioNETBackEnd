using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace PorfolioNETBackEnd.Models;

public class JobExperience
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Link { get; set; }

    [ForeignKey("Image")]
    public int? ImageId { get; set; }
    public virtual Image? Image { get; set; }

    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; }
    public virtual User? User { get; set; } = null!;

    [ForeignKey("Category")]
    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
}

public class JobExperienceValidator : AbstractValidator<JobExperience>
{
    public JobExperienceValidator()
    {
        RuleFor(x => x.Id).NotNull();
        RuleFor(x => x.UserId).NotNull();
        RuleFor(x => x.Title).NotEmpty();

    }
}
