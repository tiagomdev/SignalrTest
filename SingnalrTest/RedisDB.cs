using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingnalrTest
{
    public class RedisDB<T> where T : class
    {
        private IRedisClient GetClient()
        {
            var manager = new RedisManagerPool("localhost:6379");

            return manager.GetClient();
        }

        public IList<T> GetAll()
        {
            using (var client = GetClient())
            {
                var clientObject = client.As<T>();

                return clientObject.GetAll();
            }
        }

        public T Save(T value)
        {
            using(var client = GetClient())
            {
                var clientObject = client.As<T>();

                return clientObject.Store(value);
            }
        }

        public T GetById(object id)
        {
            using (var client = GetClient())
            {
                var clientObject = client.As<T>();

                return clientObject.GetById(id);
            }
        }

        public void DeleteById(object id)
        {
            using (var client = GetClient())
            {
                var clientObject = client.As<T>();

                clientObject.DeleteById(id);
            }
        }

        public void ClearAll()
        {
            using (var client = GetClient())
            {
                var clientObject = client.As<T>();

                clientObject.DeleteAll();
            }
        }
    }
}
