using System.Reflection;

namespace Server.WebSockets;

public static class WebSocketExtensions
{
    public static void AddRequestHandlers(this IServiceCollection services)
    {
        IEnumerable<Type> assemblyTypes = Assembly.GetExecutingAssembly().ExportedTypes;
        IEnumerable<Type> handlerTypes = assemblyTypes.Where(type => type.GetTypeInfo().BaseType == typeof(WebSocketRequestHandler));

        foreach (Type handlerType in handlerTypes)
        {
            services.AddTransient(handlerType);
        }
    }
}