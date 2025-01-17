﻿using Microsoft.AspNetCore.Mvc;
using PBL6.Hreo.Models;
using PBL6.Hreo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PBL6.Hreo.Controllers
{
    [Route("api/applicant-posts")]
    public class ApplicantPostController : HreoController
    {
        private readonly IApplicantPostAppService _service;

        public ApplicantPostController(IApplicantPostAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(PagedAndSortedResultRequestDto input)
        {
            var applicant_postList = await _service.GetListAsync(input);

            return Ok(applicant_postList);
        }

        [HttpGet]
        [Route("by-post/summitted")]
        public async Task<IActionResult> GetSubmittedList(Guid postId, SearchInvitePostRequest request, PagedAndSortedResultRequestDto pageRequest)
        {
            var applicant_postList = await _service.GetSubmitedListByCondittion(postId, request, pageRequest);

            return Ok(applicant_postList);
        }

        [HttpGet]
        [Route("by-applicant/{applicantId}")]
        public async Task<IActionResult> GetListByApplicant(Guid applicantId, SearchAppTestRequest request, PagedAndSortedResultRequestDto pageRequest)
        {
            var applicant_postList = await _service.GetListByApplicant(applicantId, request, pageRequest);

            return Ok(applicant_postList);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _service.GetAsync(id);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create2Async([FromBody] ApplicantPostRequest request)
        {
            var createdApplicant_Post = await _service.CreateAsync(request);
            return Ok(createdApplicant_Post);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ApplicantPostRequest request)
        {
            var updatedApplicant_Post = await _service.UpdateAsync(id, request);
            return Ok(updatedApplicant_Post);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
