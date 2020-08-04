using GraphQL;
using GraphQL.Types;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using GraphQL.DataLoader;
using Api.Model;
using System.Threading.Tasks;

namespace Api.Types
{
  

    public class DynamicTypesCreator
    {
      
        IServiceProvider _serviceProvider;

        public DynamicTypesCreator( IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ObjectGraphType CreateUserType()
        {
            var tableType = new ObjectGraphType();
            tableType.Name = "User";

            var columnType = new StringGraphType();

             tableType.Field("col1", columnType, 
                       description: "",
                       resolve: ctx =>
                       {
                           var resolver = (ReadonlyResolveFieldContext)ctx;
                           return ((Row)resolver.Source).Col1;
                       });

            return tableType;
        }
        public ObjectGraphType CreateOrderType()
        {
            var tableType = new ObjectGraphType();
            tableType.Name = "Order";

            var columnType = new StringGraphType();

            tableType.Field("col1", columnType,
                      description: "",
                      resolve: ctx =>
                      {
                          var resolver = (ReadonlyResolveFieldContext)ctx;
                          return ((Row)resolver.Source).Col1;
                      });
            return tableType;
        }



        public ListGraphType AddListObjectType(ObjectGraphType parent, ObjectGraphType listObjectType, Func<IEnumerable<Key>, IEnumerable<Row>> getData)
        {

            var listType = new ListGraphType(listObjectType);

            //parent.Name = "list" + tableModel.Identifier;
            var listMemberName = $"List_{parent.Name}_{listObjectType.Name}";
            parent.FieldAsync(listMemberName, listType,
                resolve: async ctx =>
                {
                    var resultRows = await ResolveRelated(ctx as ReadonlyResolveFieldContext, listMemberName,getData);
                    return resultRows;
                });

            return listType;
        }


        private async Task<IEnumerable<Row>> ResolveRelated(ReadonlyResolveFieldContext ctx, string loaderName,
                                                            Func<IEnumerable<Key>, IEnumerable<Row>> getData)
        {
            // DATA LOADER
            var dataloader = (IDataLoaderContextAccessor)_serviceProvider.GetService(typeof(IDataLoaderContextAccessor));
            var listLoader = dataloader.Context
                              .GetOrAddCollectionBatchLoader<Key, Row>(
                                loaderKey: loaderName,
                                fetchFunc: async (parentKeysList) =>
                                {
                                    var resultRows = getData(parentKeysList);
                                    var lookup = resultRows.ToLookup<Row, Key>(
                                         row =>
                                         {
                                             return row.Key;
                                         });

                                    return lookup;
                                },
                                new KeyComparer() //
                                );

            if (ctx.Source is Row row)
            {
                var ret = await listLoader.LoadAsync(row.Key);
                return  ret;
            }

            var noNestedData = getData(null);
            return noNestedData;
        }

 }
}
