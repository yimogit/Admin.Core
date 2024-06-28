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
  CodeGroupDemoAddInput,
  CodeGroupDemoGetListInput,
  CodeGroupDemoUpdateInput,
  PageInputCodeGroupDemoGetPageInput,
  ResultOutputBoolean,
  ResultOutputCodeGroupDemoGetOutput,
  ResultOutputIEnumerableCodeGroupDemoGetListOutput,
  ResultOutputInt64,
  ResultOutputPageOutputCodeGroupDemoGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class CodeGroupDemoApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags code-group-demo
   * @name Get
   * @summary 查询
   * @request GET:/api/dev/code-group-demo/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputCodeGroupDemoGetOutput, any>({
      path: `/api/dev/code-group-demo/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group-demo
   * @name GetList
   * @summary 列表查询
   * @request POST:/api/dev/code-group-demo/get-list
   * @secure
   */
  getList = (data: CodeGroupDemoGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableCodeGroupDemoGetListOutput, any>({
      path: `/api/dev/code-group-demo/get-list`,
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
   * @tags code-group-demo
   * @name GetPage
   * @summary 分页查询
   * @request POST:/api/dev/code-group-demo/get-page
   * @secure
   */
  getPage = (data: PageInputCodeGroupDemoGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputCodeGroupDemoGetPageOutput, any>({
      path: `/api/dev/code-group-demo/get-page`,
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
   * @tags code-group-demo
   * @name Add
   * @summary 新增
   * @request POST:/api/dev/code-group-demo/add
   * @secure
   */
  add = (data: CodeGroupDemoAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/dev/code-group-demo/add`,
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
   * @tags code-group-demo
   * @name Update
   * @summary 更新
   * @request PUT:/api/dev/code-group-demo/update
   * @secure
   */
  update = (data: CodeGroupDemoUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/code-group-demo/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group-demo
   * @name Delete
   * @summary 删除
   * @request DELETE:/api/dev/code-group-demo/delete
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
      path: `/api/dev/code-group-demo/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group-demo
   * @name SoftDelete
   * @summary 软删除
   * @request DELETE:/api/dev/code-group-demo/soft-delete
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
      path: `/api/dev/code-group-demo/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-group-demo
   * @name BatchSoftDelete
   * @summary 批量软删除
   * @request PUT:/api/dev/code-group-demo/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/code-group-demo/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
