using Microsoft.AspNetCore.Mvc;
using ZavrsniSeminarskiRad.Services.Interfaces;

namespace ZavrsniSeminarskiRad.Controllers
{
    [Route("[controller]")]
    public class SharedController : ControllerBase
    {
        private readonly IFileSaveService fileSaveService;

        public SharedController(IFileSaveService fileSaveService)
        {
            this.fileSaveService = fileSaveService;
        }

        [Route("files/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {

            var img = await fileSaveService.GetFile(id);
            if (img == null)
            {
                return NoContent();
            }
            Response.Headers.Add("Content-Disposition", img.ContentDisposition.ToString());

            return File(img.FileStream, "image/" + img.FileExtension.Replace(".", string.Empty));
        }

        [HttpDelete]
        [Route("file/{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            if (await fileSaveService.DeleteFile(id))
            {
                return Ok(new { Msg = "deleted!" });
            }

            return BadRequest(new { Msg = "Error!" });
        }
    }
}
