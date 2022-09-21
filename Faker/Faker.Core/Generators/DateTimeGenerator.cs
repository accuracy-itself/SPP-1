using System.Reflection;

namespace Faker.Core.Generators
{
    internal sealed class DateTimeGenerator : Generator
    {
        static DateTimeGenerator()
        {
            var genFunctions = typeof(DateTimeGenerator).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

            foreach (var func in genFunctions) basicGenerators[func.ReturnType] = () => func.Invoke(null, null);
        }

        private static DateTime GetDateTime() => new(random.NextInt64(DateTime.Today.Ticks));
    }
}
