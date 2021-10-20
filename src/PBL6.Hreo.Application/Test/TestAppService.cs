﻿using PBL6.Hreo.Entities;
using PBL6.Hreo.Models;
using PBL6.Hreo.Repository;
using PBL6.Hreo.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace PBL6.Hreo.Services
{
    public class TestAppService : CrudAppService<
            Test,
            TestResponse,
            Guid,
            PagedAndSortedResultRequestDto,
            TestRequest,
            TestRequest>, 
        ITestAppService
    {
        private readonly ITestRepository _repository;
        private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;

        public TestAppService(ITestRepository repository,
            IAsyncQueryableExecuter asyncQueryableExecuter) : base(repository)
        {
            _repository = repository;
            _asyncQueryableExecuter = asyncQueryableExecuter;
        }

        public override Task<PagedResultDto<TestResponse>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return base.GetListAsync(input);
        }
    }
}
