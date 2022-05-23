using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace Scenario.Api.Test.Extensions
{
    public static class GenericCollectionAssertionExtensions
    {
        public static void Fulfill<T>(this GenericCollectionAssertions<T> assertion, params 
            Func<T, List<Action>>[] requirementsPerElement)
        {
            Assert.NotNull(assertion.Subject);
            Assert.Equal(requirementsPerElement.Length, assertion.Subject.Count());
            foreach (var (element,requirementsForElement, index) in assertion.Subject.Zip(requirementsPerElement, Enumerable.Range(0,requirementsPerElement.Length)))
            {
                var requirements = requirementsForElement(element);
                foreach (var (requirement, subindex) in requirements.Zip(Enumerable.Range(0,requirements.Count)))
                {
                    try
                    {
                        requirement();
                    }
                    catch(Exception e)
                    {
                        throw new AssertActualExpectedException("To not fail", "but failed",
                            $"Assertion failed for requirement number {subindex + 1} for element number {index + 1}",
                            "",
                            "",
                            e);
                    }
                }
            }
        }
    }
}