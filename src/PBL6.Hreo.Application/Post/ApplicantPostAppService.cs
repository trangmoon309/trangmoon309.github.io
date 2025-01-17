﻿using PBL6.Hreo.Entities;
using PBL6.Hreo.Models;
using PBL6.Hreo.Repository;
using PBL6.Hreo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;
using static PBL6.Hreo.Common.Enum.Enum;

namespace PBL6.Hreo.Services
{
    public class ApplicantPostAppService : CrudAppService<
            ApplicantPost,
            ApplicantPostResponse,
            Guid,
            PagedAndSortedResultRequestDto,
            ApplicantPostRequest,
            ApplicantPostRequest>, IApplicantPostAppService
    {
        private readonly IApplicantPostRepository _repository;
        private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;
        private readonly IPostRepository _postRepository;
        private readonly IUserInformationRepository _userInforRepository;
        private readonly IInterestedPostRepository _interesPostRepository;
        private readonly IUserRepository _userRepository;
        private readonly IApplicantTestRepository _appTestRepository;

        public ApplicantPostAppService(IApplicantPostRepository repository,
            IAsyncQueryableExecuter asyncQueryableExecuter,
            IPostRepository postRepository,
            IUserInformationRepository userInforRepository,
            IInterestedPostRepository interesPostRepository,
            IUserRepository userRepository, IApplicantTestRepository appTestRepository) : base(repository)
        {
            _repository = repository;
            _asyncQueryableExecuter = asyncQueryableExecuter;
            _postRepository = postRepository;
            _userInforRepository = userInforRepository;
            _interesPostRepository = interesPostRepository;
            _userRepository = userRepository;
            _appTestRepository = appTestRepository;
        }

        // Danh sách ứng viên: Là những ứng viên đã submit CV của họ lên
        public async Task<PagedResultDto<UserInformationResponse>> GetSubmitedListByCondittion(Guid postId, SearchInvitePostRequest request, PagedAndSortedResultRequestDto pageRequest)
        {
            try
            {
                var userInfors = _userInforRepository.GetList();
                var interestPost = _interesPostRepository.GetByPostId(postId);
                var appTests = await _asyncQueryableExecuter.ToListAsync(_appTestRepository.GetList());
                appTests = appTests.Where(x => x.PostId == postId).ToList();

                interestPost = interestPost.Where(x => x.InterestedPostStatus == InterestedPostStatus.SUBMITTED);

                if (request.Level.HasValue)
                {
                    interestPost = interestPost.Where(x => x.UserInformation.Level.ToString().Equals(request.Level.Value.ToString()));
                }

                var toList = await _asyncQueryableExecuter.ToListAsync(interestPost);

                userInfors = userInfors.Where(x => interestPost.Select(y => y.ApplicantId).Contains(x.Id));

                var userList = await _asyncQueryableExecuter.ToListAsync(userInfors);
                var total = toList.Count();

                var userAbp = await _userRepository.GetList();
                var userAbpList = await _asyncQueryableExecuter.ToListAsync(userAbp);
                var userAbpResponse = ObjectMapper.Map<List<User>, List<UserResponse>>(userAbpList);

                var result = ObjectMapper.Map<List<UserInformation>, List<UserInformationResponse>>(userList);

                result.ForEach(x =>
                {
                    x.UserAbp = userAbpResponse.FirstOrDefault(y => y.Id.Equals(x.UserId));

                    var test = appTests.FirstOrDefault(y => y.ApplicantId == x.Id);

                    x.Result = test != null ? Convert.ToInt32(test.Result) : null;
                });

                result = result.Skip(pageRequest.SkipCount).Take(pageRequest.MaxResultCount).ToList();

                return new PagedResultDto<UserInformationResponse>(total, result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<PagedResultDto<ApplicantPostResponse>> GetListByApplicant(Guid applicantId, SearchAppTestRequest request, PagedAndSortedResultRequestDto pageRequest)
        {
            try
            {
                var query = _repository.GetListByApplicantId(applicantId);

                query = query.OrderByDescending(x => x.CreationTime);

                if (request.ApplicantPostStatus.HasValue)
                {
                    query = query.Where(x => x.ApplicantPostStatus.ToString().Equals(request.ApplicantPostStatus.Value.ToString()));
                }

                var toList = await _asyncQueryableExecuter.ToListAsync(query);
                var total = toList.Count();

                var result = ObjectMapper.Map<List<ApplicantPost>, List<ApplicantPostResponse>>(toList);

                result = result.Skip(pageRequest.SkipCount).Take(pageRequest.MaxResultCount).ToList();
                var userAbp = await _userRepository.GetList();
                var userAbpList = await _asyncQueryableExecuter.ToListAsync(userAbp);
                var userAbpResponse = ObjectMapper.Map<List<User>, List<UserResponse>>(userAbpList);

                result.ForEach(x =>
                {
                    x.UserInformation.UserAbp = userAbpResponse.FirstOrDefault(y => y.Id.Equals(x.UserInformation.UserId));
                });

                return new PagedResultDto<ApplicantPostResponse>(total, result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // Gửi bài test cho thí sinh
        public async Task<List<ApplicantPostResponse>> CreateMultiple(List<SendTestToApplicationRequest> request)
        {
            try
            {

                var entities = ObjectMapper.Map<List<SendTestToApplicationRequest>, List<ApplicantPost>>(request);

                entities.ForEach(x => {
                    EntityHelper.TrySetId(x, GuidGenerator.Create);
                    x.ApplicantPostStatus = ApplicantPostStatus.OPEN;
                });

                await _repository.CreateMultiple(entities);

                var responses = ObjectMapper.Map<List<ApplicantPost>, List<ApplicantPostResponse>>(entities);

                return responses;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
