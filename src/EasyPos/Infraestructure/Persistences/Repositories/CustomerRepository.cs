using Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Persistences.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentException(nameof(context));
    }

    public async Task AddAsync(Customer customer) => await _context.Customers.AddAsync(customer);

    public void Delete(Customer customer) => _context.Remove(customer);

    public async Task<bool> ExistsAsync(CustomerId id) => await _context.Customers.AnyAsync(w => w.Id == id);

    public async Task<List<Customer>> GetAll()  => await _context.Customers.ToListAsync();

    public async Task<Customer?> GetByIdAsync(CustomerId id) => await _context.Customers.SingleOrDefaultAsync(w => w.Id == id);

    public void Update(Customer customer) => _context.Customers.Update(customer);
}
