using Domain.Customers;
using Domain.Primitives;

namespace Application.Customers.Delete;

internal sealed class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, ErrorOr<Unit>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerCommandHandler(
        ICustomerRepository customerRepository
        , IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Unit>> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        if (await _customerRepository.GetByIdAsync(new CustomerId(command.Id)) is not Customer customer)
            return Error.NotFound("Customer.NotFound", "The customer with the provide Id was not found.");

        _customerRepository.Delete(customer);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}