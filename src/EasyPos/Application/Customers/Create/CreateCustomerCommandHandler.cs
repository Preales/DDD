using Domain.Customers;
using Domain.DomainErrors;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Application.Customers.Create;

public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ErrorOr<Guid>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository ?? throw new ArgumentException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(_unitOfWork));
    }

    public async Task<ErrorOr<Guid>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        if (PhoneNumber.Create(command.PhoneNumber) is not PhoneNumber phoneNumber)
            return Errors.Customer.PhoneNumberWithBadFormat;

        if (Address.Create(
                command.Country,
                command.Line1,
                command.Line2,
                command.City,
                command.State,
                command.ZipCode)
            is not Address address)
            return Errors.Customer.AddressWithBadFormat;

        var customer = new Customer(
            new CustomerId(Guid.NewGuid()),
            command.Name,
            command.LastName,
            command.LastName,
            phoneNumber,
            address,
            true
        );

        await _customerRepository.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id.Value;
    }
}
