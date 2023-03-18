using Mezl.Result.Handler;
using Mezl.Result.Validation;

namespace Mezl.Result.ExampleApp.Application.Requests
{
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
        public Task<R<UserByIdResponse>> HandleAsync(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
