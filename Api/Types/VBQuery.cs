using System;
using System.Collections.Generic;
using Api.Model;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Types
{
    public class VBQuery : ObjectGraphType
    {
        private IServiceProvider _serviceProvider { get; }

        ObjectGraphType ProductType;

        public VBQuery(IServiceProvider serviceProvider )
        {
            _serviceProvider = serviceProvider;
            Name = "Query";

            var dynamicTypeCreator = new DynamicTypesCreator(serviceProvider);

            var usersType = dynamicTypeCreator.CreateUserType();
            var ordersType = dynamicTypeCreator.CreateOrderType();

            // root linkage for users
            dynamicTypeCreator.AddListObjectType(this, usersType, UsersStore.GetUsers);

            //  users has a list of orders
            dynamicTypeCreator.AddListObjectType(usersType, ordersType, OrdersStore.GetOrders);
            


        }

    }
}
