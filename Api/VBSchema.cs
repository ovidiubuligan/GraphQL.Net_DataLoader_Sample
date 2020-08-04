using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using Api.Types;
namespace Api
{
    public class VBSchema : Schema
    {
        public VBSchema(IServiceProvider provider)
            : base(provider)
        {
            Query = provider.GetRequiredService<VBQuery>();
            //Mutation = provider.GetRequiredService<StarWarsMutation>();
        }
    }
}
