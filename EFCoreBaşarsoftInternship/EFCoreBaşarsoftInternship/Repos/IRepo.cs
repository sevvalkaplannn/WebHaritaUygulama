using EFCoreBaşarsoftInternship.DTOs;
using EFCoreBaşarsoftInternship.Exceptions;

namespace EFCoreBaşarsoftInternship.Repos
{
    public interface IRepo : IDisposable
    {
        Response AddDoor(DoorInfosUpdateDTO newDoor);
        Response GetAllDoors();
        Response GetDoorById(int id);
        Response UpdateDoorInfos(int id, DoorInfosUpdateDTO updatedDoor);
        Response DeleteById(int id);
        Response DeleteAllDoors();

    }
}
