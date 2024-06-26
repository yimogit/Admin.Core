﻿using System;
using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Entities;

#pragma warning disable CS8632

namespace ZhonTai.Module.Dev.Domain.CodeGroup
{
    /// <summary>
    /// 模板组 实体类
    /// </summary>
    /// <remarks></remarks>
    [Table(Name="dev_code_group")]
    public partial class CodeGroupEntity: EntityBase
    {
        /// <summary>
        /// 模板组名称
        /// </summary>
        /// <remarks></remarks>
        [Column(StringLength=200)]
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <remarks></remarks>
        [Column(StringLength=500)]
        public string? Remark { get; set; }
    }

}

#pragma warning restore CS8632

