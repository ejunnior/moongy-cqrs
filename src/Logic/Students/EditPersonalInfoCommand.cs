namespace Logic.Students;

using System.Threading.Tasks;
using Utils;

public sealed class EditPersonalInfoCommand : ICommand
{
    public string Email { get; set; }
    public long Id { get; set; }
    public string Name { get; set; }
}

public sealed class EditPersonalInfoHandler : ICommandHandler<EditPersonalInfoCommand>
{
    private readonly StudentRepository _studentRepository;
    private readonly UnitOfWork _unitOfWork;

    public EditPersonalInfoHandler(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _studentRepository = new StudentRepository(unitOfWork);
    }

    public void Handle(EditPersonalInfoCommand args)
    {
        Student student = _studentRepository.GetById(args.Id);
        //if (student == null)
        //    return Error($"No student found for Id {command.Id}");
        student.Name = args.Name;
        student.Email = args.Email;
        _unitOfWork.Commit();
    }
}