namespace ShippingSystem.Presentation.ViewModels.BranchVM
{
    public class ReadBranchesViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
