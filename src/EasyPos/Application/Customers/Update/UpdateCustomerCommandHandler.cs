using Domain.Customers;
using Domain.DomainErrors;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Application.Customers.Update;

public sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, ErrorOr<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
    {
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        if (!await _customerRepository.ExistsAsync(new CustomerId(command.Id)))
            return Errors.Customer.CustomerNoFoundById;

        if (PhoneNumber.Create(command.PhoneNumber) is not PhoneNumber phoneNumber)
            return Errors.Customer.PhoneNumberWithBadFormat;

        if (Address.Create(command.Country, command.Line1, command.Line2, command.City,
                    command.State, command.ZipCode) is not Address address)
            return Errors.Customer.AddressWithBadFormat;

        Customer customer = Customer.UpdateCustomer(command.Id, command.Name,
            command.LastName,
            command.Email,
            phoneNumber,
            address,
            command.Active);

        _customerRepository.Update(customer);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}