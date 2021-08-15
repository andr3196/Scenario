using System;
namespace Scenario.ErrorHandling
{
    public class TypeResolutionException : Exception
    {
        

        public TypeResolutionException(string assemblyQualifiedName)
            :base(Describe(assemblyQualifiedName))
        {
        }

        public TypeResolutionException(string assemblyQualifiedName, Exception innerException)
            :base(Describe(assemblyQualifiedName), innerException)
        {

        }

        private static string Describe(string assemblyQualifiedName)
        {
            return $"Type with name '{assemblyQualifiedName}' could not be resolved";
        }
    }
}
