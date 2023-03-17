//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.PortableExecutable;
//using System.Text;
//using System.Threading.Tasks;
//using Mezl.Result.Reasons;

//namespace Mezl.Result.Tests
//{
//    public class ExampleScenarioTests
//    {
//        private readonly UserService _userService = new();

//        [Fact]
//        public async Task ActivateUser_UserNotExistsTest()
//        {
//            var result = await _userService.ActivateUserAsync(0, CancellationToken.None);

//            Assert.IsType<ReasonNotFound>(result.Reason);
//        }

//        [Fact]
//        public async Task ActivateUser_AlreadyActivated()
//        {
//            var result = await _userService.ActivateUserAsync(2, CancellationToken.None);

//            Assert.IsType<ReasonInvalidOperation>(result.Reason);
//        }

//        [Fact]
//        public async Task ActivateUser_Success()
//        {
//            var result = await _userService.ActivateUserAsync(1, CancellationToken.None);

//            Assert.True(result.IsSuccessful);
//        }

//        [Fact]
//        public async Task ActivateUser_InternalErrorOnSave()
//        {
//            var result = await _userService.ActivateUserAsync(3, CancellationToken.None);

//            Assert.IsType<ReasonInternalError>(result.Reason);
//        }
//    }
//}
