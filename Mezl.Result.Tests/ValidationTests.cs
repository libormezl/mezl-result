using Mezl.Result.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mezl.Result.Validation;

namespace Mezl.Result.Tests
{
    public class ValidationTests
    {
        private record TestClass(string UserName, string Email, int Age);

        [Fact]
        public void Validation()
        {
            var test = new TestClass("testing", "test@test.ct", 16);
            var validationContext = new ValidationContext();
            validationContext.Property(test.Age).InRange(10, 18);

            validationContext.Property(test.UserName).MaxLength(50).MinLength(5);


            var result = validationContext.GetValidationResult();
            Assert.Equal(R.Success, result);
        }
    }
}
