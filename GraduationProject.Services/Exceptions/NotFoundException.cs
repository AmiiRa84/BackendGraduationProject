using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services.Exceptions
{
    public abstract class NotFoundException(string msg):Exception(msg)
    {
        
    }
    public sealed class ChildNotFoundException(int id):NotFoundException($"child Id :{id} is Not Found");
    public sealed class ChildTaskNotFoundException(int id):NotFoundException($"child Id :{id} has no Tasks Assigned ");
    public sealed class TaskNotFoundException(int id):NotFoundException($"Task with Id :{id} is Not Found");
    public sealed class SpecialistNotFoundException(int id):NotFoundException($"Specialist with Id :{id} is Not Found");
    public sealed class ParentNotFoundException(int id): NotFoundException($"Parent with Id :{id} is Not Found");
    public sealed class UserNotFoundException(int id): NotFoundException($"User with Id :{id} is Not Found");
    public sealed class ParentNotFoundExceptionAuth (): NotFoundException($"This Parent is Not Found");
}
