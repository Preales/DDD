using Domain.Customers;
using Domain.Primitives;
using Domain.ValueObjects;
using MediatR;

namespace Application.Customers.Create;

internal sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Unit>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository ?? throw new ArgumentException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(_unitOfWork));
    }

    public async Task<Unit> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        if(PhoneNumber.Create(command.PhoneNumber) is not PhoneNumber phoneNumber)
            throw new ArgumentException(nameof(phoneNumber));

        if(Address.Create(
                command.Country,
                command.Line1,
                command.Line2,
                command.City,
                command.State,
                command.ZipCode) 
            is not Address address)
            throw new ArgumentException(nameof(address));
        
        var customer = new Customer(
            new CustomerId(Guid.NewGuid()),
            command.Name,
            command.LastName,
            command.LastName,
            phoneNumber,
            address,
            true
        );

        await _customerRepository.Add(customer);
        await _unitOfWork.SaveChangeAsync(cancellationToken);

        return Unit.Value;
    }   
}
