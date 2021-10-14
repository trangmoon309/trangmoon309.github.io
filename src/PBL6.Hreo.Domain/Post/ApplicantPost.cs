﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
using static PBL6.Hreo.Common.Enum.Enum;

namespace PBL6.Hreo.Entities
{
    public class ApplicantPost : FullAuditedAggregateRoot<Guid>
    {
        public Guid PostID { get; set; }

        public Guid TestID { get; set; }

        public Guid ApplicantID { get; set; }

        public ApplicantPostStatus ApplicantPostStatus { get; set; }

        public string ApplicantAnswer { get; set;}

        public float TestResult { get; set; }

        public DateTime StartTime { get; set; }

        public float TimeUsed { get; set; }

        public float TimeFinished { get; set; }
    }
}
