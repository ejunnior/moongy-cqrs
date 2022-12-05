namespace Logic.Utils;

using System;
using Microsoft.Extensions.DependencyInjection;

public class Dispatcher : IDispatcher
{
    private readonly IServiceScope _serviceScope;

    public Dispatcher(IServiceProvider serviceProvider)
    {
        _serviceScope = serviceProvider
            .CreateScope();
    }

    public void Dispatch<T>(T args) where T : ICommand
    {
        var handler = _serviceScope
            .ServiceProvider.GetService<ICommandHandler<T>>();

        handler.Handle(args);
    }
}