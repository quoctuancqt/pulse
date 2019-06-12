namespace Pulse.FakeData.FakeDB
{
    using Domain;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    //https://romiller.com/2012/02/14/testing-with-a-fake-dbcontext/
    //http://labs.bjfocus.co.uk/2014/07/mocking-async-repository-calls/
    public class FakeDbSet<T> : IDbSet<T>, IDbAsyncEnumerable<T> where T : class, IEntity
    {
        readonly ObservableCollection<T> _data;
        readonly IQueryable _queryable;

        public FakeDbSet()
        {
            _data = new ObservableCollection<T>();
            _queryable = _data.AsQueryable();
        }

        public virtual T Find(params object[] keyValues)
        {
            return _data.SingleOrDefault(d => d.Id == (int)keyValues.Single());
        }

        public Task<T> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return Task.FromResult<T>(Find(keyValues));
        }

        public T Add(T item)
        {
            _data.Add(item);
            return item;
        }

        public void AddRange(IEnumerable<T> original)
        {
            foreach (var item in original)
            {
                _data.Add(item);
            } 
        }

        public T Remove(T item)
        {
            _data.Remove(item);
            return item;
        }

        public T Attach(T item)
        {
            _data.Add(item);
            return item;
        }

        public T Detach(T item)
        {
            _data.Remove(item);
            return item;
        }

        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public ObservableCollection<T> Local
        {
            get { return _data; }
        }

        Type IQueryable.ElementType
        {
            get { return _queryable.ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get { return _queryable.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new AsyncQueryProviderWrapper<T>(_queryable.Provider); }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public int Count
        {
            get { return _data.Count; }
        }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new AsyncEnumeratorWrapper<T>(_data.GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }
    }
}
