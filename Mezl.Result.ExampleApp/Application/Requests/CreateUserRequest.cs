using Mezl.Result.ExampleApp.Application.Repositories;
using Mezl.Result.Extensions;
using Mezl.Result.Handler;
using Mezl.Result.Reasons;

namespace Mezl.Result.ExampleApp.Application.Requests
{
    public record CreateUserRequest(string Name, string UserName, string Email, int Age) : IRequest<Id>;

    public class CreateUserValidator : IAsyncValidator<CreateUserRequest>
    {
        private readonly UserRepository _userRepository;

        public CreateUserValidator(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<R> ValidateAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var validationContext = ValidationContext.New();

            validationContext.Property(request.Name).NotNull();

            validationContext.Property(request.UserName)
                .NotNull()
                .MaxLength(50)
                .Should(userName => _userRepository.GetUserByUserName(userName).Is<ReasonNotFound>());

            validationContext.Property(request.Email)
                .IsEmail()
                .MaxLength(50)
                .Should(email => _userRepository.GetUserByEmail(email).Is<ReasonNotFound>());

            validationContext.Property(request.Age).InRange(18, 99);

            return validationContext.GetValidationResult();
        }
    }

    public class CreateUserHandler : IAsyncRequestHandler<CreateUserRequest, Id>
    {
        public record UserCreated(string Name, string UserName, string Email, int Age) : INotification;

        private readonly UserRepository _userRepository;
        private readonly IRequestExecutor _requestExecutor;

        public CreateUserHandler(UserRepository userRepository, IRequestExecutor requestExecutor)
        {
            _userRepository = userRepository;
            _requestExecutor = requestExecutor;
        }

        public async Task<R<Id>> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.SaveUserAsync(request, cancellationToken);
            return result.Reason switch
            {
                { } a => a,
                null => Complete()
            };

            Id Complete()
            {
                _requestExecutor.Notify(new UserCreated(request.Name, request.UserName, request.Email, request.Age));
                return new Id(result);
            }

            //if (result.IsNotSuccessful)
            //{
            //    return result.Reason;
            //}

            //_requestExecutor.Notify(new UserCreated(request.Name, request.UserName, request.Email, request.Age));
            //return new Id(result);
        }
    }
}
