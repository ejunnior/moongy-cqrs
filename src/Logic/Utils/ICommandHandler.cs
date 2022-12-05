namespace Logic.Utils;

using System.Threading.Tasks;

public interface ICommandHandler<T>
    where T : ICommand
{
    void Handle(T args);
}