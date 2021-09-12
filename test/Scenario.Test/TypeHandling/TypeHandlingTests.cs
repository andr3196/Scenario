using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using Scenario.Domain.Shared.TypeHandling;
using Scenario.Domain.TypeHandling;
using Scenario.Test.TypeHandling.Mocks;
using Xunit;

namespace Scenario.Test.TypeHandling
{
    public class TypeHandlingTests
    {
        [Theory]
        [MemberData(nameof(GetClassTestScenarios))]
        public void ShouldRegisterAndResolveClass(Type typeThatExistInDomain, Type? typeThatCanBeResolved)
        {
            // Arrange a simple strategy for generating keys
            var keyGeneratorMock = new Mock<ITypeKeyGenerator>();
            var i = 1;
            keyGeneratorMock.Setup(g => g.GenerateKey(It.IsAny<Type>())).Returns(() => "otherkey" + i++);
            keyGeneratorMock.Setup(g => g.GenerateKey(typeThatExistInDomain)).Returns("key1");

            // Mock assembly that has the type
            var assemblyMock = new Mock<Assembly>();
            assemblyMock.Setup(assembly => assembly.GetTypes()).Returns(new Type[] { typeThatExistInDomain });

            var handler = new DomainTypeResolver(keyGeneratorMock.Object);

            // Act - register type and resolve
            handler.RegisterAllTypesFromAssembly(assemblyMock.Object);
            var resolvedType = handler.ResolveType("key1");

            Assert.Equal(typeThatCanBeResolved, resolvedType);
        }

        public static IEnumerable<object[]> GetClassTestScenarios()
        {
            return new List<object[]>
            {
                // A regular class should be resolved
                new object[] { typeof(MockClass1), typeof(MockClass1) },

                // A abstract class should not be resolved
                new object[] { typeof(MockAbstractClass), null },

                // A generic class should not be resolved
                new object[] { typeof(MockGenericClass<>), null },

                // An interface should not be resolved
                new object[] { typeof(MockInterface), null }
            };
        }


        [Theory]
        [MemberData(nameof(GetPropertyTestScenarios))]
        public void ShouldRegisterAndResolveProperties(Type classTypeInDomain, Type propertyTypeOnClass, Type? typeThatCanBeResolved)
        {
            // Arrange a simple strategy for generating keys
            var keyGeneratorMock = new Mock<ITypeKeyGenerator>();
            var i = 1;
            keyGeneratorMock.Setup(g => g.GenerateKey(It.IsAny<Type>())).Returns(() => "otherkey" + i++);
            keyGeneratorMock.Setup(g => g.GenerateKey(classTypeInDomain)).Returns("key1");
            keyGeneratorMock.Setup(g => g.GenerateKey(propertyTypeOnClass)).Returns("key2");

            // Mock assembly that has the type
            var assemblyMock = new Mock<Assembly>();
            assemblyMock.Setup(assembly => assembly.GetTypes()).Returns(new Type[] { classTypeInDomain });

            var handler = new DomainTypeResolver(keyGeneratorMock.Object);

            // Act - register type and resolve
            handler.RegisterAllTypesFromAssembly(assemblyMock.Object);
            var resolvedType = handler.ResolveType("key2");

            Assert.Equal(typeThatCanBeResolved, resolvedType);
        }

        public static IEnumerable<object[]> GetPropertyTestScenarios()
        {
            return new List<object[]>
            {
                // public properties can be resolved
                new object[] { typeof(MockClass1), typeof(ClassPropertyType), typeof(ClassPropertyType)},
                new object[] { typeof(MockAbstractClass), typeof(AbstractClassPropertyType), typeof(AbstractClassPropertyType) },

                // private properties cannot be resolved
                new object[] { typeof(MockClass1), typeof(ClassPropertyType2), null},
                new object[] { typeof(MockAbstractClass), typeof(AbstractClassPropertyType2), null},

                // Fields should not be resolved
                new object[] { typeof(MockClass1), typeof(ClassFieldType), null },
                new object[] { typeof(MockAbstractClass), typeof(AbstractClassFieldType), null },
            };
        }


        [Theory]
        [MemberData(nameof(GetEventTestScenarios))]
        public void ShouldRegisterAndResolveEvents(Type classTypeInDomain, Type eventType, Type? typeThatCanBeResolved)
        {
            // Arrange a simple strategy for generating keys
            var keyGeneratorMock = new Mock<ITypeKeyGenerator>();
            var i = 1;
            keyGeneratorMock.Setup(g => g.GenerateKey(It.IsAny<Type>())).Returns(() => "otherkey" + i++);
            keyGeneratorMock.Setup(g => g.GenerateKey(classTypeInDomain)).Returns("key1");
            keyGeneratorMock.Setup(g => g.GenerateKey(eventType)).Returns("key2");

            // Mock assembly that has the type
            var assemblyMock = new Mock<Assembly>();
            assemblyMock.Setup(assembly => assembly.GetTypes()).Returns(new Type[] { classTypeInDomain });

            var handler = new DomainTypeResolver(keyGeneratorMock.Object);

            // Act - register type and resolve
            handler.RegisterAllTypesFromAssembly(assemblyMock.Object);
            var resolvedType = handler.ResolveType("key2");

            Assert.Equal(typeThatCanBeResolved, resolvedType);
        }

        public static IEnumerable<object[]> GetEventTestScenarios()
        {
            return new List<object[]>
            {
                // public properties methods with attribute can be resolved
                new object[] { typeof(MockClass1), typeof(EventType1), typeof(EventType1)},
                new object[] { typeof(MockAbstractClass), typeof(EventType1), typeof(EventType1) },

                // private methods with attribute cannot be resolved
                new object[] { typeof(MockClass1), typeof(EventType2), null},
                new object[] { typeof(MockAbstractClass), typeof(EventType2), null},

                // Generic methods with attribute can be resolved
                new object[] { typeof(MockClass1), typeof(EventType3), typeof(EventType3) },
                new object[] { typeof(MockAbstractClass), typeof(EventType3), typeof(EventType3) },
            };
        }

    }
}
