using Mezl.Result.ExampleApp.Application.Repositories;
using Mezl.Result.Extensions;
using Mezl.Result.Handler;
using Mezl.Result.Reasons;
using Mezl.Result.Validation;

namespace Mezl.Result.ExampleApp.Application.Requests;

public record UserByIdResponse(string Name, string UserName, string Email, int Age);
public record GetUserByIdRequest(int Id) : IRequest<UserByIdResponse>;

public class UserByIdValidator : IValidator<GetUserByIdRequest>
{
    public R Validate(GetUserByIdRequest request)
    {
        var validationReason = new ValidationContext();
        validationReason.Property(request.Id).Should(i => i > 0, "Is not valid id");

        return validationReason.GetValidationResult();
    }
}

public class UserByIdHandler : IAsyncRequestHandler<GetUserByIdRequest, UserByIdResponse>
{
    private readonly UserRepository _userRepository;

    public UserByIdHandler(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<R<UserByIdResponse>> HandleAsync(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserById(request.Id, cancellationToken)
            .ReasonIf(user => user == null, new ReasonNotFound())
            .ReasonIfAsync(User => Task.FromResult(User != null), Reason.New<ReasonNotFound>())
            .ThenAsync(user => user with { Age = 11 })
            .ThenAsync(user => new UserByIdResponse(user.Name, user.UserName, user.Email, user.Age));
    }
}