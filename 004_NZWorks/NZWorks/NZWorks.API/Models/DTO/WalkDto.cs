using NZWorks.API.Models.Domain;

namespace NZWorks.API.Models.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInks { get; set; }
        public string? WalkImageUrl { get; set; }

        // Navigation properties
        public DifficultyDto Difficulty { get; set; }
        public RegionDTO Region { get; set; }
    }
}
