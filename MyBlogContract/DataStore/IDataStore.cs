using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyBlogContract
{
    public interface IStoreModel
    {
        
    }

    public class IDataStoreSessionObject<T>
        where T: class, IStoreModel
    {
        public IDto prev_dto { get; set; }
        public Dictionary<string, T> store { get; set; }
    }

    
    public abstract class IDataStore<T>
        where T : class, IStoreModel
    {
        private  IDataStoreSessionObject<T> _DataStoreSessionObject;
        private string _session_name;
        protected IDataStore()
        {
            _session_name = typeof(T).Name;
            _DataStoreSessionObject = new IDataStoreSessionObject<T>();
            _DataStoreSessionObject.store = HttpContext.Current.Session[_session_name] as Dictionary<string, T>;
            if (_DataStoreSessionObject.store == null)
            {
                _DataStoreSessionObject.store = new Dictionary<string, T>();
            }

        }

        protected virtual void Save()
        {
            HttpContext.Current.Session[_session_name]  = _DataStoreSessionObject.store;
        }

        public virtual void SetPrevDto(IDto Dto)
        {
            _DataStoreSessionObject.prev_dto = Dto;
            Save();
        }
        public virtual IDto GetPrevDto()
        {
            return _DataStoreSessionObject.prev_dto;
        }

        public T GetModelByKey(string Key)
        {
            T result = null;
            _DataStoreSessionObject.store.TryGetValue(Key, out result);
            return result;
        }

        public void SetModelByKey(string Key, T Object)
        {
            if (_DataStoreSessionObject.store.ContainsKey(Key))
            {
                _DataStoreSessionObject.store.Remove(Key);
            }
            _DataStoreSessionObject.store.Add(Key, Object);
        }
    }
    
}
