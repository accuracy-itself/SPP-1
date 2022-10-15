using System.Reflection;
namespace Faker.Core.Generators
{
    internal sealed class StringGenerator : Generator
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        static StringGenerator()
        {
            var genFunctions = typeof(StringGenerator).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

            foreach (var func in genFunctions) basicGenerators[func.ReturnType] = () => func.Invoke(null, null);
        }

        private static string String() => new(Enumerable.Repeat(chars, random.Next(0, 20)).Select(s => s[random.Next(s.Length)]).ToArray());
        private static char Char() => chars[random.Next(chars.Length)];
    }
}
