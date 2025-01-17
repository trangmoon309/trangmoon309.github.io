﻿using PBL6.Hreo.File;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using static PBL6.Hreo.Common.Enum.Enum;

namespace PBL6.Hreo.Models
{
    public class UserInformationResponse : FullAuditedEntityDto<Guid>
    {
        public UserResponse UserAbp { get; set; }

        public Guid UserId { get; set; }

        public Guid AvatarId { get; set; }

        public Guid CVId { get; set; }

        public Guid? BranchId { get; set; }

        public string WorkAddress { get; set; }

        public string GithubLink { get; set; }

        public string Language { get; set; }

        public string Level { get; set; }

        public string Status { get; set; }

        public Major Major { get; set; }

        public int? Result { get; set; }

        // Dùng cho màn hình danh sách lời mời của HR
        public InvitationPostStatus IsInvitedForPost { get; set; }

        public BranchResponse Branch { get; set; }

        public FileInformationResponse Avatar { get; set; }
    }
}
