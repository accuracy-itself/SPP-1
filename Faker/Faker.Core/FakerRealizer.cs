using Faker.Core.Generators;
using System.Collections;
using System.Reflection;

namespace Faker.Core
{
    public class FakerRealizer
    {
        public T Create<T>() => (T)Create(typeof(T));
        private List<Type> dependences = new();

        private object Create(Type type)
        {
            //
            if (Generator.BasicGenerators.ContainsKey(type)) return Generator.BasicGenerators[type]();
            //comment
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
            /*
            var constructors = new List<ConstructorInfo>(type.GetConstructors());
            int l = constructors.Count;
            for (int i = 0; i < l; i++)
            {
                var constructor = constructors.MaxBy(x => x.GetParameters().Length);
                try
                {
                    var parameters = constructor.GetParameters();
                    var setters = type.GetProperties().Where(x => x.CanWrite && x.SetMethod.IsPublic);
                    IEnumerable<FieldInfo> fields = type.GetFields().Where(x => x.IsPublic);

                    //comment
                    {
                        /*
                        //publicSetters = publicSetters.Where(x => !parameters.Any(y => x.Name == "set_" + y.Name));
                        //publicFields = publicFields.Where(x => !parameters.Any(y => y.Name == x.Name));
                        
                    }

                    List<object> objparameters = new();
                    foreach (var parameter in constructor.GetParameters())
                    {
                        objparameters.Add(Create(parameter.ParameterType));
                    }
                    object result = constructor.Invoke(objparameters.ToArray());

                    foreach (var field in fields)
                        field.SetValue(result, Create(field.FieldType));
                    foreach (var property in setters)
                        property.SetValue(result, Create(property.PropertyType));
                    return result;
                }
                catch 
                {
                    
                }
                constructors.RemoveAt(constructors.IndexOf(constructor));


            }
        */
            //throw new Exception("!!no normal constructors available!!");

            var constructors = new List<ConstructorInfo>(type.GetConstructors());

            int l = constructors.Count;
            for (int i = 0; i < l; i++)
            {
                var constructor = constructors.MaxBy(x => x.GetParameters().Length);
                constructors.RemoveAt(constructors.IndexOf(constructor));
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
            throw new Exception("!!no normal constructors available!!");
            return null;
        }
    }
}

 