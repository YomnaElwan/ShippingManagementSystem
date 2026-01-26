using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels.CityVM
{
    public class ReadCityViewModel
    {
        public int Id { get; set; }
        public string? CityName { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal PickupCost { get; set; }
        public string? GovernorateName { get; set; }
    }
}
