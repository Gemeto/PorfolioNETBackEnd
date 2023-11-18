using System.ComponentModel.DataAnnotations.Schema;

namespace PorfolioWeb.Models;

public partial class WebUser
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
