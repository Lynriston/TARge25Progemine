using TARge25Shop.Models.Spaceship;

namespace TARge25Shop.Models.RealEstate
{
    public class RealestateCreateUpdateViewModel
    {
        public Guid? Id { get; set; }
        public double? Area { get; set; }
        public string? Location { get; set; }
        public int RoomNumber { get; set; }
        public string? BuildingType { get; set; }
        public List<IFormFile>? Files { get; set; }
        public IEnumerable<RealestateImageViewModel> Image { get; set; } = new List<RealestateImageViewModel>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        
    }
}
