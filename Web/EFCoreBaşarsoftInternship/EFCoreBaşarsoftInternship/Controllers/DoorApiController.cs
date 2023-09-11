using EFCoreBaşarsoftInternship.Repos;
using EFCoreBaşarsoftInternship.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EFCoreBaşarsoftInternship.Exceptions;

namespace EFCoreBaşarsoftInternship.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DoorApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoorApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public Response Add(DoorInfosUpdateDTO door)
        {
            return _unitOfWork.doors.AddDoor(door);
        }

        [HttpGet]
        public Response GetAll()
        {
            return _unitOfWork.doors.GetAllDoors();
        }

        [HttpGet("{id}")]
        public Response GetById(int id)
        {
            return _unitOfWork.doors.GetDoorById(id);
        }

        [HttpPut("{id}")]
        public Response UpdateDoorInfos(int id, [FromBody] DoorInfosUpdateDTO updatedDoor)
        {
            return _unitOfWork.doors.UpdateDoorInfos(id, updatedDoor);
        }

        [HttpDelete("{id}")]
        public Response DeleteById(int id)
        {
            return _unitOfWork.doors.DeleteById(id);
        }

        [HttpDelete]
        public Response DeleteAll()
        {
            return _unitOfWork.doors.DeleteAllDoors();
        }
    }
}
