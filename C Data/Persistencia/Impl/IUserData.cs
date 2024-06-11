using Data.Model;

namespace Data.Persistencia.Impl;

public interface IUserData
{
    public Task<User> GetById(int id);
    public Task<User> GetByEmail(User user);
    public Task<List<User>> GetAll();
    public Task<User> Register(User user);
    public Task<bool> Update(User user, int id);
    public Task<bool> Delete(int id);
}