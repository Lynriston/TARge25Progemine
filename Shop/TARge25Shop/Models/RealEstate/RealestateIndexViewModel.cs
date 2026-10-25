namespace TARge25Shop.Models.RealEstate
{
    public class RealestateIndexViewModel
    {

        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public int? Area { get; set; }
        public string? Location { get; set; }
        public int RoomNumber { get; set; }
        public string? BuildingType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
