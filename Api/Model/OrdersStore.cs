using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Api.Model
{
    public class OrdersStore
    {

        public static IEnumerable<Row> GetOrders(IEnumerable<Key> keys)
        {

            return new List<Row> { new Row { Key = new Key { Value = "1" }, Col1 = "order1"},
                                   new Row { Key = new Key { Value = "1" }, Col1 = "order2"},
                                   new Row { Key = new Key { Value = "2" }, Col1 = "order3"},
                                   new Row { Key = new Key { Value = "3" }, Col1 = "order4"}}
            .Where( row => keys.Any( key => key.Value == row.Key.Value) );
            
        }
    }
}
