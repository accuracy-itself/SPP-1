using System.Reflection;
namespace Faker.Core.Generators 
{
    internal class NumberGenerator: Generator 
    {
         static NumberGenerator()
        {
            var genFunctions = typeof(NumberGenerator).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

            foreach (var func in genFunctions) basicGenerators[func.ReturnType] = () => func.Invoke(null, null);
        }

        private static int Int32() => BitConverter.ToInt32(GetBytes(sizeof(int)));
        private static long Int64() => BitConverter.ToInt64(GetBytes(sizeof(long)));
        private static float Single() => BitConverter.ToSingle(GetBytes(sizeof(float)));
        private static double Double() => BitConverter.ToDouble(GetBytes(sizeof(double)));
    }
}