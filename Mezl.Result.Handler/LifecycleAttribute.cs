namespace Mezl.Result.Handler
{
    public enum Lifecycle
    {
        Transient,
        Singleton,
        Scoped
    }

    public class LifecycleAttribute : Attribute
    {
        public Lifecycle Lifecycle { get; }

        public LifecycleAttribute(Lifecycle lifecycle)
        {
            Lifecycle = lifecycle;
        }
    }
}
