using Mezl.Result.ExampleApp.Application.Requests;
using Mezl.Result.Reasons;

namespace Mezl.Result.ExampleApp.Application.Repositories;

public class UserRepository
{
    public R<User> GetUserByEmail(string email, CancellationToken _ = default)
    {
        if (email == "bob@test.com")
        {
            return new User("bob bober", "bob", "bob@test.com", 52);
        }

        if (email == "alice@test.com")
        {
            return new User("alice aliceer", "alice", "alice@test.com", 52);
        }

        return Reason.New<ReasonNotFound>();
    }

    public R<User> GetUserById(int id, CancellationToken _ = default)
    {
        if (id == 1)
        {
            return new User("bob bober", "bob", "bob@test.com", 52);
        }

        if (id == 2)
        {
            return new User("alice aliceer", "alice", "alice@test.com", 52);
        }

        return Reason.New<ReasonNotFound>();
    }

    public R<User> GetUserByUserName(string userName, CancellationToken _ = default)
    {
        if (userName == "bob")
        {
            return new User("bob bober", "bob", "bob@test.com", 52);
        }

        if (userName == "alice")
        {
            return new User("alice aliceer", "alice", "alice@test.com", 52);
        }

        return Reason.New<ReasonNotFound>();
    }

    public async Task<R<string>> SaveUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        return "new id";
    }
}