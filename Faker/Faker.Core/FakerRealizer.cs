using Faker.Core.Interfaces;
using Faker.Core.Generators;
using System.Collections;
using System.Reflection;

namespace Faker.Core
{
    public class FakerRealizer : IFaker
    {
        public T Create<T>() => (T)Create(typeof(T));
        private List<Type> dependences = new();

        private object Create(Type type)
        {
            //
            if (Generator.BasicGenerators.ContainsKey(type)) return Generator.BasicGenerators[type]();
            //comm
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var listType = Activator.CreateInstance(type) as IList;
                    listType.Add(Create(type.GetGenericArguments()[0]));
                    return listType;
                }
            }
            if (dependences.Contains(type)) throw new Exception("!!cycle exception!!");
            dependences.Add(type);

            object result = CreateComplexObject(type);

            dependences.Remove(type);
            return result;
        }

        private object CreateComplexObject(Type type)
        {
            var constructor = type.GetConstructors().MaxBy(x => x.GetParameters().Length);
            var parameters = constructor.GetParameters();
            var publicSetters = type.GetProperties().Where(x => x.CanWrite && x.SetMethod.IsPublic);
            IEnumerable<FieldInfo> publicFields = type.GetFields().Where(x => x.IsPublic);

            publicSetters = publicSetters.Where(x => !parameters.Any(y => x.Name == "set_" + y.Name));
            publicFields = publicFields.Where(x => !parameters.Any(y => y.Name == x.Name));

            List<object> objparameters = new();
            foreach (var parameter in constructor.GetParameters())
            {
                objparameters.Add(Create(parameter.ParameterType));
            }
            object result = constructor.Invoke(objparameters.ToArray());

            foreach (var field in publicFields)
                field.SetValue(result, Create(field.FieldType));
            foreach (var property in publicSetters)
                property.SetValue(result, Create(property.PropertyType));
            return result;
        }
    }
}

 