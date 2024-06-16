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

export interface DatabaseGetOutput {
  dbKey?: string | null
  type?: string | null
}

export interface DictionaryTreeOutput {
  /** @format int64 */
  id?: number
  /** 字典名称 */
  name?: string | null
  /** 字典编码 */
  code?: string | null
  childrens?: DictionaryTreeOutput[] | null
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
export interface ResultOutputIEnumerableDictionaryTreeOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: DictionaryTreeOutput[] | null
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
