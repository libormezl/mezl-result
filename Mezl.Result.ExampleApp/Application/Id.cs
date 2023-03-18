namespace Mezl.Result.ExampleApp.Application
{
    public class Id
    {
        private readonly string _value;

        public Id(string value)
        {
            _value = value;
        }

        public static implicit operator string(Id d)
        {
            return d._value;
        }

        public static implicit operator Id(string d)
        {
            return new Id(d);
        }
    }
}
