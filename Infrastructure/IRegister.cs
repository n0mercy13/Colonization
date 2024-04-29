namespace Codebase.Infrastructure
{
    public interface IRegister<TArgument>
    {
        void Register(TArgument argument);
    }
}
