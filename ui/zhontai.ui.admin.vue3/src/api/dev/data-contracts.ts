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

export interface BaseDataGetOutput {
  databases?: DatabaseGetOutput[] | null
  authorName?: string | null
  apiAreaName?: string | null
  namespace?: string | null
  backendOut?: string | null
  frontendOut?: string | null
  dbMigrateSqlOut?: string | null
  usings?: string | null
  menuAfterText?: string | null
}

export interface CodeGenFieldGetOutput {
  /** @format int64 */
  id?: number
  /** @format int64 */
  codeGenId?: number
  /** 库定位器名 */
  dbKey?: string | null
  /** 字段名 */
  columnName?: string | null
  /** 数据库列名(物理字段名) */
  columnRawName?: string | null
  /** .NET数据类型 */
  netType?: string | null
  /** 数据库中类型（物理类型） */
  dbType?: string | null
  /** 字段描述 */
  comment?: string | null
  /** 默认值 */
  defaultValue?: string | null
  /** 字段标题 */
  title?: string | null
  /** 主键 */
  isPrimary?: boolean
  /** 可空 */
  isNullable?: boolean
  /** 长度 */
  length?: string | null
  /** 编辑器 */
  editor?: string | null
  /**
   * 同步表结构时的列排序
   * @format int32
   */
  position?: number
  /** 是否通用字段 */
  whetherCommon?: boolean
  /** 列表是否缩进（字典） */
  whetherRetract?: boolean
  /** 是否是查询条件 */
  whetherQuery?: boolean
  /** 增 */
  whetherAdd?: boolean
  /** 改 */
  whetherUpdate?: boolean
  /** 分布显示 */
  whetherTable?: boolean
  /** 列表 */
  whetherList?: boolean
  /** 索引方式 */
  indexMode?: string | null
  /** 唯一键 */
  isUnique?: boolean
  /** 查询方式 */
  queryType?: string | null
  /** 字典编码 */
  dictTypeCode?: string | null
  /** 外联实体名 */
  includeEntity?: string | null
  /**
   * 外联对应关系 0 1对1 1 1对多
   * @format int32
   */
  includeMode?: number
  /** 外联实体关联键 */
  includeEntityKey?: string | null
  /** 显示文本字段 */
  displayColumn?: string | null
  /** 选中值字段 */
  valueColumn?: string | null
  /** 父级字段 */
  pidColumn?: string | null
  /** 作用类型（字典） */
  effectType?: string | null
  /** 前端规则检测触发时机 */
  frontendRuleTrigger?: string | null
}

export interface CodeGenGetOutput {
  /** @format int64 */
  id?: number
  /** 作者姓名 */
  authorName?: string | null
  /** 是否移除表前缀 */
  tablePrefix?: boolean
  /** 生成方式 */
  generateType?: string | null
  /** 库定位器名 */
  dbKey?: string | null
  /** 数据库类型 */
  dbType?: string | null
  /** 数据库表名 */
  tableName?: string | null
  /** 命名空间 */
  namespace?: string | null
  /** 实体名称 */
  entityName?: string | null
  /** 业务名 */
  busName?: string | null
  /** Api分区名称 */
  apiAreaName?: string | null
  /** 基类名称 */
  baseEntity?: string | null
  /** 父菜单 */
  menuPid?: string | null
  /** 菜单后缀 */
  menuAfterText?: string | null
  /** 后端输出目录 */
  backendOut?: string | null
  /** 前端输出目录 */
  frontendOut?: string | null
  /** 数据库迁移目录 */
  dbMigrateSqlOut?: string | null
  /** 备注说明 */
  comment?: string | null
  /** 实体导入的命令空间 */
  usings?: string | null
  /** 生成Entity实体类 */
  genEntity?: boolean
  /** 生成Repository仓储类 */
  genRepository?: boolean
  /** 生成Service服务类 */
  genService?: boolean
  /** 生成新增服务 */
  genAdd?: boolean
  /** 生成更新服务 */
  genUpdate?: boolean
  /** 新增删除服务 */
  genDelete?: boolean
  /** 生成列表查询服务 */
  genGetList?: boolean
  /** 生成软删除服务 */
  genSoftDelete?: boolean
  /** 生成批量删除服务 */
  genBatchDelete?: boolean
  /** 生成批量软删除服务 */
  genBatchSoftDelete?: boolean
  fields?: CodeGenFieldGetOutput[] | null
}

export interface CodeGenUpdateInput {
  /** @format int64 */
  id?: number
  /** 作者姓名 */
  authorName?: string | null
  /** 是否移除表前缀 */
  tablePrefix?: boolean
  /** 生成方式 */
  generateType?: string | null
  /** 库定位器名 */
  dbKey?: string | null
  /** 数据库类型 */
  dbType?: string | null
  /** 数据库表名 */
  tableName?: string | null
  /** 命名空间 */
  namespace?: string | null
  /** 实体名称 */
  entityName?: string | null
  /** 业务名 */
  busName?: string | null
  /** Api分区名称 */
  apiAreaName?: string | null
  /** 基类名称 */
  baseEntity?: string | null
  /** 父菜单 */
  menuPid?: string | null
  /** 菜单后缀 */
  menuAfterText?: string | null
  /** 后端输出目录 */
  backendOut?: string | null
  /** 前端输出目录 */
  frontendOut?: string | null
  /** 数据库迁移目录 */
  dbMigrateSqlOut?: string | null
  /** 备注说明 */
  comment?: string | null
  /** 实体导入的命令空间 */
  usings?: string | null
  /** 生成Entity实体类 */
  genEntity?: boolean
  /** 生成Repository仓储类 */
  genRepository?: boolean
  /** 生成Service服务类 */
  genService?: boolean
  /** 生成新增服务 */
  genAdd?: boolean
  /** 生成更新服务 */
  genUpdate?: boolean
  /** 新增删除服务 */
  genDelete?: boolean
  /** 生成列表查询服务 */
  genGetList?: boolean
  /** 生成软删除服务 */
  genSoftDelete?: boolean
  /** 生成批量删除服务 */
  genBatchDelete?: boolean
  /** 生成批量软删除服务 */
  genBatchSoftDelete?: boolean
  fields?: CodeGenFieldGetOutput[] | null
}

/** 模板组新增输入 */
export interface CodeGroupAddInput {
  /**
   * 模板标题
   * @minLength 1
   */
  name: string
  /** 备注 */
  remark?: string | null
}

/** 模板演示新增输入 */
export interface CodeGroupDemoAddInput {
  /**
   * 文本框
   * @minLength 1
   */
  inputText: string
  /**
   * 数字框
   * @format int32
   */
  inputNumber?: number | null
  /** 文本域 */
  inputTextArea?: string | null
  /**
   * 日期
   * @format date-time
   */
  inputDate?: string | null
  /** 开关 */
  inputSwitch?: boolean | null
  /**
   * 下拉框
   * @minLength 1
   */
  inputSelectCustom: string
  /** 复选框 */
  inputCheckbox: boolean
  /**
   * 字典
   * @minLength 1
   */
  inputSelectDict: string
  /**
   * 模块业务单选
   * @format int64
   */
  inputBussinessSingle?: number | null
  /** 模块业务多选 */
  inputBussinessMultiple?: string | null
  /** 页面提交的模块业务多选数组 */
  inputBussinessMultiple_Values?: string[] | null
  /** 图片上传 */
  inputImage?: string | null
  /** 编辑器 */
  inputEditor?: string | null
}

/** 模板演示列表查询条件输入 */
export interface CodeGroupDemoGetListInput {
  /** 文本框 */
  inputText?: string | null
  /**
   * 模块业务单选
   * @format int64
   */
  inputBussinessSingle?: number | null
}

/** 模板演示列表查询结果输出 */
export interface CodeGroupDemoGetListOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 文本框 */
  inputText?: string | null
  /**
   * 数字框
   * @format int32
   */
  inputNumber?: number | null
  /** 文本域 */
  inputTextArea?: string | null
  /**
   * 日期
   * @format date-time
   */
  inputDate?: string | null
  /** 开关 */
  inputSwitch?: boolean | null
  /** 下拉框 */
  inputSelectCustom?: string | null
  /** 复选框 */
  inputCheckbox?: boolean
  /** 字典 */
  inputSelectDict?: string | null
  /** 字典名称 */
  inputSelectDictDictName?: string | null
  /**
   * 模块业务单选
   * @format int64
   */
  inputBussinessSingle?: number | null
  /** 模块业务单选显示文本 */
  inputBussinessSingle_Text?: string | null
  /** 模块业务多选 */
  inputBussinessMultiple?: string | null
  /** 模块业务多选显示文本 */
  inputBussinessMultiple_Texts?: string[] | null
  /** 页面使用的模块业务多选数组 */
  inputBussinessMultiple_Values?: string[] | null
  /** 图片上传 */
  inputImage?: string | null
  /** 编辑器 */
  inputEditor?: string | null
}

/** 模板演示查询结果输出 */
export interface CodeGroupDemoGetOutput {
  /** @format int64 */
  id?: number
  /** 文本框 */
  inputText?: string | null
  /**
   * 数字框
   * @format int32
   */
  inputNumber?: number | null
  /** 文本域 */
  inputTextArea?: string | null
  /**
   * 日期
   * @format date-time
   */
  inputDate?: string | null
  /** 开关 */
  inputSwitch?: boolean | null
  /** 下拉框 */
  inputSelectCustom?: string | null
  /** 复选框 */
  inputCheckbox?: boolean
  /** 字典 */
  inputSelectDict?: string | null
  /**
   * 模块业务单选
   * @format int64
   */
  inputBussinessSingle?: number | null
  /** 模块业务单选显示文本 */
  inputBussinessSingle_Text?: string | null
  /** 模块业务多选 */
  inputBussinessMultiple?: string | null
  /** 模块业务多选显示文本 */
  inputBussinessMultiple_Texts?: string[] | null
  /** 页面使用的模块业务多选数组 */
  inputBussinessMultiple_Values?: string[] | null
  /** 图片上传 */
  inputImage?: string | null
  /** 编辑器 */
  inputEditor?: string | null
}

/** 模板演示分页查询条件输入 */
export interface CodeGroupDemoGetPageInput {
  /** 文本框 */
  inputText?: string | null
  /**
   * 模块业务单选
   * @format int64
   */
  inputBussinessSingle?: number | null
}

/** 模板演示分页查询结果输出 */
export interface CodeGroupDemoGetPageOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 文本框 */
  inputText?: string | null
  /**
   * 数字框
   * @format int32
   */
  inputNumber?: number | null
  /** 文本域 */
  inputTextArea?: string | null
  /**
   * 日期
   * @format date-time
   */
  inputDate?: string | null
  /** 开关 */
  inputSwitch?: boolean | null
  /** 下拉框 */
  inputSelectCustom?: string | null
  /** 复选框 */
  inputCheckbox?: boolean
  /** 字典 */
  inputSelectDict?: string | null
  /** 字典名称 */
  inputSelectDictDictName?: string | null
  /**
   * 模块业务单选
   * @format int64
   */
  inputBussinessSingle?: number | null
  /** 模块业务单选显示文本 */
  inputBussinessSingle_Text?: string | null
  /** 模块业务多选 */
  inputBussinessMultiple?: string | null
  /** 模块业务多选显示文本 */
  inputBussinessMultiple_Texts?: string[] | null
  /** 页面使用的模块业务多选数组 */
  inputBussinessMultiple_Values?: string[] | null
  /** 图片上传 */
  inputImage?: string | null
  /** 编辑器 */
  inputEditor?: string | null
}

/** 模板演示更新数据输入 */
export interface CodeGroupDemoUpdateInput {
  /** @format int64 */
  id?: number
  /**
   * 文本框
   * @minLength 1
   */
  inputText: string
  /**
   * 数字框
   * @format int32
   */
  inputNumber?: number | null
  /** 文本域 */
  inputTextArea?: string | null
  /**
   * 日期
   * @format date-time
   */
  inputDate?: string | null
  /** 开关 */
  inputSwitch?: boolean | null
  /**
   * 下拉框
   * @minLength 1
   */
  inputSelectCustom: string
  /** 复选框 */
  inputCheckbox: boolean
  /**
   * 字典
   * @minLength 1
   */
  inputSelectDict: string
  /**
   * 模块业务单选
   * @format int64
   */
  inputBussinessSingle?: number | null
  /** 模块业务多选 */
  inputBussinessMultiple?: string | null
  /** 页面提交的模块业务多选数组 */
  inputBussinessMultiple_Values?: string[] | null
  /** 图片上传 */
  inputImage?: string | null
  /** 编辑器 */
  inputEditor?: string | null
}

/** 模板明细新增输入 */
export interface CodeGroupDetailAddInput {
  /**
   * 模板名称
   * @minLength 1
   */
  name: string
  /**
   * 模板分组
   * @format int64
   */
  groupId: number
  /** 生成路径 */
  path?: string | null
  /** 模板分组 */
  groupIds?: string | null
  /** 页面提交的模板分组数组 */
  groupIds_Values?: string[] | null
  /**
   * 模板内容
   * @minLength 1
   */
  content: string
}

/** 模板明细列表查询条件输入 */
export interface CodeGroupDetailGetListInput {
  /**
   * 模板分组
   * @format int64
   */
  groupId?: number | null
}

/** 模板明细列表查询结果输出 */
export interface CodeGroupDetailGetListOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 模板名称 */
  name?: string | null
  /**
   * 模板分组
   * @format int64
   */
  groupId?: number
  /** 模板分组显示文本 */
  groupId_Text?: string | null
  /** 生成路径 */
  path?: string | null
  /** 模板分组 */
  groupIds?: string | null
  /** 模板分组显示文本 */
  groupIds_Texts?: string[] | null
  /** 页面使用的模板分组数组 */
  groupIds_Values?: string[] | null
  /** 模板内容 */
  content?: string | null
}

/** 模板明细查询结果输出 */
export interface CodeGroupDetailGetOutput {
  /** @format int64 */
  id?: number
  /** 模板名称 */
  name?: string | null
  /**
   * 模板分组
   * @format int64
   */
  groupId?: number
  /** 模板分组显示文本 */
  groupId_Text?: string | null
  /** 生成路径 */
  path?: string | null
  /** 模板分组 */
  groupIds?: string | null
  /** 模板分组显示文本 */
  groupIds_Texts?: string[] | null
  /** 页面使用的模板分组数组 */
  groupIds_Values?: string[] | null
  /** 模板内容 */
  content?: string | null
}

/** 模板明细分页查询条件输入 */
export interface CodeGroupDetailGetPageInput {
  /**
   * 模板分组
   * @format int64
   */
  groupId?: number | null
}

/** 模板明细分页查询结果输出 */
export interface CodeGroupDetailGetPageOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 模板名称 */
  name?: string | null
  /**
   * 模板分组
   * @format int64
   */
  groupId?: number
  /** 模板分组显示文本 */
  groupId_Text?: string | null
  /** 生成路径 */
  path?: string | null
  /** 模板分组 */
  groupIds?: string | null
  /** 模板分组显示文本 */
  groupIds_Texts?: string[] | null
  /** 页面使用的模板分组数组 */
  groupIds_Values?: string[] | null
  /** 模板内容 */
  content?: string | null
}

/** 模板明细更新数据输入 */
export interface CodeGroupDetailUpdateInput {
  /** @format int64 */
  id?: number
  /**
   * 模板名称
   * @minLength 1
   */
  name: string
  /**
   * 模板分组
   * @format int64
   */
  groupId: number
  /** 生成路径 */
  path?: string | null
  /** 模板分组 */
  groupIds?: string | null
  /** 页面提交的模板分组数组 */
  groupIds_Values?: string[] | null
  /**
   * 模板内容
   * @minLength 1
   */
  content: string
}

/** 模板组列表查询条件输入 */
export interface CodeGroupGetListInput {
  /** 模板标题 */
  name?: string | null
}

/** 模板组列表查询结果输出 */
export interface CodeGroupGetListOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 模板标题 */
  name?: string | null
  /** 备注 */
  remark?: string | null
}

/** 模板组查询结果输出 */
export interface CodeGroupGetOutput {
  /** @format int64 */
  id?: number
  /** 模板标题 */
  name?: string | null
  /** 备注 */
  remark?: string | null
}

/** 模板组分页查询条件输入 */
export interface CodeGroupGetPageInput {
  /** 模板标题 */
  name?: string | null
}

/** 模板组分页查询结果输出 */
export interface CodeGroupGetPageOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 模板标题 */
  name?: string | null
  /** 备注 */
  remark?: string | null
}

/** 模板组更新数据输入 */
export interface CodeGroupUpdateInput {
  /** @format int64 */
  id?: number
  /**
   * 模板标题
   * @minLength 1
   */
  name: string
  /** 备注 */
  remark?: string | null
}

export interface DatabaseGetOutput {
  dbKey?: string | null
  type?: string | null
}

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
export interface PageInputCodeGroupDemoGetPageInput {
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
  /** 模板演示分页查询条件输入 */
  filter?: CodeGroupDemoGetPageInput
}

/** 分页信息输入 */
export interface PageInputCodeGroupDetailGetPageInput {
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
  /** 模板明细分页查询条件输入 */
  filter?: CodeGroupDetailGetPageInput
}

/** 分页信息输入 */
export interface PageInputCodeGroupGetPageInput {
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
  /** 模板组分页查询条件输入 */
  filter?: CodeGroupGetPageInput
}

/** 分页信息输出 */
export interface PageOutputCodeGroupDemoGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: CodeGroupDemoGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputCodeGroupDetailGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: CodeGroupDetailGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputCodeGroupGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: CodeGroupGetPageOutput[] | null
}

/** 结果输出 */
export interface ResultOutputBaseDataGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: BaseDataGetOutput
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
export interface ResultOutputCodeGenGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: CodeGenGetOutput
}

/** 结果输出 */
export interface ResultOutputCodeGroupDemoGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 模板演示查询结果输出 */
  data?: CodeGroupDemoGetOutput
}

/** 结果输出 */
export interface ResultOutputCodeGroupDetailGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 模板明细查询结果输出 */
  data?: CodeGroupDetailGetOutput
}

/** 结果输出 */
export interface ResultOutputCodeGroupGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 模板组查询结果输出 */
  data?: CodeGroupGetOutput
}

/** 结果输出 */
export interface ResultOutputIEnumerableCodeGenGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: CodeGenGetOutput[] | null
}

/** 结果输出 */
export interface ResultOutputIEnumerableCodeGroupDemoGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: CodeGroupDemoGetListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputIEnumerableCodeGroupDetailGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: CodeGroupDetailGetListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputIEnumerableCodeGroupGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: CodeGroupGetListOutput[] | null
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
export interface ResultOutputPageOutputCodeGroupDemoGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputCodeGroupDemoGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputCodeGroupDetailGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputCodeGroupDetailGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputCodeGroupGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputCodeGroupGetPageOutput
}

/** 结果输出 */
export interface ResultOutputString {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: string | null
}
