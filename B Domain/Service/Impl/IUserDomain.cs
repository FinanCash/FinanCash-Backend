using Data.Model;

namespace Domain.Service.Impl;

public interface IUserDomain
{
    public Task<User> GetById(int id);
    public Task<User> GetByEmail(User user);
    public Task<List<User>> GetAll();
    public Task<string> Login(User user);
    public Task<User> Register(User user);
    public Task<bool> Update(User user, int id);
    public Task<bool> Delete(int id);
}