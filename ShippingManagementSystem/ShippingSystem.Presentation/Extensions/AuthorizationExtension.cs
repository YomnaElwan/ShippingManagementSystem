namespace ShippingSystem.Presentation.Extensions
{
    public static class AuthorizationExtension
    {
       public static IServiceCollection AddPermissionPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>{
                //Account
                options.AddPolicy("Register", policy => policy.RequireClaim("Permission", "Register"));
                //Branches 
                options.AddPolicy("ViewBranches", policy => policy.RequireClaim("Permission", "ViewBranches"));
                options.AddPolicy("ViewBranchDetails", policy => policy.RequireClaim("Permission", "ViewBranchDetails"));
                options.AddPolicy("AddNewBranch", policy => policy.RequireClaim("Permission", "AddNewBranch"));
                options.AddPolicy("EditBranch", policy => policy.RequireClaim("Permission", "EditBranch"));
                options.AddPolicy("DeleteBranch", policy => policy.RequireClaim("Permission", "DeleteBranch"));
                //Cities
                options.AddPolicy("ViewCities", policy => policy.RequireClaim("Permission", "ViewCities"));
                options.AddPolicy("ViewCityDetails", policy => policy.RequireClaim("Permission", "ViewCityDetails"));
                options.AddPolicy("AddNewCity", policy => policy.RequireClaim("Permission", "AddNewCity"));
                options.AddPolicy("EditCity", policy => policy.RequireClaim("Permission", "EditCity"));
                options.AddPolicy("DeleteCity", policy => policy.RequireClaim("Permission", "DeleteCity"));
                //Governorates
                options.AddPolicy("ViewGovernorates", policy => policy.RequireClaim("Permission", "ViewGovernorates"));
                options.AddPolicy("ViewGovernorateDetails", policy => policy.RequireClaim("Permission", "ViewGovernorateDetails"));
                options.AddPolicy("AddNewGovernorate", policy => policy.RequireClaim("Permission", "AddNewGovernorate"));
                options.AddPolicy("EditGovernorate", policy => policy.RequireClaim("Permission", "EditGovernorate"));
                options.AddPolicy("DeleteGovernorate", policy => policy.RequireClaim("Permission", "DeleteGovernorate"));
                //Regions
                options.AddPolicy("ViewRegions", policy => policy.RequireClaim("Permission", "ViewRegions"));
                options.AddPolicy("ViewRegionDetails", policy => policy.RequireClaim("Permission", "ViewRegionDetails"));
                options.AddPolicy("AddNewRegion", policy => policy.RequireClaim("Permission", "AddNewRegion"));
                options.AddPolicy("EditRegion", policy => policy.RequireClaim("Permission", "EditRegion"));
                options.AddPolicy("DeleteRegion", policy => policy.RequireClaim("Permission", "DeleteRegion"));
                //Roles
                options.AddPolicy("ViewRoles", policy => policy.RequireClaim("Permission", "ViewRoles"));
                options.AddPolicy("AddNewRole", policy => policy.RequireClaim("Permission", "AddNewRole"));
                options.AddPolicy("ManageRole", policy => policy.RequireClaim("Permission", "ManageRole"));
                options.AddPolicy("AddClaim", policy => policy.RequireClaim("Permission", "AddClaim"));
                options.AddPolicy("DeleteClaim", policy => policy.RequireClaim("Permission", "DeleteClaim"));
                options.AddPolicy("DeleteRole", policy => policy.RequireClaim("Permission", "DeleteRole"));
                //ShippingTypes
                options.AddPolicy("ViewShippingTypes", policy => policy.RequireClaim("Permission", "ViewShippingTypes"));
                options.AddPolicy("AddNewShippingType", policy => policy.RequireClaim("Permission", "AddNewShippingType"));
                options.AddPolicy("EditShippingType", policy => policy.RequireClaim("Permission", "EditShippingType"));
                options.AddPolicy("DeleteShippingType", policy => policy.RequireClaim("Permission", "DeleteShippingType"));
                //PaymentMethods
                options.AddPolicy("ViewPaymentMethods", policy => policy.RequireClaim("Permission", "ViewPaymentMethods"));
                options.AddPolicy("AddNewPaymentMethod", policy => policy.RequireClaim("Permission", "AddNewPaymentMethod"));
                options.AddPolicy("EditPaymentMethod", policy => policy.RequireClaim("Permission", "EditPaymentMethod"));
                options.AddPolicy("DeletePaymentMethod", policy => policy.RequireClaim("Permission", "DeletePaymentMethod"));
                //WeightSettings
                options.AddPolicy("ViewWeightSettings", policy => policy.RequireClaim("Permission", "ViewWeightSettings"));
                options.AddPolicy("ViewWeightSettingsDetails", policy => policy.RequireClaim("Permission", "ViewWeightSettingsDetails"));
                options.AddPolicy("AddNewWeightSetting", policy => policy.RequireClaim("Permission", "AddNewWeightSetting"));
                options.AddPolicy("EditWeightSetting", policy => policy.RequireClaim("Permission", "EditWeightSetting"));
                options.AddPolicy("DeleteWeightSetting", policy => policy.RequireClaim("Permission", "DeleteWeightSetting"));
                //OrderStatus
                options.AddPolicy("ViewAllOrderStatus", policy => policy.RequireClaim("Permission", "ViewAllOrderStatus"));
                options.AddPolicy("AddOrderStatus", policy => policy.RequireClaim("Permission", "AddOrderStatus"));
                options.AddPolicy("EditOrderStatus", policy => policy.RequireClaim("Permission", "EditOrderStatus"));
                options.AddPolicy("DeleteOrderStatus", policy => policy.RequireClaim("Permission", "DeleteOrderStatus"));
                //Couriers
                options.AddPolicy("ViewCouriers", policy => policy.RequireClaim("Permission", "ViewCouriers"));
                options.AddPolicy("ViewCourierDetails", policy => policy.RequireClaim("Permission", "ViewCourierDetails"));
                options.AddPolicy("AddNewCourier", policy => policy.RequireClaim("Permission", "AddNewCourier"));
                options.AddPolicy("EditCourier", policy => policy.RequireClaim("Permission", "EditCourier"));
                options.AddPolicy("DeleteCourier", policy => policy.RequireClaim("Permission", "DeleteCourier"));
                //Employees
                options.AddPolicy("ViewEmployeeHome", policy => policy.RequireClaim("Permission", "ViewEmployeeHome"));
                options.AddPolicy("ViewEmployees", policy => policy.RequireClaim("Permission", "ViewEmployees"));
                options.AddPolicy("AddNewEmployee", policy => policy.RequireClaim("Permission", "AddNewEmployee"));
                options.AddPolicy("EditEmployee", policy => policy.RequireClaim("Permission", "EditEmployee"));
                options.AddPolicy("DeleteEmployee", policy => policy.RequireClaim("Permission", "DeleteEmployee"));
                //Merchants
                options.AddPolicy("ViewMerchantHome", policy => policy.RequireClaim("Permission", "ViewMerchantHome"));
                options.AddPolicy("ViewMerchants", policy => policy.RequireClaim("Permission", "ViewMerchants"));
                options.AddPolicy("ViewMerchantDetails", policy => policy.RequireClaim("Permission", "ViewMerchantDetails"));
                options.AddPolicy("AddNewMerchant", policy => policy.RequireClaim("Permission", "AddNewMerchant"));
                options.AddPolicy("EditMerchant", policy => policy.RequireClaim("Permission", "EditMerchant"));
                options.AddPolicy("DeleteMerchant", policy => policy.RequireClaim("Permission", "DeleteMerchant"));
                //Orders
                options.AddPolicy("OrdersHome", policy => policy.RequireClaim("Permission", "OrdersHome"));
                options.AddPolicy("AllOrders", policy => policy.RequireClaim("Permission", "AllOrders"));
                options.AddPolicy("AddNewOrder", policy => policy.RequireClaim("Permission", "AddNewOrder"));
                options.AddPolicy("EditOrderStatus", policy => policy.RequireClaim("Permission", "EditOrderStatus"));
                options.AddPolicy("EditOrder", policy => policy.RequireClaim("Permission", "EditOrder"));
                options.AddPolicy("DeleteOrder", policy => policy.RequireClaim("Permission", "DeleteOrder"));
                options.AddPolicy("OrderReport", policy => policy.RequireClaim("Permission", "OrderReport"));


            });
            return services;
        }
    }
}
