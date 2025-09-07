using ShippingSystem.Domain.Entities;
namespace ShippingSystem.Domain.Interfaces
{
    public interface IGovernorateRepository:IGenericRepository<Governorates>
    {
        Task<List<Governorates>> regionGovsList(int regionId);
        Task<Governorates> govByIdIncludeRegion(int govId); 
    }
}
