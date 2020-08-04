using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Model
{
    public class UsersStore
    {
        public static  IEnumerable<Row> GetUsers(IEnumerable<Key> keys)
        {

            return new List<Row> { new Row { Key = new Key { Value = "1" },
                                            Col1 = "User1"} };
        }
    }
}
