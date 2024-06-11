using AutoMapper;
using Data.Model;
using UniqueTrip.Request;

namespace UniqueTrip.Mapper;

public class ResourceToModel : Profile
{
    public ResourceToModel()
    {
        CreateMap<ClientRequest, Client>();
        CreateMap<ContractRequest, Contract>();
        CreateMap<LoanRequest, Loan>();
        CreateMap<MonthlyScheduleRequest, MonthlySchedule>();
        CreateMap<ScheduleRequest, Schedule>();
        CreateMap<UserRequest, User>();
    }
}