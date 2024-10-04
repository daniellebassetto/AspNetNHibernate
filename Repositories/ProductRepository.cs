using AspNetNHibernate.Models;
using NHibernate;

namespace AspNetNHibernate.Repositories;

public class ProductRepository : IRepository<Product>
{
    private NHibernate.ISession _session;
    public ProductRepository(NHibernate.ISession session) => _session = session;
    public async Task Add(Product item)
    {
        ITransaction transaction = null;
        try
        {
            transaction = _session.BeginTransaction();
            await _session.SaveAsync(item);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            await transaction?.RollbackAsync();
        }
        finally
        {
            transaction?.Dispose();
        }
    }

    public IEnumerable<Product> FindAll() =>
                    _session.Query<Product>().ToList();

    public async Task<Product> FindByID(long id) =>
                    await _session.GetAsync<Product>(id);

    public async Task Remove(long id)
    {
        ITransaction transaction = null;
        try
        {
            transaction = _session.BeginTransaction();
            var item = await _session.GetAsync<Product>(id);
            await _session.DeleteAsync(item);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            await transaction?.RollbackAsync();
        }
        finally
        {
            transaction?.Dispose();
        }
    }

    public async Task Update(Product item)
    {
        ITransaction transaction = null;
        try
        {
            transaction = _session.BeginTransaction();
            await _session.UpdateAsync(item);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            await transaction?.RollbackAsync();
        }
        finally
        {
            transaction?.Dispose();
        }
    }
}