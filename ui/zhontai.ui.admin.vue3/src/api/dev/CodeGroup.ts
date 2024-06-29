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
  CodeGroupAddInput,
  CodeGroupGetListInput,
  CodeGroupUpdateInput,
  PageInputCodeGroupGetPageInput,
  ResultOutputBoolean,
  ResultOutputCodeGroupGetOutput,
  ResultOutputIEnumerableCodeGroupGetListOutput,
  ResultOutputInt64,
  ResultOutputPageOutputCodeGroupGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class CodeGroupApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags code-group
   * @name Get
   * @summary 查询
   * @request GET:/api/dev/code-group/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputCodeGroupGetOutput, any>({
      path: `/api/dev/code-group/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group
   * @name GetList
   * @summary 列表查询
   * @request POST:/api/dev/code-group/get-list
   * @secure
   */
  getList = (data: CodeGroupGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableCodeGroupGetListOutput, any>({
      path: `/api/dev/code-group/get-list`,
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
   * @tags code-group
   * @name GetPage
   * @summary 分页查询
   * @request POST:/api/dev/code-group/get-page
   * @secure
   */
  getPage = (data: PageInputCodeGroupGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputCodeGroupGetPageOutput, any>({
      path: `/api/dev/code-group/get-page`,
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
   * @tags code-group
   * @name Add
   * @summary 新增
   * @request POST:/api/dev/code-group/add
   * @secure
   */
  add = (data: CodeGroupAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/dev/code-group/add`,
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
   * @tags code-group
   * @name Update
   * @summary 更新
   * @request PUT:/api/dev/code-group/update
   * @secure
   */
  update = (data: CodeGroupUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/code-group/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group
   * @name Delete
   * @summary 删除
   * @request DELETE:/api/dev/code-group/delete
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
      path: `/api/dev/code-group/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group
   * @name SoftDelete
   * @summary 软删除
   * @request DELETE:/api/dev/code-group/soft-delete
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
      path: `/api/dev/code-group/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
}
