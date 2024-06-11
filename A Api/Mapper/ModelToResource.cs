using AutoMapper;
using Data.Model;
using UniqueTrip.Request;
using UniqueTrip.Response;

namespace UniqueTrip.Mapper;

public class ModelToResource : Profile
{
    public ModelToResource()
    {
        //Request
        CreateMap<Client, ClientRequest>();
        CreateMap<Contract, ContractRequest>();
        CreateMap<Loan, LoanRequest>();
        CreateMap<MonthlySchedule, MonthlyScheduleRequest>();
        CreateMap<Schedule, ScheduleRequest>();
        CreateMap<User, UserRequest>();
        //Response
        CreateMap<Client, ClientResponse>();
        CreateMap<Contract, ContractResponse>();
        CreateMap<Loan, LoanResponse>();
        CreateMap<MonthlySchedule, MonthlyScheduleResponse>();
        CreateMap<Schedule, ScheduleResponse>();
        CreateMap<User, UserResponse>();


    }
    
}