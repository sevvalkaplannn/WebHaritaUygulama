using EFCoreBaşarsoftInternship.DTOs;
using EFCoreBaşarsoftInternship.Exceptions;

namespace EFCoreBaşarsoftInternship.Repos
{
    public class Repo : IRepo
    {
        private readonly AppDbContext _dbContext;

        public Repo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Response AddDoor(DoorInfosUpdateDTO newDoor)
        {
            var response = new Response();

            try
            {
                var door = new Door
                {
                    name = newDoor.name,
                    x = newDoor.x,
                    y = newDoor.y
                };

                _dbContext.doors.Add(door);
                _dbContext.SaveChanges();

                response.data = door;
                response.isSuccess = true;
                response.message = "Successfully added.";
            }
            catch (Exception ex)
            {
                response.message = "Failed to add.";
            }

            return response;
        }


        public Response GetAllDoors()
        {
            var response = new Response();

            try
            {
                var doors = _dbContext.doors.ToList();
                response.data = doors;
                response.isSuccess = true;
                response.message = "Successful.";
            }
            catch
            {
                response.message = "Failed to get.";
            }

            return response;
        }

        public Response GetDoorById(int id)
        {
            var response = new Response();

            try
            {
                var door = _dbContext.doors.FirstOrDefault(d => d.id == id);

                if (door != null)
                {
                    response.data = door;
                    response.isSuccess = true;
                    response.message = "Successful.";
                }
                else
                {
                    response.message = "Door not found.";
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to get.";
            }

            return response;
        }

        public Response UpdateDoorInfos(int id, DoorInfosUpdateDTO updatedDoor)
        {
            var response = new Response();

            try
            {
                var door = _dbContext.doors.FirstOrDefault(d => d.id == id);

                if (door != null)
                {
                    if (updatedDoor.x != 0)
                        door.x = updatedDoor.x;

                    if (updatedDoor.y != 0)
                        door.y = updatedDoor.y;

                    if (!string.IsNullOrEmpty(updatedDoor.name) && updatedDoor.name != "string")
                        door.name = updatedDoor.name;

                    _dbContext.SaveChanges();

                    response.message = "Successfully updated.";
                    response.data = updatedDoor;
                    response.isSuccess = true;
                }
                else
                {
                    response.message = "Door not found.";
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to update.";
            }

            return response;
        }
        public Response DeleteById(int id)
        {
            Response response = new Response();

            try
            {
                var door = _dbContext.doors.FirstOrDefault(d => d.id == id);

                if (door != null)
                {
                    _dbContext.doors.Remove(door);
                    _dbContext.SaveChanges();

                    response.message = "Successfully deleted.";
                    response.isSuccess = true;
                }
                else
                {
                    response.message = "Door not found.";
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete.";
            }

            return response;
        }

        public Response DeleteAllDoors()
        {
            Response response = new Response();

            try
            {
                _dbContext.doors.RemoveRange(_dbContext.doors);
                _dbContext.SaveChanges();

                response.message = "Successfully deleted.";
                response.isSuccess = true;
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete.";
            }

            return response;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
