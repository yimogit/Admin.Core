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
  PageInputThingTagGetPageInput,
  ResultOutputBoolean,
  ResultOutputIEnumerableThingTagGetListOutput,
  ResultOutputInt64,
  ResultOutputPageOutputThingTagGetPageOutput,
  ResultOutputThingTagGetOutput,
  ThingTagAddInput,
  ThingTagGetListInput,
  ThingTagUpdateInput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class ThingTagApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags thing-tag
   * @name Get
   * @summary 查询
   * @request GET:/api/homely/thing-tag/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputThingTagGetOutput, any>({
      path: `/api/homely/thing-tag/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags thing-tag
   * @name GetList
   * @summary 列表查询
   * @request POST:/api/homely/thing-tag/get-list
   * @secure
   */
  getList = (data: ThingTagGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableThingTagGetListOutput, any>({
      path: `/api/homely/thing-tag/get-list`,
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
   * @tags thing-tag
   * @name GetPage
   * @summary 分页查询
   * @request POST:/api/homely/thing-tag/get-page
   * @secure
   */
  getPage = (data: PageInputThingTagGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputThingTagGetPageOutput, any>({
      path: `/api/homely/thing-tag/get-page`,
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
   * @tags thing-tag
   * @name Add
   * @summary 新增
   * @request POST:/api/homely/thing-tag/add
   * @secure
   */
  add = (data: ThingTagAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/homely/thing-tag/add`,
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
   * @tags thing-tag
   * @name Update
   * @summary 更新
   * @request PUT:/api/homely/thing-tag/update
   * @secure
   */
  update = (data: ThingTagUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/homely/thing-tag/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags thing-tag
   * @name Delete
   * @summary 删除
   * @request DELETE:/api/homely/thing-tag/delete
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
      path: `/api/homely/thing-tag/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags thing-tag
   * @name SoftDelete
   * @summary 软删除
   * @request DELETE:/api/homely/thing-tag/soft-delete
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
      path: `/api/homely/thing-tag/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags thing-tag
   * @name BatchSoftDelete
   * @summary 批量软删除
   * @request PUT:/api/homely/thing-tag/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/homely/thing-tag/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
