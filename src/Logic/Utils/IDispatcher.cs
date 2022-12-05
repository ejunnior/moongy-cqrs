namespace Logic.Utils;

public interface IDispatcher
{
    void Dispatch<T>(T args) where T : ICommand;
}