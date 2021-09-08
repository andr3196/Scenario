using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Domain.Modeling.Attributes;
using Scenario.Domain.Modeling.Models;
using Scenario.Domain.Modeling.Models.Constants;
using Scenario.Domain.Modeling.Models.Filters;
using Scenario.Domain.Modeling.Models.Logicals;
using Scenario.Domain.Modeling.Services.Translation;
using Scenario.Domain.SharedTypes;

namespace Scenario.Domain.Modeling.Services
{
    public class ScenarioDomainService : IScenarioDomainService 
    {
        private const string ConsequenceType = "consequence";
        private const string EntityType = "entity";
        private const string PropertyType = "property";
        private const string ParameterType = "parameter";
        private const string LogicalType = "logical";
        private const string FilterType = "filter";
        private readonly IServiceProvider serviceProvider;
        private readonly AssemblyProvider assemblyProvider;
        private readonly IGetEventsStrategy getEventsStrategy;
        private readonly ITranslationService translationService;
        private readonly IDomainTypeResolver domainTypeResolver;

        public ScenarioDomainService(
            IServiceProvider serviceProvider,
            AssemblyProvider assemblyProvider,
            IGetEventsStrategy getEventsStrategy,
            ITranslationService translationService,
            IDomainTypeResolver domainTypeResolver)
        {
            this.serviceProvider = serviceProvider;
            this.assemblyProvider = assemblyProvider;
            this.getEventsStrategy = getEventsStrategy;
            this.translationService = translationService;
            this.domainTypeResolver = domainTypeResolver;
        }

        public ScenarioSetup GetScenarioSetup()
        {
            var assembly = assemblyProvider();

            var scenarioTypes = assembly
                .GetTypes()
                .Where(t => Attribute.IsDefined(t, typeof(ScenarioEnabledAttribute)))
                .ToList();

            var entitiesTypes = scenarioTypes
                .Select(t => new
                {
                    EntityType = t,
                    Attribute = t.GetCustomAttributes(true).OfType<ScenarioEnabledAttribute>().First(),
                })
                .ToList();

            var entities = entitiesTypes
                .Select(x => new Entity
                {
                    Label = x.Attribute.Label ?? x.EntityType.Name,
                    Type = EntityType,
                    Value = domainTypeResolver.GetKey(x.EntityType),
                    Properties = x.EntityType
                        .GetProperties()
                        .Where(p => !p.GetCustomAttributes(true).OfType<ScenarioIgnore>().Any())
                        .Select(p => new Property
                        {
                            Label = p.Name,
                            Type = PropertyType,
                            Value = p.Name
                        })
                        .ToList()
                })
                .ToList();
            var eventsDictionary = entitiesTypes.ToDictionary(
                e => domainTypeResolver.GetKey(e.EntityType),
                e => getEventsStrategy.GetEvents(e.EntityType));

            var consequences = assembly
                .GetTypes()
                .Where(t => Attribute.IsDefined(t, typeof(ScenarioConsequenceAttribute)))
                .Select(t => new
                {
                    HandlerType = t,
                    Attribute = t.GetCustomAttributes(true).OfType<ScenarioConsequenceAttribute>().First(),
                })
                .Select(x => new Consequence
                {
                    Label = x.Attribute.Label,
                    Type = ConsequenceType,
                    Value = domainTypeResolver.GetKey(x.HandlerType),
                    CommandType = domainTypeResolver.GetKey(x.Attribute.ParametersType),
                    HandlerType = domainTypeResolver.GetKey(x.HandlerType),
                    Parameters = x.Attribute.ParametersType
                        .GetProperties()
                        .Where(p => !p.GetCustomAttributes(true).OfType<ScenarioIgnore>().Any())
                        .Select(p => new Parameter
                        {
                            Label = p.Name,
                            Type = ParameterType,
                            Value = domainTypeResolver.GetKey(p.PropertyType),
                        })
                        .ToList(),
                })
                .ToList();

            var setup = new ScenarioSetup
            {
                Entities = entities,
                EventsDictionary = eventsDictionary,
                Constants = GetConstants(),
                FilterDictionary = GetFilterDictionary(),
                Logicals = GetLogicals(),
                Consequences = consequences,
            };

            return setup;
        }

        private Dictionary<string, IEnumerable<Filter>> GetFilterDictionary()
        {
            var filters = serviceProvider.GetServices<IFilter>();
            var interfaceName = typeof(IFilter<,>).Name;
            return filters
                .ToDictionary(
                f => string.Join("-", f.GetType().GetInterface(interfaceName).GenericTypeArguments.Select(t => t.Name)),
                f => f.SupportedComparisonsKeys.Select(k => new Filter
                {
                    Label = translationService.Translate(k),
                    Value = k,
                    Type = FilterType
                }));
        }

        private List<Logical> GetLogicals()
        {
            var logicals = serviceProvider
                .GetServices<ILogical>();
            return logicals.Select(l => new Logical
            {
                Label = translationService.Translate(l.Key),
                Value = l.Key,
                Type = LogicalType,
            }).ToList();
        }

        private List<Constant> GetConstants()
        {
            var constants = serviceProvider.GetServices<IConstant>();
            return constants
                .Where(c => c.GetType().GenericTypeArguments.Length == 1)
                .Select(c => new Constant
                {
                    Label = translationService.Translate(c.Key),
                    Type = c.GetType().GenericTypeArguments.First().Name,
                    Value = c.Key,
                })
                .ToList();
        }
    }

}
