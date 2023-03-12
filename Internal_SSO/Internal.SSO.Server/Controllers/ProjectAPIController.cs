using AutoMapper;
using Internal.SSO.Entities.Models;
using Internal.SSO.IServices;
using Internal.SSO.Server.Bindings.Projects;
using Internal.SSO.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Internal.SSO.Server.Controllers
{
    [Route("api/project")]
    [ApiController]
    public class ProjectAPIController : ControllerBase
    {
        private IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectAPIController(
            IProjectService projectService,
            IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Add(CreateProjectBinding input)
        {
            var project = _mapper.Map<Project>(input);
            project.Creator = User.GetUserId();

            _projectService.Add(project);

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var result = _projectService.Get(id);

            return Ok(result);
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var result = _projectService.GetAll();

            return Ok(result);
        }

        [HttpGet("getinfoall")]
        [AllowAnonymous]
        public IActionResult GetInfoAll()
        {
            var result = _projectService.GetInfoAll();

            return Ok(result);
        }

        [HttpGet("paged")]
        public IActionResult GetAll(string name, int pageIndex, int pageSize)
        {
            var result = _projectService.GetAll(name, pageIndex, pageSize);

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Edit(EditProjectBinding input)
        {
            var project = _mapper.Map<Project>(input);
            project.Updater = User.GetUserId();

            _projectService.Update(project);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _projectService.Delete(id);

            return Ok();
        }

        [HttpPost("export")]
        public IActionResult ExportExcel(ExportProjectBinding input)
        {
            var stream = new MemoryStream();

            _projectService.Export(input.Name, stream);

            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"export.xlsx");
        }
    }
}
