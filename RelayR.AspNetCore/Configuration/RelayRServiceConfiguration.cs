using System.Reflection;

namespace RelayR.AspNetCore.Configuration;

public class RelayRServiceConfiguration
{
    internal List<Assembly> AssembliesToRegister { get; } = [];

    public RelayRServiceConfiguration RegisterServicesFromAssemblyContaining<T>()
    => RegisterServicesFromAssemblyContaining(typeof(T));

    public RelayRServiceConfiguration RegisterServicesFromAssemblyContaining(Type type)
    => RegisterServicesFromAssembly(type.Assembly);

    public RelayRServiceConfiguration RegisterServicesFromAssembly(Assembly assembly)
    {
        AssembliesToRegister.Add(assembly);
        return this;
    }
}