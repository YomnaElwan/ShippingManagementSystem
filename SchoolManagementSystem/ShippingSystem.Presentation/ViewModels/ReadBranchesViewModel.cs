namespace ShippingSystem.Presentation.ViewModels
{
    public class ReadBranchesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
