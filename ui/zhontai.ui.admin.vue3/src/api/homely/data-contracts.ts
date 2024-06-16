/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface DynamicFilterInfo {
  field?: string | null
  /** Contains=0,StartsWith=1,EndsWith=2,NotContains=3,NotStartsWith=4,NotEndsWith=5,Equal=6,Equals=7,Eq=8,NotEqual=9,GreaterThan=10,GreaterThanOrEqual=11,LessThan=12,LessThanOrEqual=13,Range=14,DateRange=15,Any=16,NotAny=17,Custom=18 */
  operator?: DynamicFilterOperator
  value?: any
  /** And=0,Or=1 */
  logic?: DynamicFilterLogic
  filters?: DynamicFilterInfo[] | null
}

/**
 * And=0,Or=1
 * @format int32
 */
export type DynamicFilterLogic = 0 | 1

/**
 * Contains=0,StartsWith=1,EndsWith=2,NotContains=3,NotStartsWith=4,NotEndsWith=5,Equal=6,Equals=7,Eq=8,NotEqual=9,GreaterThan=10,GreaterThanOrEqual=11,LessThan=12,LessThanOrEqual=13,Range=14,DateRange=15,Any=16,NotAny=17,Custom=18
 * @format int32
 */
export type DynamicFilterOperator = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13 | 14 | 15 | 16 | 17 | 18

/** 分页信息输入 */
export interface PageInputThingCategoryGetPageInput {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  /** 物品分类分页查询条件输入 */
  filter?: ThingCategoryGetPageInput
}

/** 分页信息输入 */
export interface PageInputThingGetPageInput {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  /** 物品分页查询条件输入 */
  filter?: ThingGetPageInput
}

/** 分页信息输出 */
export interface PageOutputThingCategoryGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: ThingCategoryGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputThingGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: ThingGetPageOutput[] | null
}

/** 结果输出 */
export interface ResultOutputBoolean {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: boolean
}

/** 结果输出 */
export interface ResultOutputIEnumerableThingCategoryGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: ThingCategoryGetListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputIEnumerableThingGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: ThingGetListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputInt64 {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /**
   * 数据
   * @format int64
   */
  data?: number
}

/** 结果输出 */
export interface ResultOutputPageOutputThingCategoryGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputThingCategoryGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputThingGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputThingGetPageOutput
}

/** 结果输出 */
export interface ResultOutputThingCategoryGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 物品分类查询结果输出 */
  data?: ThingCategoryGetOutput
}

/** 结果输出 */
export interface ResultOutputThingGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 物品查询结果输出 */
  data?: ThingGetOutput
}

/** 物品新增输入 */
export interface ThingAddInput {
  /**
   * 物品名称
   * @minLength 1
   */
  name: string
  /** 图片 */
  imageUrl?: string | null
  /**
   * 有效期
   * @format date-time
   */
  availableDate?: string | null
  /** 备注 */
  remark?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /**
   * 分类
   * @format int64
   */
  categoryId?: number | null
  /** 标签 */
  tags?: string | null
}

/** 物品分类新增输入 */
export interface ThingCategoryAddInput {
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /** 分类名称 */
  name?: string | null
}

/** 物品分类列表查询条件输入 */
export interface ThingCategoryGetListInput {
  /** 分类名称 */
  name?: string | null
}

/** 物品分类列表查询结果输出 */
export interface ThingCategoryGetListOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /** 分类名称 */
  name?: string | null
  /**
   * 租户
   * @format int64
   */
  tenantId?: number | null
}

/** 物品分类查询结果输出 */
export interface ThingCategoryGetOutput {
  /** @format int64 */
  id?: number
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /** 分类名称 */
  name?: string | null
  /**
   * 租户
   * @format int64
   */
  tenantId?: number | null
}

/** 物品分类分页查询条件输入 */
export interface ThingCategoryGetPageInput {
  /** 分类名称 */
  name?: string | null
}

/** 物品分类分页查询结果输出 */
export interface ThingCategoryGetPageOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /** 分类名称 */
  name?: string | null
}

/** 物品分类更新数据输入 */
export interface ThingCategoryUpdateInput {
  /** @format int64 */
  id?: number
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /** 分类名称 */
  name?: string | null
}

/** 物品列表查询条件输入 */
export interface ThingGetListInput {
  /** 物品名称 */
  name?: string | null
  /**
   * 有效期
   * @format date-time
   */
  availableDate?: string | null
  /** 备注 */
  remark?: string | null
}

/** 物品列表查询结果输出 */
export interface ThingGetListOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 物品名称 */
  name?: string | null
  /** 图片 */
  imageUrl?: string | null
  /**
   * 有效期
   * @format date-time
   */
  availableDate?: string | null
  /** 备注 */
  remark?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /**
   * 分类
   * @format int64
   */
  categoryId?: number | null
  /** 标签 */
  tags?: string | null
}

/** 物品查询结果输出 */
export interface ThingGetOutput {
  /** @format int64 */
  id?: number
  /** 物品名称 */
  name?: string | null
  /** 图片 */
  imageUrl?: string | null
  /**
   * 有效期
   * @format date-time
   */
  availableDate?: string | null
  /** 备注 */
  remark?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /**
   * 分类
   * @format int64
   */
  categoryId?: number | null
  /** 标签 */
  tags?: string | null
}

/** 物品分页查询条件输入 */
export interface ThingGetPageInput {
  /** 物品名称 */
  name?: string | null
  /**
   * 有效期
   * @format date-time
   */
  availableDate?: string | null
  /** 备注 */
  remark?: string | null
}

/** 物品分页查询结果输出 */
export interface ThingGetPageOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 物品名称 */
  name?: string | null
  /** 图片 */
  imageUrl?: string | null
  /**
   * 有效期
   * @format date-time
   */
  availableDate?: string | null
  /** 备注 */
  remark?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /**
   * 分类
   * @format int64
   */
  categoryId?: number | null
  /** 标签 */
  tags?: string | null
}

/** 物品更新数据输入 */
export interface ThingUpdateInput {
  /** @format int64 */
  id?: number
  /**
   * 物品名称
   * @minLength 1
   */
  name: string
  /** 图片 */
  imageUrl?: string | null
  /**
   * 有效期
   * @format date-time
   */
  availableDate?: string | null
  /** 备注 */
  remark?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /**
   * 分类
   * @format int64
   */
  categoryId?: number | null
  /** 标签 */
  tags?: string | null
}
