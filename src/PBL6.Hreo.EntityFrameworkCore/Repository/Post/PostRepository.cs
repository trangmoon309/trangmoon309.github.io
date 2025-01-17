﻿using Microsoft.EntityFrameworkCore;
using PBL6.Hreo.Entities;
using PBL6.Hreo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;

namespace PBL6.Hreo.Repository
{
    public class PostRepository : EfCoreRepository<IHreoDbContext, Post, Guid>, IPostRepository
    {
        public PostRepository(Volo.Abp.EntityFrameworkCore.IDbContextProvider<IHreoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public IQueryable<Post> GetList()
        {
            return GetQueryable().Where(x => !x.IsDeleted)
                .Include(x => x.InvitationPosts)
                .Include(x => x.InterestedPosts)
                .Include(x => x.ApplicantPosts)
                .Include(x => x.PostTests).ThenInclude(y => y.Test);
        }

        public async Task<Post> GetById(Guid id)
        {
            var query = await GetQueryable().Where(x => !x.IsDeleted && x.Id.Equals(id))
                 .Include(x => x.InvitationPosts)
                .Include(x => x.InterestedPosts)
                .Include(x => x.ApplicantPosts)
                .Include(x => x.PostTests).ThenInclude(y => y.Test)
                .FirstOrDefaultAsync();
            return query;
        }

        public IQueryable<Post> SearchQueryable(IQueryable<Post> query, string keyWord)
        {
            query = query.Where(x => (EF.Functions.Unaccent(x.Title.ToLower()).Contains(EF.Functions.Unaccent(keyWord.ToLower())))
                                && !x.IsDeleted);
            return query;
        }
    }
}
