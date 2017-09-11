﻿namespace GeekLearning.Authorizations.EntityFrameworkCore.Data
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Group : Principal
    {
        [Required]
        [StringLength(Constants.ColumnNameLength)]
        public string Name { get; set; }
    }
}