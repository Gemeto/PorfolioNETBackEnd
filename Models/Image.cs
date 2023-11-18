using PorfolioWeb.Models.Interfaces;

namespace PorfolioWeb.Models;

public partial class Image : IFileModel
{
    public int Id { get; set; }

    public string Path { get; set; } = null!;

    public virtual ICollection<JobExperience> JobExperiences { get; set; } = new List<JobExperience>();

    public virtual ICollection<WebUser> Users { get; set; } = new List<WebUser>();
}
