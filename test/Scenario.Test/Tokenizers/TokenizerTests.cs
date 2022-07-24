using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Application.Tokenizers;
using Xunit;

namespace Scenario.Test.Tokenizers
{
    public class TokenizerTests : IClassFixture<WebApplicationFactory<Scenario.Project.Test.Program>>
    {
        private readonly WebApplicationFactory<Project.Test.Program> factory;
        private readonly TokenizeScenarioDefinitionHandler tokenizer;

        public TokenizerTests(WebApplicationFactory<Project.Test.Program> factory)
        {
            this.factory = factory;
            this.tokenizer = new TokenizeScenarioDefinitionHandler();
        }

        [Fact]
        public async Task Tokenizer_ShouldYieldEventId_WhenAccountCreated()
        {
            const string definition = @"WHEN Account CREATED THEN @sendemail { to: Account.Email, title: ""Thank you for joining""}";
            var tokenResult = await tokenizer.Handle(new TokenizeScenarioDefinition(definition), CancellationToken.None);

            tokenResult.Should().NotBeNull();
            tokenResult.EventId.Should().Be("CREATED");
            tokenResult.EventEntity.Should().Be("Account");
        }

    }
}