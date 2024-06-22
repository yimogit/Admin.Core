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

import { AxiosResponse } from 'axios'
import {
  CodeGroupDetailAddInput,
  CodeGroupDetailGetListInput,
  CodeGroupDetailUpdateInput,
  PageInputCodeGroupDetailGetPageInput,
  ResultOutputBoolean,
  ResultOutputCodeGroupDetailGetOutput,
  ResultOutputIEnumerableCodeGroupDetailGetListOutput,
  ResultOutputInt64,
  ResultOutputPageOutputCodeGroupDetailGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class CodeGroupDetailApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags code-group-detail
   * @name Get
   * @summary 查询
   * @request GET:/api/dev/code-group-detail/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputCodeGroupDetailGetOutput, any>({
      path: `/api/dev/code-group-detail/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group-detail
   * @name GetList
   * @summary 列表查询
   * @request POST:/api/dev/code-group-detail/get-list
   * @secure
   */
  getList = (data: CodeGroupDetailGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableCodeGroupDetailGetListOutput, any>({
      path: `/api/dev/code-group-detail/get-list`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group-detail
   * @name GetPage
   * @summary 分页查询
   * @request POST:/api/dev/code-group-detail/get-page
   * @secure
   */
  getPage = (data: PageInputCodeGroupDetailGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputCodeGroupDetailGetPageOutput, any>({
      path: `/api/dev/code-group-detail/get-page`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group-detail
   * @name Add
   * @summary 新增
   * @request POST:/api/dev/code-group-detail/add
   * @secure
   */
  add = (data: CodeGroupDetailAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/dev/code-group-detail/add`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group-detail
   * @name Update
   * @summary 更新
   * @request PUT:/api/dev/code-group-detail/update
   * @secure
   */
  update = (data: CodeGroupDetailUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/code-group-detail/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group-detail
   * @name Delete
   * @summary 删除
   * @request DELETE:/api/dev/code-group-detail/delete
   * @secure
   */
  delete = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/code-group-detail/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group-detail
   * @name SoftDelete
   * @summary 软删除
   * @request DELETE:/api/dev/code-group-detail/soft-delete
   * @secure
   */
  softDelete = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/code-group-detail/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group-detail
   * @name BatchSoftDelete
   * @summary 批量软删除
   * @request PUT:/api/dev/code-group-detail/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/code-group-detail/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
