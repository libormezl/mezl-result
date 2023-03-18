using Mezl.Result.ExampleApp.Application.Repositories;
using Mezl.Result.Extensions;
using Mezl.Result.Handler;
using Mezl.Result.Reasons;

namespace Mezl.Result.ExampleApp.Application.Requests
{
    public record UserByIdResponse(string Name, string UserName, string Email, int Age);
    public record GetUserByIdRequest(int Id) : IRequest<UserByIdResponse>;

    public class UserByIdValidator : IAsyncValidator<GetUserByIdRequest>
    {
        // todo: create sync validator

        public async Task<R> ValidateAsync(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            var validationReason = ValidationContext.New();
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
