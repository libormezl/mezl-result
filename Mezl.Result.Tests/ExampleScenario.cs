//using Mezl.Result.Reasons;

//namespace Mezl.Result.Tests
//{
//    public class User
//    {
//        public int Id { get; set; }

//        public string Name { get; set; }

//        public string Email { get; set; }

//        public bool Activated { get; set; }
//    }

//    public class ExampleScenario
//    {
//#pragma warning disable CS1998
//        public async Task<Result<User>> GetUserByIdAsync(int id)
//#pragma warning restore CS1998
//        {
//            if (id == 0)
//            {
//                return Reason.New<ReasonNotFound>()
//                    .WithMessage("User not found")
//                    .AddInfo($"id:{id}");
//            }

//            return new User
//            {
//                Id = id,
//                Email = "test@email.com",
//                Name = "Name",
//                Activated = id == 2
//            };
//        }

//        public async Task<Result> SaveUserAsync(User user)
//        {
//            if (user.Id == 3)
//            {
//                return Reason.New<ReasonInternalError>();
//            }

//            return Result.Success;
//        }
//    }

//    public class UserService
//    {
//        private readonly ExampleScenario _userRepository = new();

//        public async Task<Result> ActivateUserAsync(int id, CancellationToken cancellationToken)
//        {
//            var userResult = await _userRepository.GetUserByIdAsync(id);
//            return await userResult
//                .TapCheck(CheckUserIsActivated)
//                .Tap(user => user.Activated = true)
//                .TapCheckAsync(user => _userRepository.SaveUserAsync(user))
//                .TapAsync(user => Console.WriteLine($"Send activation mail to {user.Email}"))
//                .MapAsync();

//        }

//        // How 'ActivateUserAsync' works on background
//        public async Task<Result> ActivateUserAsync2(int id)
//        {
//            var userResult = await _userRepository.GetUserByIdAsync(id);
//            if (userResult.IsNotSuccessful)
//            {
//                return userResult.Reason;
//            }

//            var validationResult = CheckUserIsActivated(userResult);
//            if (validationResult.IsNotSuccessful)
//            {
//                return validationResult.Reason;
//            }

//            var savingResult = await _userRepository.SaveUserAsync(userResult);
//            if (savingResult.IsNotSuccessful)
//            {
//                return savingResult.Reason;
//            }

//            Console.WriteLine($"Send activation mail to {userResult.Value.Email}");
//            return Result.Success;
//        }

//        // logic in method is easy testable separately, smaller unit - better unit
//        internal Result CheckUserIsActivated(User user)
//        {
//            if (user.Activated)
//            {
//                return Reason
//                    .New<ReasonInvalidOperation>()
//                    .WithMessage("Already activated");
//            }

//            return Result.Success;
//        }
//    }
//}