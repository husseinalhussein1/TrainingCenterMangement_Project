using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrainingCenterManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult GetFile(string id)
        {
            var path = "test.txt";
            if(!System.IO.File.Exists(path))
            {
                return NotFound();
            }
            var mytextfile =System.IO.File.ReadAllBytes(path);
            return File(mytextfile,"text/plain",Path.GetFileName(path));
        }
    }
}
